using UnityEngine;
using System.Collections;

public class Raycaster : MonoBehaviour {
	
	private GameObject target;		//unit selected
	public int state = 0;			

	void Start () 
	{
		target = GameObject.Find("peasant1");
	}
	
	void Update () 
	{
		if (Input.GetMouseButtonDown(1))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit))
			{	
				Vector3 destination = hit.point;
				target.GetComponent<SmartWorker>().moveToTarget(destination);
				
			}
		}
		
   		if(Input.GetMouseButtonDown(0))
		{
			//if a unit menu is opened ignore all left clicks beyond button clicks
			if (GameObject.Find("UserInterface").GetComponent<InGameGUI>().guiOpened() && state == 0)
			{
				return;
			}
        	Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			// Casts the ray and get the first game object hit
			if (Physics.Raycast(ray, out hit))
			{
				//state 0 means that a unit is selected
				//state 1 means that we instructed the worker to build. This means that the next left click is reserved for selecting the place of the new building
				if (state == 0)
				{
					target = hit.transform.gameObject;
					GameObject.Find("UserInterface").GetComponent<InGameGUI>().selectObject(target);
				}
				else
				{
					Vector3 targetPoint = hit.point;
					GameObject.Find("UserInterface").GetComponent<InGameGUI>().getCurrentUnitSelected().GetComponent<SmartWorker>().buildStructure(GameObject.Find("UserInterface").GetComponent<InGameGUI>().getTargetTag(), targetPoint);
					state = 0;
				}
				
			}
		
		}
	}
	
	public void setState(int st)
	{
		state = st;
	}
}
