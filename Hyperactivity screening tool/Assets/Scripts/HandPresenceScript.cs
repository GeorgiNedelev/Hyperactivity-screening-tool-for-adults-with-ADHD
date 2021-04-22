using System.Collections;
using System.Collections.Generic;

using System;
using System.Globalization;

using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System.Text;


public class HandPresenceScript : MonoBehaviour
{
    private List<InputDevice> inputDevicesList;

    private InputDevice targetDeviceRight;
    private InputDevice targetDeviceHMD;
    private InputDevice targetDeviceLeft;

    public SensorClass RightController;
    public SensorClass HMD;
    public SensorClass LeftController;


    private InputDeviceCharacteristics rightControllerCharactersitics;
    private InputDeviceCharacteristics HMDControllerCharacteristics;
    private InputDeviceCharacteristics leftControllerCharacteristics;

    private string path;
    private bool allDevicesFoundBool = false;
    // Input and data collection control
    private float testDuration = 10;
    private float timeOfTestStart;
    private int gameSwitch = 1;


    public Animator catAnimator;
    CatMotion catScript;
    GameObject theCat;
   
    public float catAnimTime =1;
    public float startAnimationTime;
    public float animationPlayedTime;
    private float stopAnimationTime;
    private GameObject CPTcanvas;
    private CPT_func CPT_script;
    GameObject testFinished;
    public void Start()
    {
        // Instatiate the devices list and differentiate the controller characteristics
        inputDevicesList = new List<InputDevice>();

        rightControllerCharactersitics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        HMDControllerCharacteristics = InputDeviceCharacteristics.HeadMounted;
        leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;

    }

    // Makes an intance of a sensorClass for each controller found
    private void DeviceSetup()
    {

        // Rreturns all devices tracked.  Used for safety, makes sure all devices are tracked before assigning them
        InputDevices.GetDevices(inputDevicesList);

       // Debug.LogWarning("number of decvices " + inputDevicesList.Count);
        if (inputDevicesList.Count == 3)
        {
            targetDeviceHMD = inputDevicesList[0];
            targetDeviceLeft = inputDevicesList[1];
            targetDeviceRight = inputDevicesList[2];


            RightController = new SensorClass("RightController", targetDeviceRight);
            HMD = new SensorClass("HMD", targetDeviceHMD);
            LeftController = new SensorClass("LeftController", targetDeviceLeft);

            foreach (var device in inputDevicesList)
            {
                Debug.LogWarning("device name: " + device.name);
            }
            allDevicesFoundBool = true;
        }

        CPTcanvas = GameObject.Find("Text");
        CPT_script = CPTcanvas.GetComponent<CPT_func>();
        testFinished = GameObject.Find("Text1");
        testFinished.SetActive(false);
        theCat = GameObject.Find("Cat");
        catScript = theCat.GetComponent<CatMotion>();
        catAnimator = theCat.GetComponent<Animator>();



    }




    // Update is called once per frame
    void Update()
    {
        // tracks button pressed events on the controllers
        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueRight);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueLeft);
        // Making sure that all devices are tracked
        if (!allDevicesFoundBool)
            DeviceSetup();


        // Button functionality(optional)
        if (primaryButtonValueRight)
        {

            //AddToList(targetDevice);

        }
        
        if (secondaryButtonValue)
        {
          

        }
        // This switch statement controls the flow of the game
        catScript.CatMoves();

        switch (gameSwitch)
        {
            case 1:
                // Listens to a button press to save the time and move on to data collection
                if (secondaryButtonValue)
                {
                     timeOfTestStart = Time.realtimeSinceStartup;
                     Debug.LogWarning("The test Begins" + Time.realtimeSinceStartup.ToString());
                     CPT_script.cptStart = true;
                     gameSwitch = 2;
                     startAnimationTime= Time.realtimeSinceStartup;
                }
                break;
            case 2:
                //Data geathering 
                LeftController.GetSensorData();
                RightController.GetSensorData();
                HMD.GetSensorData();
                
                //Cat animation
                if(Time.realtimeSinceStartup > startAnimationTime + 60.0f && catAnimator.IsInTransition(0) )
                {
                    catAnimator.SetBool("walkingBool", true);
                    catAnimator.SetBool("idleBool", false);
                   
                    Debug.LogWarning("catMoves");
                    startAnimationTime = Time.realtimeSinceStartup;

                    theCat.GetComponent<AudioSource>().Play();
                    
                }
                if (catAnimator.GetCurrentAnimatorStateInfo(0).length > catAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime && Time.realtimeSinceStartup > startAnimationTime + 6.0f && catAnimator.GetBool("walkingBool") == true)
                {   
                    catAnimator.SetBool("walkingBool", false);
                    catAnimator.SetBool("idleBool", true);
                    Debug.LogWarning("Finished MOving");
                }



                // when the test time is over save the files to csv files
                if (Time.realtimeSinceStartup > timeOfTestStart + testDuration)
                {
                    Debug.LogWarning("The test ended" + Time.realtimeSinceStartup.ToString());
                    PathConfig();
                    RightController.SaveToFile(path);
                    HMD.SaveToFile(path);
                    LeftController.SaveToFile(path);
                    CPT_script.cptStart = false;
                    CPTcanvas.SetActive(false);
                    testFinished.gameObject.SetActive(true);
                    gameSwitch = 1;
                }
                 break;


        }


    }

    
    // Checking if a directory exist in a particular path, if it does it increments the participantID
    // until a folder with an id does not exist and creates it to save the sensor data in it
    public void PathConfig()
    {

        int participantID = 0;

        path = @"C:\Data\DataFromParticipiant";


        while (true)
        {

            string path1 = path + participantID.ToString();
            if (!Directory.Exists(path1))
            {
                path = path1;
                break;

            }
            participantID++;

        }

        participantID = 0;
        Debug.LogWarning("Path created at" + path);
        DirectoryInfo di = Directory.CreateDirectory(path);

    }



}

