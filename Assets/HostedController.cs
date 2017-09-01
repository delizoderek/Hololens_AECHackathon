using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;

public class HostedController : MonoBehaviour {

    public string filePath;
    private List<ItemHelper> revList = new List<ItemHelper>();
    private List<ItemHelper> wallList = new List<ItemHelper>();
    private float scaleFactor = .3048f;
    public Process FluxAgent = new Process();
    // Use this for initialization
    void Start()
    {
        //FluxAgent.StartInfo.WorkingDirectory = "/FluxAgent/";
        FluxAgent.StartInfo.FileName = "FluxAgent/FluxAgent.exe";
        FluxAgent.Start();
        FluxAgent.BeginOutputReadLine();
        Process[] running = Process.GetProcesses();
        foreach (Process process in running)
        {
            UnityEngine.Debug.Log(process.ProcessName);
        }

        StreamReader sr = new StreamReader(filePath + "/hostedTest.csv");
        
        GameObject[] designObj = GameObject.FindGameObjectsWithTag("active");
        for (int i = 0; i < designObj.Length; i++)
        {
            revList.Add(new ItemHelper(designObj[i].name, designObj[i]));
        }

        //GameObject[] wallObjs = GameObject.FindGameObjectsWithTag("wall");
        //for (int i = 0; i < designObj.Length; i++)
        //{
        //    wallList.Add(new ItemHelper(wallObjs[i].name, designObj[i]));
        //}

        string line;
        while ((line = sr.ReadLine()) != null)
        {
            string[] values = line.Split(',');
            int index = itemIndex(values[0]);
            if (index > -1)
            {
                revList[index].setPosition(new Vector3(float.Parse(values[2]) * scaleFactor, float.Parse(values[4]) * scaleFactor, float.Parse(values[3]) * scaleFactor));
                revList[index].setRotation(float.Parse(values[5]));
                UnityEngine.Debug.Log(values[6]);
                UnityEngine.Debug.Log(GameObject.Find(values[6]).ToString());
                UnityEngine.Debug.Log(GameObject.Find(values[6]).transform);
                revList[index].transform.parent = GameObject.Find(values[6]).transform;
            }

        }

    }

    public int itemIndex(string name)
    {
        int retVal = -1;
        for (int i = 0; i < revList.Count; i++)
        {
            if (revList[i].getId().Equals(name))
            {
                retVal = i;
                break;
            }
        }
        return retVal;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
