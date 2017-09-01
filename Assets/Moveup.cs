using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moveup : MonoBehaviour {

    void OnSelect()
    {
        // If the sphere has no Rigidbody component, add one to enable physics.
        transform.position = new Vector3(transform.position.x, 2, transform.position.z);
       
    }
}
