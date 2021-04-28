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
    public List<Vector3> Rotation_list;
    public List<List<Vector3>> MeasurementsCollectiveList;
    public List<float> TimeStamp;
    
    public SensorClass(string SensorName,InputDevice Sensor)
    {
        sensorName = SensorName;
        sensorDevice = Sensor;
        Position_list = new List<Vector3>();
        Velocity_list = new List<Vector3>();
        Acceleration_list = new List<Vector3>();
        Angular_Acceleration_list = new List<Vector3>();
        Angular_Velocity_list = new List<Vector3>();
        Rotation_list = new List<Vector3>();
        MeasurementsCollectiveList = new List<List<Vector3>>();
        TimeStamp = new List<float>();
    }





    //Get and Save sensor data to their coresponding lists
    public void GetSensorData()
    {

        sensorDevice.TryGetFeatureValue(CommonUsages.devicePosition, out Vector3 devicePosition);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceRotation, out Quaternion deviceRotation);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceVelocity, out Vector3 deviceVelocity);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAcceleration, out Vector3 deviceAcceleration);
        //angular
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAngularVelocity, out Vector3 angularVelocity);
        sensorDevice.TryGetFeatureValue(CommonUsages.deviceAngularAcceleration, out Vector3 angularAcceleration);


    
        SensorSaveDataToList(Position_list, devicePosition);
        SensorSaveDataToList(Velocity_list, deviceVelocity);
        SensorSaveDataToList(Acceleration_list, deviceAcceleration);
        SensorSaveDataToList(Angular_Acceleration_list, angularAcceleration);
        SensorSaveDataToList(Angular_Velocity_list, angularVelocity);
        SensorSaveDataToList(Rotation_list, deviceRotation);
        //Debug.LogWarning("Time: " + Time.realtimeSinceStartup);
        TimeStamp.Add(Time.realtimeSinceStartup);


    }

    //Adding data to a sensor parameter list
    public void SensorSaveDataToList(List<Vector3> dataList, Vector3 dataToSave)
    {

         dataList.Add(dataToSave);
    
    }
    // Overloaded function for quaternion input
    public void SensorSaveDataToList(List<Vector3> dataList, Quaternion dataToSave)
    {
         dataList.Add(dataToSave.eulerAngles);

    }






    //Saving a CSV file for each sensor for each sensor input
    public void SaveToFile(string path)
    {   
        //Adding the sensor input lists to a common list
        MeasurementsCollectiveList.Add(Position_list);
        MeasurementsCollectiveList.Add(Velocity_list);
        MeasurementsCollectiveList.Add(Acceleration_list);
        MeasurementsCollectiveList.Add(Angular_Acceleration_list);
        MeasurementsCollectiveList.Add(Angular_Velocity_list);
        MeasurementsCollectiveList.Add(Rotation_list);
        string[] listNames = { "Position_list", "Velocity_list", "Acceleration_list", "Angular_Acceleration_list", "Angular_Velocity_list", "Rotation_List"};

       
        // For each list in the collective list
        for (int i = 0; i < MeasurementsCollectiveList.Count; i++)
        {
            //string path1 = @"C:\Data\";
            
            string strFilePath = (path + @"/" + sensorName + listNames[i] + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
            string strSeperator = ",";

            StringBuilder sbOutput = new StringBuilder();

            sbOutput.AppendLine(string.Join(strSeperator, "X", "Y", "Z", "Time"));

            // For each element of a sublist save the data in CSV
            for (int j = 0; j < MeasurementsCollectiveList[i].Count; j++)
            {

               // Debug.LogError(MeasurementsCollectiveList[i].Count + " " + TimeStamp[j]);
                sbOutput.AppendLine(string.Join(strSeperator, MeasurementsCollectiveList[i][j].x, MeasurementsCollectiveList[i][j].y, MeasurementsCollectiveList[i][j].z, TimeStamp[j]));


            }

            File.WriteAllText(strFilePath, sbOutput.ToString());
            MeasurementsCollectiveList[i].Clear();

        }

            
           MeasurementsCollectiveList.Clear();
      }

    


}
