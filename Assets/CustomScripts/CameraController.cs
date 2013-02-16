using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	// Use this for initialization
	void Start () 
	{
		//nothing
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (Input.GetKey(KeyCode.W))
		{
			Vector3 temp = transform.position;
			transform.position = temp - new Vector3(0,0,1);
		}
		if (Input.GetKey(KeyCode.S))
		{
			Vector3 temp = transform.position;
			transform.position = temp + new Vector3(0,0,1);
		}
		if (Input.GetKey(KeyCode.D))
		{
			Vector3 temp = transform.position;
			transform.position = temp - new Vector3(1,0,0);
		}
		if (Input.GetKey(KeyCode.A))
		{
			Vector3 temp = transform.position;
			transform.position = temp + new Vector3(1,0,0);
		}
		if (Input.GetKey(KeyCode.Q))
		{
			Vector3 temp = transform.position;
			transform.position = temp + new Vector3(0,1,0);
		}
		if (Input.GetKey(KeyCode.E))
		{
			Vector3 temp = transform.position;
			transform.position = temp - new Vector3(0,1,0);
		}
		if (Input.GetKey(KeyCode.PageUp))
		{
			transform.Rotate(new Vector3(1,0,0));
		}
		if (Input.GetKey(KeyCode.PageDown))
		{
			transform.Rotate(new Vector3(-1,0,0));
		}
		if (Input.GetKey(KeyCode.Minus))
		{
			transform.camera.fieldOfView -= 1;
		}
		if (Input.GetKey(KeyCode.Equals))
		{
			transform.camera.fieldOfView += 1;
		}
		
	}
}
