using UnityEngine;
using System.Collections;

public class WindMillRotation : MonoBehaviour {

	// Use this for initialization
	private float rotationVelocity = 50;
	
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0, Time.deltaTime * rotationVelocity, 0);
	}
}
