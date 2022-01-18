//Author: Tyler Desplenter
//Date: 21-02-2020

using SciencetechDeviceController.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace SciencetechDeviceController
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private DeviceViewModel DVM; //Our ViewModel

        //Default Constructor
        public MainWindow()
        {
            InitializeComponent(); //.NET UI setup
            DVM = new DeviceViewModel(this); //create a new DeviceViewModel object
            this.DataContext = DVM; //Set the DataContext to our DeviceViewModel
        }

        //Load Settings button click handler
        private void LoadSettings_Click(object sender, RoutedEventArgs e)
        {
            bool status = DVM.DeserializeDeviceSettings(); //Deserialize the device settings from an XML file
            if (status) //if it was successful
            {
                UpdateMessageCenter("Device settings loaded."); //Let the user know
                DVM.UpdateSettingsUI(); //Update the UI elements
            }
        }

        //Save Settings button click handler
        private void SaveSettings_Click(object sender, RoutedEventArgs e)
        {
            bool status = DVM.SerializeDeviceSettings(); //Save the device settings to the XML file
            if (status) //if it was successful
            {
                UpdateMessageCenter("Device settings saved."); //Let the user know
            }
        }

        //A function for writing messages to the message center
        public void UpdateMessageCenter(string message)
        {
            message_center.AppendText(message + "\r\n");
            message_center.ScrollToEnd();
        }

        //Open Communication button click handler
        private void OpenCommunication_Click(object sender, RoutedEventArgs e)
        {
            DVM.OpenCommunication(); //Pass the message along
        }
        
        //Close Communication button click handler
        private void CloseCommunication_Click(object sender, RoutedEventArgs e)
        {
            DVM.CloseCommunication(); //pass the message along
        }

        //Move To Position button click handler
        private void MoveToPosition_Click(object sender, RoutedEventArgs e)
        {
            //It is assumed that the textBox1.Text.Length will be <= 4
            string position= null;
            for (int i = 0; i < 4 - textBox1.Text.Length; i++) //for each character that lenght is less than 4
            {
                position += "0"; //pad the position with a 0 at the front of the string
            }
            position += textBox1.Text; //add the user's desired position to the end
            DVM.MoveToPosition(position); //call the DVM to handle this movement request
        }

        //Query Move Position button click handler
        private void QueryMovePosition_Click(object sender, RoutedEventArgs e)
        {
            DVM.QueryMovePosition(); //call the DVM to handle this request
        }

        //Query Movement State button click handler
        private void QueryMovementState_Click(object sender, RoutedEventArgs e)
        {
            DVM.QueryMovementState(); //call the DVM to handle this request
        }

        //Desired Motor Position textbox text changed handler
        private void TextBox1_TextChanged(object sender, TextChangedEventArgs e)
        {
            //Do error checking here for the appropriate 
            if (textBox1.Text == "")
                textBox1.Text = "0000"; //turn empty strings into zeros
            if (textBox1.Text.Length > 4)
                textBox1.Text = textBox1.Text.Substring(0, 4); //if the user tries to enter in more than 4 digits, shorten it
            for (int i = 0; i < textBox1.Text.Length; i++) //for every character in the position text
            {
                char temp = textBox1.Text[i]; //convert to a char
                if (!IsDigit(temp)) //if the char is not a digit
                {
                    textBox1.Text = "0000"; //change it to the default value
                    UpdateMessageCenter("Motor position must be within the range of 0000 and 9999."); //and let the user know
                }
            }
        }

        //A function to determine if a character is a numerical value between 0 and 9
        private bool IsDigit(char value)
        {
            if (value < '0' || value > '9')
                return false;
            return true;
        }
    }
}
