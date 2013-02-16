using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Harvest Item from Structure using pick
class Harvest : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject Pick;
	
	public Harvest(string name, GameObject i, GameObject s, GameObject t) : base(name)
	{
		Item = i;
		Structure = s;
		Pick = t;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("provides", Structure, Item));
		preconditions.Add(new iThinkFact("holding", Pick));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", Item));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + Pick.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

