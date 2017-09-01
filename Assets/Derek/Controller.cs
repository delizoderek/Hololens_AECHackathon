using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Diagnostics;
using System.Linq; 
using UnityEngine;

public class Controller : MonoBehaviour {

	public string filePath;
	public LayerMask canShootAt;
	public LayerMask floor;

	private List<ItemHelper> revList = new List<ItemHelper>();
	private List<string> objUpdates = new List<string> ();
	private float scaleFactor = .3048f;
	private StreamReader sr;
	private StreamWriter sw;
	private float syncTime = 5f;
	private Process FluxAgent = new Process();
	private GameObject alexaListener;
	// Use this for initialization
	void Start () {
        FluxAgent.StartInfo.FileName = filePath + "/FluxAgent/FluxAgent.exe";
        FluxAgent.StartInfo.UseShellExecute = false;
        FluxAgent.StartInfo.RedirectStandardInput = true;
        FluxAgent.Start();

        sr = new StreamReader (filePath + "/revitout.csv");
		GameObject[] designObj = GameObject.FindGameObjectsWithTag ("active");
		for (int i = 0; i < designObj.Length; i++) {
			revList.Add (new ItemHelper (designObj [i].name, designObj [i]));
		}
		
		string line;
		while ((line = sr.ReadLine ()) != null) {
			string[] values = line.Split (',');
			int index = itemIndex (values [0]);
			if (index > -1) {
				revList [index].setPosition(new Vector3(float.Parse(values[2]) * scaleFactor ,float.Parse(values[4]) * scaleFactor,float.Parse(values[3]) * scaleFactor));
				revList [index].setRotation (float.Parse (values [5]));
			}

		}

      
        InvokeRepeating ("decreaseSyncTime",1f,1f);

	}

	public int itemIndex(string name){
		int retVal = -1;
		for (int i = 0; i < revList.Count; i++) {
			if (revList [i].getId ().Equals (name)) {
				retVal = i;
				break;
			}
		}
		return retVal;
	}

	public void register(GameObject alexa){
		alexaListener = alexa;
	}

	// Update is called once per frame
	void Update () {
		
		Transform cam = gameObject.transform;
		RaycastHit hit;
		RaycastHit hitFloor;
		//Ray ray = camera.ScreenPointToRay(Input.mousePosition);
		Ray ray = new Ray(cam.position, cam.forward);

		if (Input.GetKey(KeyCode.R))
		{
			if (Physics.Raycast(ray, out hit, 9999, canShootAt))
			{
				Transform objectHit = hit.transform;
				UnityEngine.Debug.Log(objectHit.name);
				// Do something with the object that was hit by the raycast.

				if(Physics.Raycast(ray, out hitFloor, 9999, floor))
				{
					objectHit.position = hitFloor.point;
					UnityEngine.Debug.Log(hitFloor.point);
				}

			}
		}


		if (syncTime <= 0) {
			
			for (int i = 0; i < revList.Count; i++) {
				Vector3 start = revList [i].getPastPosition ();
				Vector3 end = revList [i].getCurrentPosition();
				if(!(start.x == end.x && start.y == end.y && start.z == end.z)){
					revList [i].setPastPosition (end);
					writeToFile (revList[i]);
				}
			}
			if (objUpdates.Count > 0) {
				sw = new StreamWriter (filePath + "/UnityText.csv");
				for (int i = 0; i < objUpdates.Count; i++) {
					sw.WriteLine (objUpdates [i]);
				}
				sw.Close ();
			}
			objUpdates = new List<string> ();
			syncTime = 5f;
		}
	}

	public void select(){
		
	}

	public void writeToFile(ItemHelper upInfo){
        Vector3 pos = upInfo.getCurrentPosition ();
        float rot = upInfo.getCurrentRotate ();
        FluxAgent.StandardInput.WriteLine("push 3 " + pos + rot);
        objUpdates.Add(upInfo.getId() + ","
			+ upInfo.getId() + ","
			+ pos.x + ","
			+ pos.y + ","
			+ pos.z + ","
			+ rot);
	}

	void decreaseSyncTime(){
		syncTime--;
	}
}
