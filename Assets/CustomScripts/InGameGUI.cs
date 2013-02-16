using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*using iThinkLibrary.iThinkActionRepresentation;*/
using iThinkLibrary.iThinkPlannerUitility;
using iThinkLibrary.iThinkActionRepresentation;

public class InGameGUI : MonoBehaviour 
{
	public Texture buildButton;
	public Texture foodButton;
	public Texture weaponButton;
	public Texture coinButton;
	public Texture materialButton;
	
	private bool unitSelected = false;
	private GameObject currentUnitSelected;
	
	private Vector3 targetPoint;
	private string targetTag;
	
	private iThinkPlan plan;
	private bool hasPlan = false;
	
	private string textFieldValue = "";
	private string goal = "";
	
	void Start()
	{
		currentUnitSelected = GameObject.Find("peasant1");
		unitSelected = true;
	}
	
	void OnGUI()
	{
		if (hasPlan)
		{
			int i=0;
			GUI.Box(new Rect(650, 10, 300, 250), goal);
			List<iThinkAction> actions = plan.getPlanActions();
			if (actions.Count == 0)
			{
				GUI.Label(new Rect(670, 40 + i * 30, 300, 40), "No Plan Found");
			}
			for (i=0 ; i<actions.Count ; i++)
			{
				GUI.Label(new Rect(670, 40 + i * 30, 300, 40), actions[i].toString());
			}
			
			if (GUI.Button(new Rect(670, 40 + (i + 1) * 30, 70, 30), "Hide"))
			{
				hasPlan = false;
			}
		}
		if (unitSelected)
		{
			if (currentUnitSelected.Equals(GameObject.Find ("Terrain")))
			{
				unitSelected = false;
				return;
			}
			string label = "";
			if (currentUnitSelected.name == "peasant1")
			{
				label = "choose a goal";
			}
			else
			{
				label = currentUnitSelected.name;
			}
			GUI.Box (new Rect(10, 10, 280, 200), label);
			foreach (Transform a in GameObject.Find ("individuals").transform)
			{
				GameObject obj = a.gameObject;
				if (obj.Equals(currentUnitSelected))
				{
					textFieldValue = GUI.TextField(new Rect(30, 130, 150, 25), textFieldValue);
					if (GUI.Button (new Rect(200, 120, 50, 50), buildButton))
					{
						targetTag = textFieldValue;
						GameObject.Find("raycaster").GetComponent<Raycaster>().setState(1);
					}
					
					GUI.Label(new Rect(30, 90, 200, 50), "Type the tag of the building you wish to build");
					if (GUI.Button (new Rect(30, 30, 50, 50), foodButton))
					{
						GameObject.Find("State").GetComponent<Agent>().givePlanCommand(currentUnitSelected, "bring-food");
					}
					if (GUI.Button (new Rect(90, 30, 50, 50), weaponButton))
					{
						GameObject.Find("State").GetComponent<Agent>().givePlanCommand(currentUnitSelected, "arm-yourself");
					}
					if (GUI.Button (new Rect(150, 30, 50, 50), coinButton))
					{
						GameObject.Find("State").GetComponent<Agent>().givePlanCommand(currentUnitSelected, "bring-money");
					}
					if (GUI.Button (new Rect(210, 30, 50, 50), materialButton))
					{
						GameObject.Find("State").GetComponent<Agent>().givePlanCommand(currentUnitSelected, "bring-minerals");
					}
					if (GUI.Button(new Rect(120, 170, 70, 30), "Cancel"))
					{
						unitSelected = false;
					}
				}
			}
			foreach (Transform a in GameObject.Find("places").transform)
			{
				GameObject obj = a.gameObject;
				if (obj.Equals(currentUnitSelected))
				{
					if (GUI.Button(new Rect(30, 60, 70, 30), "Enable"))
					{
						GameObject.Find("State").GetComponent<Agent>().addToDomain(currentUnitSelected);
						unitSelected = false;
					}
					if (GUI.Button(new Rect(120, 60, 70, 30), "Disable"))
					{
						GameObject.Find("State").GetComponent<Agent>().disableBuilding(currentUnitSelected);
						unitSelected = false;
					}
					if (GUI.Button(new Rect(120, 170, 70, 30), "Cancel"))
					{
						unitSelected = false;
					}
					
					return;
				}
			}
		}
	}
	
	public void selectObject(GameObject selectedObject)
	{
		currentUnitSelected = selectedObject;
		unitSelected = true;
	}
	
	public bool guiOpened()
	{
		return unitSelected;
	}
	
	public void showPlan(iThinkPlan p, string g)
	{
		plan = p;
		hasPlan = true;
		goal = g;
	}
	
	public string getTargetTag()
	{
		return targetTag;
	}
	
	public GameObject getCurrentUnitSelected()
	{
		return currentUnitSelected;
	}
	
	
}
