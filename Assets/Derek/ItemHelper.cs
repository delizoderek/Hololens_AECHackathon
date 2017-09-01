using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent (typeof (Controller))]
public class ItemHelper : MonoBehaviour {

	private string id;
	private GameObject item;
	private Vector3 pastPosition;
	private bool moved = false;

	public ItemHelper(string i,GameObject n){
		id = i;
		item = n;
		pastPosition = item.transform.position;
	}

	public void setPosition(Vector3 position){
		item.transform.position = position;
		pastPosition = position;
	}

	public void setPastPosition(Vector3 past){
		pastPosition = past;
	}

	public void setRotation(float y){
		item.transform.Rotate (0, y, 0);
	}

	public Vector3 getPastPosition(){
		return pastPosition;
	}

	public Vector3 getCurrentPosition(){
		return item.transform.position;
	}

	public float getCurrentRotate(){
		return item.transform.rotation.y;
	}

	public GameObject geItem(){
		return item;
	}

	public string getId(){
		return id;
	}
}
