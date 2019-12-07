/*
 * ~~~~~~~~~~VR Glove 2 Arduino Sketch~~~~~~~~~~
 *  0.0.9
 * 
 * This program handles the input and output required 
 * for the VR glove.
 *
 *  ***THIS CODE IS A WORK IN PROGRESS***
 * To-do list:
 *  - change the nomenclature of the vibration motor pin to be more generic  
 *
 * New in this version:
 *  - fixed intermittent stalling durin gyro initialization
 * 
 * The code is designed to work with a project-specific 
 * shield to be placed on an Arduino MEGA. 
 * More details coming in a future update.
 * 
 * Latest update: December 7, 2019
 * 
 * Zakhar Kanyuka - CS 207 
 * VR Glove Project
 *
 * 
 */


#include<Servo.h>
#include <Wire.h>
#include <Adafruit_Sensor.h>
#include <Adafruit_L3GD20_U.h>

// general global varible declarations
const int BAUD = 9600; // baud rate for serial communication
const int LOOP_DELAY = 50; // delay between loop iterations
  // set to 500 ms for debugging purposes

// servo declarations
Servo servos[5]; // thumb to pinky
const int servo_pins[] = {6, 5, 4, 3, 2}; // thumb to pinky

// vibration motor declarations
const int vib_pin = 7;
bool vib_state;

// current sensor declarations
const int isensor_pins[] = {A0, A1, A2, A3, A4}; // thumb to pinky  

// gyroscope object declaration
Adafruit_L3GD20_Unified gyro[6];
const int gyro_addr[] = {2, 3, 4, 5, 6, 7}; // correspond to the addresses on the MUX

void setup() 
{
  // serial communication start
  Serial.begin(BAUD);

  Serial.println("Setup initialized.");
  
  // servo attachment
  for(int i = 0; i < 5; i++)
  {
    servos[i].attach(servo_pins[i]);
  }

  ////////////////////////////////////////////////////////
  // A block of code adapted from an I2C scanner by 
  // gist.github.com/riyas-org
  //
  // The piece of code appears to relieve stalling issues 
  // likely caused by the I2C mux
  ////////////////////////////////////////////////////////
  byte error, address;
  int nDevices;

  for(int MUX_address = 0; MUX_address < 8; MUX_address++)
  {

    MUXSelect(MUX_address);
    nDevices = 0;
    for(address = 1; address < 127; address++ ) 
    {
    // The i2c_scanner uses the return value of
    // the Write.endTransmisstion to see if
    // a device did acknowledge to the address.
    Wire.beginTransmission(address);
    error = Wire.endTransmission();
   
    } 
    delay(50);           // wait before next scan
  }
  ////////////////////////////////////////////////////////

  // gyro initialization
  for(int i = 0; i < 6; i++)
  {
    MUXSelect(gyro_addr[i]);
    gyro[i] = Adafruit_L3GD20_Unified(20);
    gyro[i].enableAutoRange(true);
    //Serial.println("before begin");
    gyro[i].begin(); // This line was the culprit of the 
					 //intermittent stalling that 
					 // necessitates the above excerpt 
					 // from the I2C scanner
    //Serial.println("after begin");
  }

  
  Serial.println("Setup complete.");  
}

void loop()
{
  int servo_positions[5]; // holds desired servo positions
  readHostInstruction(servo_positions);
  positionServos(servos, servo_positions);
  applyVibState(vib_state);
  
  int isensor_data[5]; // holds current (I) sensor data
  readCurrent(isensor_data);
  
  int gyro_data[6][3]; // holds gyro data
  readGyros(gyro_data);
  
  writeToHost(buildOutputToHost(isensor_data, gyro_data)); 
  
}

// Reads instructions from host, processes them, and stores them in appropriate containers
// (just handles servo positions for now)
//
// input format: s#0#,#1#,#2#,#3#,#4#,...e
void readHostInstruction(int servo_positions[])
{
  
  if(!Serial.available()) // ensures that serial data available
    return;
  
  String hostInput = Serial.readString();
  
  if(!((hostInput.charAt(0) == 's') && (hostInput.charAt(hostInput.length()-1) == 'e')))
    // ensures data received is a complete string in the appropriate format
    // if not, returns from the function
  return;
  
  int count = 0;
  
  do
  {
    int commaPosition = hostInput.indexOf(',');
    String numString = hostInput.substring(1, commaPosition);
    hostInput = hostInput.substring(commaPosition, hostInput.length());
    
  if(count <= 4)
    servo_positions[count] = atoi(numString.c_str());
  else
    vib_state = atoi(numString.c_str());
    
    count ++;
  } while (count < 6);
}

// Reads current and stores data in the parameter array
void readCurrent(int isensor_data[])
{
  for(int i = 0; i < 5; i++)
  {
    isensor_data[i] = analogRead(isensor_pins[i]);
  }
}

// Reads gyros and stores data in the parameter array
void readGyros(int gyro_data[][3])
{
  // This function will be completed once the setback surrounding the I2C hardware is resolved.
  // For now it mirrors the single gyro signal to all gyros in the array
  sensors_event_t event; 

  for(int i = 0; i < 6; i++)
  {
      MUXSelect(gyro_addr[i]);
      
      gyro[i].getEvent(&event);
      
      gyro_data[i][0] = event.gyro.x * 100;
      gyro_data[i][1] = event.gyro.y * 100;
      gyro_data[i][2] = event.gyro.z * 100; 
    

    
  }
  
  
}

// Writes positions to servos
void positionServos(Servo servos[], int servo_positions[])
{
  for(int i = 0; i < 5; i++)
  {
    servos[i].write(servo_positions[i]);
  }
}

// Writes state to vibration motor
void applyVibState(bool vib_state)
{
  digitalWrite(vib_pin, vib_state);
}

// Builds output string 
// output format: s#0#,#1#,#2#,#3#,#4#,e
String buildOutputToHost(int isensor_data[], int gyro_data[][3])
{
  String outputToHost = "";
  
  outputToHost = outputToHost + 's';
  
  for(int i = 0; i < 5; i++)
  {
    outputToHost = outputToHost + isensor_data[i];
    outputToHost = outputToHost + ',';
  }

  for(int i = 0; i < 6; i++)
  {
    outputToHost = outputToHost + gyro_data[i][0];
    outputToHost = outputToHost + ',';
    outputToHost = outputToHost + gyro_data[i][1];
    outputToHost = outputToHost + ',';
    outputToHost = outputToHost + gyro_data[i][2];
    outputToHost = outputToHost + ',';
    
  }
  
  // further concatenation can be added here
  
  outputToHost = outputToHost + 'e';

  return outputToHost;
}

// Delivers string to host over serial
void writeToHost(String outputToHost)
{
  Serial.println(outputToHost);
}

// Selects address for I2C MUX
// This function and other MUX manipulation highly inspired by:
// https://learn.adafruit.com/adafruit-tca9548a-1-to-8-i2c-multiplexer-breakout/wiring-and-test
#define TCAADDR 0x70
void MUXSelect(uint8_t i)
{
  if(i > 7) return;
  
  Wire.beginTransmission(TCAADDR);
  Wire.write(1 << i);
  Wire.endTransmission();
}
