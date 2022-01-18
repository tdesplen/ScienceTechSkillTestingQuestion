//Author: Tyler Desplenter
//Date: 21-02-2020

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.IO.Ports;
using SciencetechDeviceController.Model;
using SciencetechDeviceController.ViewModel;

namespace SciencetechDeviceController
{
    public class Device
    {
        private SerialPort device_communication; //need a serial port to talk with the device
        private DeviceViewModel DVM; //DVm object, so we can update the UI through it
        private int state; //communication state: 0 - not configured, 1 - configured but communication closed, 2 - communication open
        private DeviceSettings current_settings; //current communication settings

        //Default constructor
        public Device(DeviceViewModel _DVM)
        {
            device_communication = new SerialPort(); //create a new serial port
            DVM = _DVM;
            state = 0; //always start at state 0
        }
        
        //Exposing the state variable as a property for the DVM to access
        public int State
        {
            get { return state; }
            set { state = value; }
        }

        //A function for updating the device settings after a change
        public void ConfigureSerialPort(DeviceSettings _settings)
        {
            current_settings = _settings;
            device_communication.BaudRate = current_settings.BaudRate;
            device_communication.Parity = (Parity)current_settings.Parity;
            device_communication.Handshake = (Handshake)current_settings.Handshaking;
            device_communication.StopBits = (StopBits)current_settings.Stopbits;
            device_communication.DataBits = current_settings.Databits;
            device_communication.PortName = current_settings.PortName;
            state = 1; //change the state
        }

        //A function for opening the serial communication with the device
        public void OpenCommunication()
        {
            try
            {
                if (!device_communication.IsOpen) //check to see that is isn't already open
                    device_communication.Open();
                state = 2; //change the state
                DVM.UpdateMessageCenter("Device connection open on port: " + current_settings.PortName);
            }
            catch(Exception e)
            {
                DVM.UpdateMessageCenter(e.Message);
            }
        }

        //A function for closing the serial communication with the device
        public void CloseCommunication()
        {
            try
            {
                if (device_communication.IsOpen) //check to see that the communication is open
                    device_communication.Close();
                state = 1; //change the state
                DVM.UpdateMessageCenter("Device connection was closed.");
            }
            catch (Exception e)
            {
                DVM.UpdateMessageCenter(e.Message);
            }
        }

        //A function for sending commands to the device
        public void SendCommand(string message)
        {
            try
            {
                if (device_communication.IsOpen) 
                {
                    device_communication.Write(message + current_settings.TerminationCharacter + "\n"); //write the command
                    DVM.UpdateMessageCenter("Sent Command: " + message); //let the use rknow what was sent
                    Thread.Sleep(2000); //give the device time to send a response
                    ReadResponse(); //tell the user what was received in response to the command
                }
            }
            catch (Exception e)
            {
                DVM.UpdateMessageCenter(e.Message);
            }
        }

        //A function for reading the responses of the device
        public string ReadResponse()
        {
            string message = null;
            try
            {
                if (device_communication.IsOpen && device_communication.BytesToRead > 0) //if communication is open and there are bytes to read
                {
                    message = device_communication.ReadLine(); //read the message
                    DVM.UpdateMessageCenter("Device Response: " + message.Substring(0,message.Length - 1)); //let the user know what was received
                }
            }
            catch (Exception e)
            {
                DVM.UpdateMessageCenter(e.Message);
            }
            return message;
        }
    }
}
