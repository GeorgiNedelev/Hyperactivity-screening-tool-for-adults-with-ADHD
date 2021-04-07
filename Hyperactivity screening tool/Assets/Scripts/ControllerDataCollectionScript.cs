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

            if (dataList[dataList.Count - 1].ToString() != dataToSave.ToString())
            {

                dataList.Add(dataToSave);
                Debug.LogWarning("Added Value" + dataToSave);
                // Debug.LogWarning("last data point :" + RightControllerData_Acceleration[RightControllerData_Acceleration.Count - 1] + "added data point" + devicePosition );
            }
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

        string strFilePath = (@"C:\Data\" + sensorName + listNames[i]+ DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
        string strSeperator = ",";
        StringBuilder sbOutput = new StringBuilder();
        //UnicontaAPI.FetchInv(inventar, compID, userN, pw).GetAwaiter().GetResult();
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
