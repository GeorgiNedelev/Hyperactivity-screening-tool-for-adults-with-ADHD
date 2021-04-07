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
    private List<InputDevice> inputDevicesList1;
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

        inputDevicesList = new List<InputDevice>();
        inputDevicesList1 = new List<InputDevice>();


        rightControllerCharactersitics = InputDeviceCharacteristics.Right | InputDeviceCharacteristics.Controller;
        HMDControllerCharacteristics = InputDeviceCharacteristics.HeadMounted;
        leftControllerCharacteristics = InputDeviceCharacteristics.Left | InputDeviceCharacteristics.Controller;


       /* Debug.LogWarning(devices.Count);


        foreach (var item in devices)
        {
            Debug.LogWarning(item.name + item.characteristics);



        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            // targetDevice2 = devices[1];
        }
        if (devices1.Count > 0)
        {
            targetDevice2 = devices1[0];

        }


        Debug.LogWarning(targetDevice.name + " controller" + targetDevice2.name + "HMD");

        RightController = new SensorClass("RightController", targetDevice);
        HMD = new SensorClass("HMD", targetDevice2);
       */

    }

    private void DeviceSetup()
    {
            
           /* InputDevices.GetDevicesWithCharacteristics(rightControllerCharactersitics, inputDevicesList);
            InputDevices.GetDevicesWithCharacteristics(HMDControllerCharacteristics, inputDevicesList);
            InputDevices.GetDevicesWithCharacteristics(leftControllerCharacteristics, inputDevicesList);

        if (inputDevicesList.Count == 2) {
            foreach (var device in inputDevicesList)
            {
                Debug.LogWarning("device name: " + device.name);
            }
            

        }
           */
        InputDevices.GetDevices(inputDevicesList1);

        Debug.LogWarning("number of decvices " + inputDevicesList1.Count);
        if (inputDevicesList1.Count  == 3)
        {
            targetDeviceHMD = inputDevicesList1[0];
            targetDeviceLeft = inputDevicesList1[1];
            targetDeviceRight = inputDevicesList1[2];


            RightController = new SensorClass("RightController", targetDeviceRight);
            HMD = new SensorClass("HMD", targetDeviceHMD);
            LeftController = new SensorClass("LeftController", targetDeviceLeft);

            foreach (var device in inputDevicesList1)
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

        targetDeviceRight.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueRight);
        targetDeviceRight.TryGetFeatureValue(CommonUsages.secondaryButton, out bool secondaryButtonValue);
        targetDeviceLeft.TryGetFeatureValue(CommonUsages.primaryButton, out bool primaryButtonValueLeft);

        if (!allDevicesFoundBool)
            DeviceSetup();



        if (primaryButtonValueRight)
        {
            RightController.GetSensorData();
            HMD.GetSensorData();
            //AddToList(targetDevice);
            once = true;
        }

        if (secondaryButtonValue)
        {
            //AddToList(targetDevice2);

           
            
            
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

