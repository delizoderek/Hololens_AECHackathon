using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraRay : MonoBehaviour
{
    public LayerMask canShootAt;
    public LayerMask floor;

    // Use this for initialization
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(canShootAt.value.ToString());
        //Debug.Log(LayerMask.GetMask("active", "ignoreRaycast"));
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
                Debug.Log(objectHit.name);
                // Do something with the object that was hit by the raycast.

                if(Physics.Raycast(ray, out hitFloor, 9999, floor))
                {
                    objectHit.position = hitFloor.point;
                    Debug.Log(hitFloor.point);
                }

            }
        }
    }
}