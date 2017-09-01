using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swap_window_options : MonoBehaviour {
    public GameObject NoWindow;
    public GameObject Window;

	// Use this for initialization
	void Start () {
        Window.SetActive(false);
        NoWindow.SetActive(true);
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown(KeyCode.W))
        {
            if (Window.activeSelf == true)
            {
                Window.SetActive(false);
                NoWindow.SetActive(true);
            }
            else
            {
                Window.SetActive(true);
                NoWindow.SetActive(false);
            }
        }
	}
}
