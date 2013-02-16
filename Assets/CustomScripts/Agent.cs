using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;
using iThinkLibrary.iThinkActionRepresentation;

using System;
using System.IO;

public class Agent : MonoBehaviour 
{
	List<string> tags;
	public iThinkBrain brain = null;
	private string [] actionSchemas = 
	{
		"get-2-Tag::weapon-Tag::armory",
		"hunt-4-Tag::animal-Tag::forest-Tag::resource-Tag::weapon",
		"make-3-Tag::object-Tag::slaughterhouse-Tag::resource",
		"get-2-Tag::object-Tag::warehouse",
		"get-2-Tag::object-Tag::townhall",
		"get-2-Tag::resource-Tag::windmill",
		"buy-3-Tag::object-Tag::market-Tag::object",
		"make-3-Tag::object-Tag::laboratory-Tag::resource",
		"get-2-Tag::fishingPole-Tag::armory",
		"get-2-Tag::weapon-Tag::magicTower",
		"hunt-4-Tag::animal-Tag::lake-Tag::resource-Tag::fishingPole",
		"get-2-Tag::tool-Tag::repository",
		"harvest-3-Tag::resource-Tag::mine-Tag::tool",
		"store-2-Tag::tool-Tag::repository",
		"store-2-Tag::object-Tag::warehouse",
		"get-2-Tag::armory-Tag::repository",
		"produce-3-Tag::object-Tag::factory-Tag::resource",
		"craftSimpleWeapon-4-Tag::weapon-Tag::factory-Tag::object",
		"store-2-Tag::fishingPole-Tag::armory",
		"store-2-Tag::weapon-Tag::armory",
		"harvest-3-Tag::resource-Tag::forest-Tag::tool",
		"produce-3-Tag::object-Tag::lumberjack-Tag::resource",
		"craftSpell-3-Tag::weapon-Tag::magicTower-Tag::resource",	
	};
	
	// Use this for initialization
	void Start () 
	{
		brainInit();
	}
	
	// Update is called once per frame
	void Update () 
	{	
		if (Input.GetKeyUp(KeyCode.L))
		{
			foreach (GameObject a in brain.sensorySystem.getKnownObjects())
			{
				Debug.Log(a.name);
			}
		}
	}
	
	//instruct agent to execute specific goal
	public void givePlanCommand(GameObject agent, string goal)
	{
		brain.goalState = new iThinkState("Goal", new List<iThinkFact>());
		if (goal == "bring-food")
		{
			brain.goalState.addFact(new iThinkFact("holding", GameObject.Find("supply-ration")));
		}
		else if (goal == "bring-money")
		{
			brain.goalState.addFact(new iThinkFact("holding", GameObject.Find("coins")));
		}
		else if (goal == "bring-minerals")
		{
			brain.goalState.addFact(new iThinkFact("holding", GameObject.Find("ironBar")));
		}
		else if (goal == "arm-yourself")
		{
			brain.goalState.addFact(new iThinkFact("holding", GameObject.Find("bow")));
		}
		agent.GetComponent<SmartWorker>().thinkAndExecute(brain, goal);
	}
	
	public iThinkBrain getBrain()
	{
		return brain;
	}
	
	//initialize brain, state and goal
	public void brainInit()
	{
		brain = new iThinkBrain();
		//find all game objects by tags
		tags = new List<string>();
		tags.Add("forest");
		tags.Add("mine");
		tags.Add("factory");
		//tags.Add("warehouse");
		//tags.Add("windmill");
		tags.Add("lumberjack");
		tags.Add("magicTower");
		tags.Add("object");
		tags.Add("animal");
		tags.Add("resource");
		tags.Add("weapon");
		tags.Add("tool");
		tags.Add("laboratory");
		tags.Add("armory");
		tags.Add("market");
		tags.Add("lake");
		tags.Add("fishingPole");
		//tags.Add("townhall");
		tags.Add("temple");
		tags.Add("barracks");
		tags.Add("district");
		tags.Add("repository");
		//tags.Add("slaughterhouse");
		
		brain.sensorySystem.OmniscientUpdate( this.gameObject, tags );
		brain.startState = new iThinkState("Initial", new List<iThinkFact>());
		brain.goalState = new iThinkState("Goal", new List<iThinkFact>());
		initKnowledge();
		initGoal();
		brain.curState = new iThinkState(brain.startState);
		brain.ActionManager = new iThinkActionManager();
        brain.ActionManager.initActionList( this.gameObject, actionSchemas, brain.getKnownObjects(), brain.getKnownFacts() );
	}
	
	//update action manager, when new objects enter the domain or other ones are removed from it
	public void updateActionManagers()
	{
		brain.ActionManager = new iThinkActionManager();
        brain.ActionManager.initActionList( this.gameObject, actionSchemas, brain.getKnownObjects(), brain.getKnownFacts() );
		//printActions();
	}
	
	//enabling a building or building a new one
	public void addToDomain(GameObject building)
	{
		string tag = building.tag;
		if (tag == "armory")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find("bow")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("spear")));
		}
		else if (tag == "laboratory")
		{
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("rice")));
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("gold")));
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("rawMeat")));
		}
		else if (tag == "repository")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find("pick")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find("fishingPole")));
		}
		else if (tag == "slaughterhouse")
		{
			//brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("rawMeat")));
		}
		else if (tag == "warehouse")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("supply-ration")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("lumber")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("ironBar")));
		}
		else if (tag == "market")
		{
			brain.startState.addFact(new iThinkFact("accepts", building, GameObject.Find("coins")));
			brain.startState.addFact(new iThinkFact("sells", building, GameObject.Find ("potion")));
			brain.startState.addFact(new iThinkFact("sells", building, GameObject.Find ("herbs")));
			brain.startState.addFact(new iThinkFact("sells", building, GameObject.Find ("supply-ration")));
			brain.startState.addFact(new iThinkFact("sells", building, GameObject.Find ("magicEssence")));
		}
		else if (tag == "windmill")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("rice")));
		}
		else if (tag == "magicTower")
		{
			brain.startState.addFact(new iThinkFact("accepts", building, GameObject.Find("magicEssence")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("fireSpell")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("meteorSpell")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("ultimaSpell")));
		}
		else if (tag == "mine")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("gold")));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("iron")));
		}
		else if (tag == "forest")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("herbs")));
			brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("deer"), building));
			brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("boar"), building));
			brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("unicorn"), building));
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find ("wood")));
		}
		else if (tag == "temple")
		{
			//nothing yet
		}
		else if (tag == "townhall")
		{
			brain.startState.addFact(new iThinkFact("provides", building, GameObject.Find("coins")));
		}
		else if (tag == "lumberjack")
		{
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("wood")));
		}
		else if (tag == "factory")
		{
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("iron")));
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("ironBar")));
			brain.startState.addFact(new iThinkFact("processes", building, GameObject.Find("lumber")));
		}
		else
		{
			return;
		}
		
		brain.sensorySystem.addObject(building);
		updateActionManagers();
	}
	
	//initialize Init State of the problem
	private void initKnowledge()
	{
		
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("gold"), GameObject.Find ("coins")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("iron"), GameObject.Find ("ironBar")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("rice"), GameObject.Find ("supply-ration")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("rawMeat"), GameObject.Find ("supply-ration")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("wood"), GameObject.Find ("lumber")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("herbs"), GameObject.Find ("potion")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("boar"), GameObject.Find ("rawMeat")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("deer"), GameObject.Find ("rawMeat")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("fish"), GameObject.Find ("rawMeat")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("unicorn"), GameObject.Find ("magicEssence")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("magicEssence"), GameObject.Find ("fireSpell")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("magicEssence"), GameObject.Find ("meteorSpell")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("magicEssence"), GameObject.Find ("ultimaSpell")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("ironBar"), GameObject.Find ("bow")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("ironBar"), GameObject.Find ("spear")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("ironBar"), GameObject.Find ("pick")));
		brain.startState.addFact(new iThinkFact("isConvertedTo", GameObject.Find ("lumber"), GameObject.Find ("fishingPole")));
		
		GameObject[] a = GameObject.FindGameObjectsWithTag("warehouse");		//all warehouses
		if (tags.Contains("warehouse"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("supply-ration")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("lumber")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("ironBar")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("magicTower");						//all magic towers
		if (tags.Contains("magicTower"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("Spell", GameObject.Find("fireSpell")));
				brain.startState.addFact(new iThinkFact("Spell", GameObject.Find("meteorSpell")));
				brain.startState.addFact(new iThinkFact("Spell", GameObject.Find("ultimaSpell")));
				brain.startState.addFact(new iThinkFact("accepts", k, GameObject.Find("magicEssence")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("fireSpell")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("meteorSpell")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("ultimaSpell")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("armory");
		if (tags.Contains("armory"))//all armories
		{
			foreach(GameObject k in a)
			{
				
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("bow")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("spear")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find("fishingPole")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("windmill");						//all windmills
		if (tags.Contains("windmill"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("rice")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("market");
		if (tags.Contains("market")) //all markets
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("accepts", k, GameObject.Find("coins")));
				brain.startState.addFact(new iThinkFact("sells", k, GameObject.Find ("potion")));
				brain.startState.addFact(new iThinkFact("sells", k, GameObject.Find ("herbs")));
				brain.startState.addFact(new iThinkFact("sells", k, GameObject.Find ("supply-ration")));
				brain.startState.addFact(new iThinkFact("sells", k, GameObject.Find ("magicEssence")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("mine");							//all mines
		if (tags.Contains("mine"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("gold")));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("iron")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("forest");							//all forests
		if (tags.Contains("forest"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("herbs")));
				brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("deer"), k));
				brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("boar"), k));
				brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("unicorn"), k));
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find ("wood")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("lake");							//all forests
		if (tags.Contains("lake"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("livesIn", GameObject.Find ("fish"), k));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("laboratory");
		if (tags.Contains("laboratory"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("rice")));
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("herbs")));
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("gold")));
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("rawMeat")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("slaughterhouse");
		if (tags.Contains("slaughterhouse"))
		{
			foreach (GameObject k in a)
			{
				
				//brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("rawMeat")));
				
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("factory");
		if (tags.Contains("factory"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("iron")));
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("ironBar")));
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("lumber")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("lumberjack");
		if (tags.Contains("lumberjack"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("processes", k, GameObject.Find("wood")));
			}
		}
		
		
		a = GameObject.FindGameObjectsWithTag("townhall");
		if (tags.Contains("townhall"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find("coins")));
			}
		}
		
		a = GameObject.FindGameObjectsWithTag("repository");
		if (tags.Contains("repository"))
		{
			foreach (GameObject k in a)
			{
				brain.startState.addFact(new iThinkFact("provides", k, GameObject.Find("pick")));
			}
		}
		brain.startState.addFact(new iThinkFact("constructMaterial", GameObject.Find("ironBar")));
		brain.startState.addFact(new iThinkFact("constructMaterial", GameObject.Find("lumber")));
		//printState();
		//brain.startState = new iThinkState(brain.startState);
		
	}
	
	//default goal
	private void initGoal()
	{
		brain.goalState.addFact(new iThinkFact("holding", GameObject.Find ("supply-ration")));
	}
	
	public void disableBuilding(GameObject building)
	{
		
		List<iThinkFact> factsToDelete = new List<iThinkFact>();
		foreach (iThinkFact fact in brain.startState.getFactList())
		{
			if (fact.getObjects().Contains(building))
			{
				factsToDelete.Add(fact);
			}
		}
		foreach (iThinkFact fact in factsToDelete)
		{
			brain.startState.delFact(fact);
		}
		brain.sensorySystem.ignoreObject(building);
		updateActionManagers();
		
	}
	
	public void printActions()
	{
		TextWriter tw = new StreamWriter("actions.txt");
		foreach (iThinkAction act in brain.ActionManager.getActions())
		{
			tw.WriteLine(act.toString());
		}
		tw.Close();
	}
}
