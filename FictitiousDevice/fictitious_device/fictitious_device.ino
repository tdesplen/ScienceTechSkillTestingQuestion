//Author: Tyler Desplenter
//Date: 21-02-2020

//This program mimics the requirements as specific in the skill testing question document. However, instead of creating and managing a fictitious device state,
//the device responses to commands are determined randomly.

//Variables
String command = "";
String last_move_position_command = "M0000";

void setup() {
  Serial.begin(19200);
  
}

void loop() {
  //wait for something to be sent over the serial port
  while (Serial.available() == 0) {}

  //Found a command, so read it
  command = Serial.readString();

  //Make sure the command meets the minimum length requirements, which is 2 -> "M?"
  if (command.length() > 2)
  {
    //Found a movement command
    if(command[0] == 'M')
    {
      if (command[1] == '?') //Query Move Position command
      {
        long random_response = random(0,2);
        if (random_response == 0)
        {
          Serial.print("FAIL\r\n");
        }
        else
        {
          Serial.print(last_move_position_command + "\r\n");
        }
      }
      else if (command[1] == 'S') //Query Movement State command
      {
        long random_response = random(0,3);
        if (random_response == 0)
        {
          Serial.print("FAIL\r\n");
        }
        else if (random_response == 1)
        {
          Serial.print("NOT MOVING\r\n");
        }
        else
        {
          Serial.print("MOVING\r\n");
        }
      }
      else if (command[1] >= '0' && command[1] <= '9') //Move To Position command
      {
        long random_response = random(0,2);
        if (random_response == 0)
        {
          Serial.print("FAIL\r\n");
        }
        else
        {
          last_move_position_command = command.substring(0,5);
          Serial.print("OK\r\n");
        }
      }      
    }
  }  
}
