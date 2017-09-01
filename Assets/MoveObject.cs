using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using AWSSDK.Examples;

public class MoveObject : MonoBehaviour {
    bool selected = false;
    public GameObject alexa;
    RaycastHit hit;
    RaycastHit hitFloor;
    public LayerMask canShootAt;
    public LayerMask floor;
    //public GameObject mycamera;
    Transform objectHit;
    Vector3 shiftVect;
    public GameObject cam;

    void OnSelect()
    {
        selected = !selected;

        if (selected)
        {
            
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            if (Physics.Raycast(ray, out hit, 999, canShootAt))
            {
                objectHit = hit.transform;
                alexa.GetComponent<SQSExample>().setObject(objectHit.gameObject);
                //if (Physics.Raycast(ray, out hitFloor, 9999, floor))
                //{
                //    shiftVect = hitFloor.point-objectHit.position;
                //}
                Debug.Log("selected " + objectHit.name);

            }
        }
        else
        {
            objectHit = null;
            shiftVect = new Vector3(0, 0, 0);
        }
    }
    // Use this for initialization
    void Start () {


    }
	
	// Update is called once per frame
	void Update () {
        
        if (Input.GetKeyDown(KeyCode.S))
        {
            SendMessage("OnSelect");
        }

        if (selected) {
            
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);

       
            
               
              if (Physics.Raycast(ray, out hitFloor, 9999, floor))
                {
                objectHit.position = hitFloor.point;// +shiftVect;
                    Debug.Log("moved");
                }

            }
        }
    }


