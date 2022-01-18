Skill Testing Question - README

Description
---------------
In this submission, I have developed both a C# WPF application, SciencetechDeviceController, and an Arduino application, 
FictitiousDevice. The SciencetechDeviceController uses the MVVM architecture (or as best I could understand it given the time limits of this task). The FictitiousDevice application mimics the command/responses laid out in the skill testing questions document, but the responses are generated randomly for testing purposes.

Testing
---------------
Both the SciencetechDeviceController and the FiticiousDevice applications have been tested together for basic functionality. However, not all pieces of code were extensively, which means that there may be error cases present. The 'Message Center' of the SciencetechDeviceController relays all information regarding UI interactions. In order to test the applications, perform the following steps:
1) Open the fictitious_device file and compile/upload it to an Arduino microcontroller board
2) Open the SciencetechDeviceController solution, build it, and run it
3) Within the SciencetechDeviceController, change the 'Port Name' to the approriate COM port name, and press the 'Save Settings' button to save this configuration data
4) Within the SciencetechDeviceController, press the 'Open Communication' button to open the serial port connection
5) Within the SciencetechDeviceController, press any of the 'Move To Position', 'Query Move Position', or 'Query Movement State' buttons to send command to the Arduino and receive responses. 


