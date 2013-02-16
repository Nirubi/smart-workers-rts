using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;
using iThinkLibrary.iThinkActionRepresentation;

public class SmartWorker : MonoBehaviour {
	
	public int state = 0;
	public int previousState = 0;
	private string goal = "";
	private float h = 0.0f;
	private float v = 0.0f;
	
	private string targetTag;
	private Vector3[] places = null;
	private int counter = 0;
	
	public int velocity = 3;
	public Vector3 targetPosition;
	public float rotationSpeed = 0.1f;
	
	private iThinkBrain brain = null;
	
	void Start () 
	{
		this.GetComponent<CustomController>().setControls(0.0f, 0.0f);
		targetPosition = Vector3.zero;
		state = 0;
		foreach (Transform x in GameObject.Find("places").transform)
		{
			Physics.IgnoreCollision(collider, x.collider);
		}
	}
	
	void Update () 
	{
		if (state == 0)
		{
			
		}
		else if (state == 1)
		{
			//planning
			places = null;
			counter = 0;
			DateTime n1 = DateTime.Now;
			//writeInitState(brain.startState);
			brain.resetPlanner();
			previousState = 0;
			brain.planner.forwardSearch(brain.startState, brain.goalState, brain.ActionManager);
			DateTime n2 = DateTime.Now;
			TimeSpan interval = n2 - n1;
			Debug.Log (interval.TotalMilliseconds + " ms");
			brain.planner.getPlan().debugPrintPlan();
			GameObject.Find("UserInterface").GetComponent<InGameGUI>().showPlan(brain.planner.getPlan(), goal);
			List<iThinkAction> planActions = brain.planner.getPlan().getPlanActions();
			places = new Vector3[planActions.Count];
			for (int i=0 ; i<planActions.Count ; i++)
			{
				places[i] = planActions[i].getPlace().transform.position;
			}
			
			if (planActions.Count > 0)
			{
				state = 2;
			}
			else
			{
				state = 0;
			}
			previousState = 1;
			
		}
		else if (state == 2)
		{
			//executing a plan
			if (places != null)
			{
				if (counter >= places.Length)
				{
					places = null;
					counter = 0;
					state = 0;
					
					Vector3 tempPosition = this.transform.position;
					tempPosition.z += 10;
					this.transform.position = tempPosition;
				}
				else
				{
					moveToTarget(places[counter]);
					counter++;
				}
			}
		}
		else if (state == 3)
		{
			//moving
			executeStep();
			if (h == 0.0f && v == 0.0f)
			{
				int tempState = previousState;
				previousState = state;
				state = tempState;
				//Debug.Log ("Movement completed");
				targetPosition = Vector3.zero;
			}
			
			
		}
		
		else if (state == 4)
		{
			//building a structure
			executeStep();
			if (h == 0.0f && v == 0.0f)
			{
				state = 0;
				previousState = 4;
				targetPosition = Vector3.zero;
				//Debug.Log ("Building completed");
				GameObject x = GameObject.FindGameObjectsWithTag(targetTag)[0];
				Vector3 tempV = this.transform.position;
				tempV.y = x.transform.position.y;
				GameObject obj = (GameObject) Instantiate(x, tempV, Quaternion.identity);
				obj.name = targetTag + GameObject.FindGameObjectsWithTag(targetTag).Length;
				obj.transform.parent = GameObject.Find ("places").transform;
				Vector3 pos = new Vector3(this.transform.position.x, this.transform.position.y, this.transform.position.z);
				pos.x -= 5.0f;
				this.transform.position = pos;
				GameObject.Find("State").GetComponent<Agent>().addToDomain(obj);
			}
			
		}
	
	}
	
	private void executeStep()
	{
			if (targetPosition == Vector3.zero)
			{
				return;
			}
			Vector3 currentPosition = this.transform.position;
			if (currentPosition.x < targetPosition.x - 1.0f)
			{
				h = -1.0f;
			}
			else if (currentPosition.x > targetPosition.x + 1.0f)
			{
				h = 1.0f;
			}
			else 
			{
				h = 0.0f;
			}
			if (currentPosition.z < targetPosition.z - 1.0f)
			{
				v = -1.0f;
			}
			else if (currentPosition.z > targetPosition.z + 1.0f)
			{
				v = 1.0f;
			}
			else
			{
				v = 0.0f;
			}
			this.GetComponent<CustomController>().setControls(v, h);
	}
	
	public void thinkAndExecute(iThinkBrain br, string g)
	{
		brain = br;
		state = 1;
		//writeInitState(brain.startState);
		goal = g;
	}
	
	public void moveToTarget(Vector3 target)
	{
		previousState = state;
		targetPosition = target;
		targetPosition.y = this.transform.position.y;
		state = 3;
		
	}
	
	public void buildStructure(string tag, Vector3 target)
	{
		Vector3 tempV = target;
		tempV.y = this.transform.position.y;
		targetPosition = tempV;
		targetTag = tag;
		state = 4;
		
	}
	
	public void writeInitState(iThinkState st)
	{
		 // create a writer and open the file
            TextWriter tw = new StreamWriter("initState.txt");
		
			foreach (iThinkFact f in st.getFactList())
			{
            // write a line of text to the file
            	tw.WriteLine(f.ToString());
			}

            // close the stream
            tw.Close();
	}
}
