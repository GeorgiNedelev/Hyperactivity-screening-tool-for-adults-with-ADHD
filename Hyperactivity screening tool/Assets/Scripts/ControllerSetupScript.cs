using System.Collections;
using System.Collections.Generic;

using System;
using System.Globalization;

using UnityEngine;
using UnityEngine.XR;
using System.IO;
using System.Text;


public class ControllerSetupScript : MonoBehaviour
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

    private bool allDevicesFoundBool= false;
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

        Debug.LogWarning("number of decvices " + inputDevicesList.Count);
        if (inputDevicesList.Count  == 3)
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

        

    }



    bool once = true;
    // Update is called once per frame
    void Update()
    {
        // tracks button pressed events on the controllers
        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueRight);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueLeft);

        if (!allDevicesFoundBool)
            DeviceSetup();


        // Save data locally
        if (primaryButtonValueRight)
        {
            RightController.GetSensorData();
            HMD.GetSensorData();
            //AddToList(targetDevice);
            once = true;
        }
        // Save data to a csv
        if (secondaryButtonValue)
        {
            
            if (once)
            {   
                RightController.SaveToFile();
                HMD.SaveToFile();
                LeftController.SaveToFile();

                once = false;
            }


        }
    }




   
}

