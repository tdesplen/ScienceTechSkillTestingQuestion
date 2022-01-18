//Author: Tyler Desplenter
//Date: 21-02-2020

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using SciencetechDeviceController.Model;

namespace SciencetechDeviceController.ViewModel
{
    public class DeviceViewModel : INotifyPropertyChanged
    {
        private DeviceSettings device_settings; //device settings object
        private XmlSerializer serializer; //used for loading and storing xml data
        private MainWindow main_window; //access to the main window for updating the message center
        private Device device_controller; //device controller object

        //Default constructor
        public DeviceViewModel(MainWindow _main_window)
        {
            //initialize the variables
            main_window = _main_window;
            device_controller = new Device(this);
            device_settings = new DeviceSettings();
            serializer = new XmlSerializer(typeof(DeviceSettings));
            bool status = DeserializeDeviceSettings(); //get the last device settings from the XML file
            if (status) //if this was successful
            {
                main_window.UpdateMessageCenter("Device settings loaded."); //let the user know
                device_controller.ConfigureSerialPort(device_settings); //and configure the serial port 
            }
        }

        //Expose the baudrate as a property
        public string BaudRate
        {
            get { return device_settings.BaudRate.ToString(); }
            set {
                if (value == "") //not the best solution but this avoid a crash when textbox.Text == ""
                    value = null;
                device_settings.BaudRate = Convert.ToInt32(value);
                OnPropertyChanged("BaudRate");
            }
        }

        //expose the parity as a property
        public string Parity
        {
            get { return device_settings.Parity.ToString(); }
            set
            {
                if (value == "")
                    value = null;
                device_settings.Parity= Convert.ToInt32(value);
                OnPropertyChanged("Parity");
            }
        }

        //Expose the handshaking as a property
        public string Handshaking
        {
            get { return device_settings.Handshaking.ToString(); }
            set
            {
                if (value == "")
                    value = null;
                device_settings.Handshaking = Convert.ToInt32(value);
                OnPropertyChanged("Handshaking");
            }
        }

        //Expose the stopbits as a property
        public string Stopbits
        {
            get { return device_settings.Stopbits.ToString(); }
            set
            {
                if (value == "")
                    value = null;
                device_settings.Stopbits = Convert.ToInt32(value);
                OnPropertyChanged("Stopbits");
            }
        }

        //Expose the databits as a property
        public string Databits
        {
            get { return device_settings.Databits.ToString(); }
            set
            {
                if (value == "")
                    value = null;
                device_settings.Databits = Convert.ToInt32(value);
                OnPropertyChanged("Databits");
            }
        }

        //Expose the termination character as a property
        public string TerminationCharacter
        {
            get { return device_settings.TerminationCharacter; }
            set
            {
                device_settings.TerminationCharacter = value;
                OnPropertyChanged("TerminationCharacter");
            }
        }

        //Expose the portname as a property
        public string PortName
        {
            get { return device_settings.PortName; }
            set
            {
                device_settings.PortName = value;
                OnPropertyChanged("PortName");
            }
        }


        public event PropertyChangedEventHandler PropertyChanged; 

        //Property Changed handler function
        protected void OnPropertyChanged(string property_name)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
            {
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property_name));
            }
        }
        
        //A function for loading the device settings from an XML file
        public bool DeserializeDeviceSettings()
        {
            bool status = false;
            try
            {
                using (Stream reader = new FileStream("..\\..\\settings.xml", FileMode.Open))
                {
                    device_settings = (DeviceSettings)serializer.Deserialize(reader); //get the settings
                }
                device_controller.ConfigureSerialPort(device_settings); //configure the serial connection
                status = true;
            }
            catch(Exception e)
            {
                UpdateMessageCenter(e.Message);
            }
            return status;
        }

        //A function for store the device settings as XML data
        public bool SerializeDeviceSettings()
        {
            bool status = false;
            try
            {
                using (TextWriter writer = new StreamWriter("..\\..\\settings.xml"))
                {
                    serializer.Serialize(writer, device_settings); //save the data
                }
                status = true;
            }
            catch(Exception e)
            {
                UpdateMessageCenter(e.Message);
            }
            return status;
        }

        //A function for opening communication with the device
        public void OpenCommunication()
        {
            if (device_controller.State == 1)
                device_controller.OpenCommunication();
        }

        //A function for closing communication with the device
        public void CloseCommunication()
        {
            if (device_controller.State == 2)
                device_controller.CloseCommunication();
        }

        //A function for requesting the sending of the MXXXX command
        public void MoveToPosition(string position)
        {
            if (device_controller.State == 2)
                device_controller.SendCommand("M" + position);
            else
                UpdateMessageCenter("Cannot send command. The device communication must be opened first.");
        }

        //A functtion to request the sending of the M? command
        public void QueryMovePosition()
        {
            if (device_controller.State == 2)
                device_controller.SendCommand("M?");
            else
                UpdateMessageCenter("Cannot send command. The device communication must be opened first.");
        }

        //A function to request the sending of the MS? command
        public void QueryMovementState()
        {
            if (device_controller.State == 2)
                device_controller.SendCommand("MS?");
            else
                UpdateMessageCenter("Cannot send command. The device communication must be opened first.");
        }

        //A function for updating the UI elements that are connected through data bindings
        public void UpdateSettingsUI()
        {
            OnPropertyChanged("BaudRate");
            OnPropertyChanged("Parity");
            OnPropertyChanged("Handshaking");
            OnPropertyChanged("Stopbits");
            OnPropertyChanged("Databits");
            OnPropertyChanged("TerminationCharacter");
            OnPropertyChanged("PortName");
        }

        //A function for updating the message center on the main window
        public void UpdateMessageCenter(string message)
        {
            main_window.UpdateMessageCenter(message);
        }
    }
}
