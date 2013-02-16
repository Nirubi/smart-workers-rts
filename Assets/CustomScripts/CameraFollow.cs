using UnityEngine;
using System.Collections;

public class CameraFollow : MonoBehaviour {

	// Use this for initialization
	private Vector3 previousPosition;
	
	void Start () 
	{
		previousPosition = GameObject.Find("peasant1").transform.position;
	}
	
	// Update is called once per frame
	void Update () 
	{
		Vector3 currentPosition = GameObject.Find("peasant1").transform.position;
		Vector3 change = new Vector3(currentPosition.x - previousPosition.x, currentPosition.y - previousPosition.y, currentPosition.z - previousPosition.z);
		Vector3 cameraPosition = transform.position;
		cameraPosition.x += change.x;
		cameraPosition.y += change.y;
		cameraPosition.z += change.z;
		transform.position = cameraPosition;
		previousPosition = currentPosition;
		
	}
}
