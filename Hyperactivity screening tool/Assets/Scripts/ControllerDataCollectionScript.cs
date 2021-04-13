using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using System;
using UnityEngine.XR;




public class SensorClass : MonoBehaviour
{

    public string sensorName;
    private InputDevice sensorDevice;
    public List<Vector3> Position_list;
    public List<Vector3> Velocity_list;
    public List<Vector3> Acceleration_list;
    public List<Vector3> Angular_Velocity_list;
    public List<Vector3> Angular_Acceleration_list;
    public List<List<Vector3>> MeasurementsCollectiveList;

    public float timeStep = 1.0f;
    public float time1 = 0.0f;
    public int sec = 1;
    
    public SensorClass(string SensorName,InputDevice Sensor)
    {
        sensorName = SensorName;
        sensorDevice = Sensor;
        Position_list = new List<Vector3>();
        Velocity_list = new List<Vector3>();
        Acceleration_list = new List<Vector3>();
        Angular_Acceleration_list = new List<Vector3>();
        Angular_Velocity_list = new List<Vector3>();
        MeasurementsCollectiveList = new List<List<Vector3>>();
        
    }





    //Get and Save sensor data
    public void GetSensorData()
    {

        sensorDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 deviceVelocity);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAcceleration, out Vector3 deviceAcceleration);
        //angular
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration, out Vector3 angularAcceleration);
        // time control
        if (time1 < Time.time)
        {


            
            time1 = Time.time + timeStep;
            Debug.LogWarning("Inputs in a second" + Position_list.Count / Time.time);
            
            //Debug.LogWarning("Inputs " + Position_list.Count );
            //Debug.LogWarning(" second :" +  sec);
            sec ++;
        }
        SensorSaveDataToList(Position_list, devicePosition);
        SensorSaveDataToList(Velocity_list, deviceVelocity);
        SensorSaveDataToList(Acceleration_list, deviceAcceleration);
        SensorSaveDataToList(Angular_Acceleration_list, angularVelocity);
        SensorSaveDataToList(Angular_Velocity_list, angularAcceleration);

        
    }

    

    //Adding data to a sensor parameter list
    public void SensorSaveDataToList(List<Vector3> dataList, Vector3 dataToSave)
    {
        if (dataList.Count > 0)
        {

            // if (dataList[dataList.Count - 1].ToString() != dataToSave.ToString())

            
                dataList.Add(dataToSave);
                //Debug.LogWarning("Added Value" + dataToSave);

                // Debug.LogWarning("last data point :" + RightControllerData_Acceleration[RightControllerData_Acceleration.Count - 1] + "added data point" + devicePosition );

                //Debug.LogWarning("Discarded value" + dataToSave);
               
            
        }
        else
        {
            dataList.Add(dataToSave);
        }

        

    }
    
    public void SaveToFile()
    {
        MeasurementsCollectiveList.Add(Position_list);
        MeasurementsCollectiveList.Add(Velocity_list);
        MeasurementsCollectiveList.Add(Acceleration_list);
        MeasurementsCollectiveList.Add(Angular_Acceleration_list);
        MeasurementsCollectiveList.Add(Position_list);
        string[] listNames = { "Position_list", "Velocity_list", "Acceleration_list", "Angular_Acceleration_list", "Position_list" };

        for (int i = 0; i < MeasurementsCollectiveList.Count; i++)
        {
        string path = Application.persistentDataPath ;
        string strFilePath = ( path + sensorName + listNames[i]+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
            //@"C:\Data\"
            string strSeperator = ",";
        StringBuilder sbOutput = new StringBuilder();
        ;
        sbOutput.AppendLine(string.Join(strSeperator, "X", "Y", "Z"));
        foreach (Vector3 input in MeasurementsCollectiveList[i])
               {
            sbOutput.AppendLine(string.Join(strSeperator, input.x, input.y, input.z));

                 }

        File.WriteAllText(strFilePath, sbOutput.ToString());
        MeasurementsCollectiveList[i].Clear();
        }
        MeasurementsCollectiveList.Clear();
    }

}
