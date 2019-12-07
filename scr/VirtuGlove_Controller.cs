/*
 * ~~~~~~~~~~VR Glove 2 Unity Sketch~~~~~~~~~~
 *  0.0.5
 * 
 * This program handles the input and output required 
 * for the VR glove.
 *
 *  ***THIS CODE IS A WORK IN PROGRESS***
 * To-do list:
 *  - implementaion with SteamVR hand
 *    Must be implemented.
 *  - inspector view of variables
 *    Must be better organized.
 *  - documentation
 *    More detail and instruction is needed on setup.
 *	- overall
 *	  Refine serial communication to work consistently.
 *
 * New in this version:
 *  - made some refinements on serial communication and threading implementation
 * 
 * The code is designed to work with a project-specific 
 * shield to be placed on an Arduino MEGA. 
 * More details coming in a future update.
 * 
 * Latest update: December 2, 2019
 * 
 * Zakhar Kanyuka - CS 207 
 * VR Glove Project
 *
 * 
 */

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

using System.IO.Ports; // worked in the past

public class VirtuGlove_Controller : MonoBehaviour
{
    private bool threadRunning = false;

    // delcarations of gyro input signal arrays
    public int[] Gyro0 = new int[3];
    public int[] Gyro1 = new int[3];
    public int[] Gyro2 = new int[3];
    public int[] Gyro3 = new int[3];
    public int[] Gyro4 = new int[3];
    public int[] Gyro5 = new int[3];

    // declarations of current sensor input signal variables
    public int Curr0;
    public int Curr1;
    public int Curr2;
    public int Curr3;
    public int Curr4;

    // declarations of servo output signal variables
    [Range(0, 180)] public int Servo0;
    [Range(0, 180)] public int Servo1;
    [Range(0, 180)] public int Servo2;
    [Range(0, 180)] public int Servo3;
    [Range(0, 180)] public int Servo4;

    // delcaration of the the vibration motor output signal variable
    public int Vib;

    // declaration of key manual joints
    public Transform jntThu1;
    public Transform jntThu2;

    public Transform jntInd0;
    public Transform jntInd1;
    public Transform jntInd2;

    public Transform jntMid0;
    public Transform jntMid1;
    public Transform jntMid2;

    public Transform jntRin0;
    public Transform jntRin1;
    public Transform jntRin2;

    public Transform jntPin0;
    public Transform jntPin1;
    public Transform jntPin2;

    const string SERIAL_PORT = "COM3";
    const int SERIAL_BAUD = 9600;

    SerialPort serialStream = new SerialPort(SERIAL_PORT, SERIAL_BAUD);

    private Thread gloveSignalRead;


    // Start is called before the first frame update
    void Start()
    {
        serialStream.Open();
        serialStream.ReadTimeout = 10000;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        /*
        Servo0 = 2;
        Servo1 = 4;
        Servo2 = 8;
        Servo3 = 16;
        Servo4 = 32;
        Vib = 64;
        */

        gloveSignalRead = new Thread(ReadAndParse);
        

        if (!threadRunning)
        {
            gloveSignalRead.Start();
        }

        ConcAndSignal();
        
    }

	
	// Reads an input string over serial and parses it to the script's variables
    private void ReadAndParse()
    {
        threadRunning = true; // Triggers a flag inidcating that a thread is running. 
        Debug.Log("Thread begun.");

        string serialInput = serialStream.ReadLine();
        Debug.Log("Read string:");
        Debug.Log(serialInput);

        if (!((serialInput[0] == 's') && (serialInput[(serialInput.Length - 1)] == 'e')))
        {
            threadRunning = false; // Triggers a flag inidcating that a thread is no longer running. 
            Debug.Log("Thread stopped.");
            return;
        }


        //string serialInput = "s2,4,8,16,32,e";

        int count = 0;

        do
        {
            int commaPosition = serialInput.IndexOf(',', 1);
            string num_serialInput = serialInput.Substring(1, (commaPosition - 1));
            serialInput = serialInput.Substring(commaPosition);

            //Debug.Log(commaPosition);
            // Debug.Log(num_serialInput);

            switch (count)
            {
                // Current sensors
                case 0:
                    Curr0 = int.Parse(num_serialInput);
                    break;
                case 1:
                    Curr1 = int.Parse(num_serialInput);
                    break;
                case 2:
                    Curr2 = int.Parse(num_serialInput);
                    break;
                case 3:
                    Curr3 = int.Parse(num_serialInput);
                    break;
                case 4:
                    Curr4 = int.Parse(num_serialInput);
                    break;
                // Gyros
                // Gyro0
                case 5:
                    Gyro0[0] = int.Parse(num_serialInput);
                    break;
                case 6:
                    Gyro0[1] = int.Parse(num_serialInput);
                    break;
                case 7:
                    Gyro0[2] = int.Parse(num_serialInput);
                    break;
                // Gyro1
                case 8:
                    Gyro1[0] = int.Parse(num_serialInput);
                    break;
                case 9:
                    Gyro1[1] = int.Parse(num_serialInput);
                    break;
                case 10:
                    Gyro1[2] = int.Parse(num_serialInput);
                    break;
                // Gyro2
                case 11:
                    Gyro2[0] = int.Parse(num_serialInput);
                    break;
                case 12:
                    Gyro2[1] = int.Parse(num_serialInput);
                    break;
                case 13:
                    Gyro2[2] = int.Parse(num_serialInput);
                    break;
                // Gyro3
                case 14:
                    Gyro3[0] = int.Parse(num_serialInput);
                    break;
                case 15:
                    Gyro3[1] = int.Parse(num_serialInput);
                    break;
                case 16:
                    Gyro3[2] = int.Parse(num_serialInput);
                    break;
                // Gyro4
                case 17:
                    Gyro4[0] = int.Parse(num_serialInput);
                    break;
                case 18:
                    Gyro4[1] = int.Parse(num_serialInput);
                    break;
                case 19:
                    Gyro4[2] = int.Parse(num_serialInput);
                    break;
                // Gyro5
                case 20:
                    Gyro5[0] = int.Parse(num_serialInput);
                    break;
                case 21:
                    Gyro5[1] = int.Parse(num_serialInput);
                    break;
                case 22:
                    Gyro5[2] = int.Parse(num_serialInput);
                    break;
            }

            count++;

        } while (count < 22);
        //

        threadRunning = false; // Triggers a flag inidcating that a thread is no longer running. 
        Debug.Log("Thread stopped.");
    }

	// Concatenates a host signal string and sends it over serial port
    void ConcAndSignal()
    {
        string serialOutput = "s";

        serialOutput += Servo0;
        serialOutput += ',';
        serialOutput += Servo1;
        serialOutput += ',';
        serialOutput += Servo2;
        serialOutput += ',';
        serialOutput += Servo3;
        serialOutput += ',';
        serialOutput += Servo4;
        serialOutput += ',';
        serialOutput += Vib;
        serialOutput += ',';

        serialOutput += 'e';

        serialStream.Write(serialOutput);

        //Debug.Log(serialOutput);

    }

	// In this function we wll implement the transformations 
    // to the hand model based on the gyro signals.
    void ApplyHandModel()
    {
        // In this function we wll implement the transformations 
        // to the hand model based on the gyro signals.
        
        jntInd0.localRotation = Quaternion.Euler(0, 0, 0);
        jntInd1.localRotation = Quaternion.Euler(0, 0, 0);
        jntInd2.localRotation = Quaternion.Euler(0, 0, 0);

        
        jntMid0.localRotation = Quaternion.Euler(0, 0, 0);
        jntMid1.localRotation = Quaternion.Euler(0, 0, 0);
        jntMid2.localRotation = Quaternion.Euler(0, 0, 0);

        
        jntRin0.localRotation = Quaternion.Euler(0, 0, 0);
        jntRin1.localRotation = Quaternion.Euler(0, 0, 0);
        jntRin2.localRotation = Quaternion.Euler(0, 0, 0);

        
        jntPin0.localRotation = Quaternion.Euler(0, 0, 0);
        jntPin1.localRotation = Quaternion.Euler(0, 0, 0);
        jntPin2.localRotation = Quaternion.Euler(0, 0, 0);

        
        //jntThu0.localRotation = Quaternion.Euler(0, 0, 0);
        jntThu1.localRotation = Quaternion.Euler(0, 0, 0);
        jntThu2.localRotation = Quaternion.Euler(0, 0, 0);
    }
}


