using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System.Text;
using System.IO;
using System;
using UnityEngine.XR;

public class WriteToCsvScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SaveToFile(List<Vector3> dataList, string listName)
    {

        string strFilePath = (@"C:\Data\" + listName + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv");
        string strSeperator = ",";
        StringBuilder sbOutput = new StringBuilder();
        //UnicontaAPI.FetchInv(inventar, compID, userN, pw).GetAwaiter().GetResult();
        sbOutput.AppendLine(string.Join(strSeperator, "X", "Y", "Z"));
        foreach (Vector3 input in dataList)
        {
            sbOutput.AppendLine(string.Join(strSeperator, input.x, input.y, input.z));

        }

        File.WriteAllText(strFilePath, sbOutput.ToString());


    }
}
