using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Get Item from Structure
class Get : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	
	public Get(string name, GameObject i, GameObject s) : base(name)
	{
		Item = i;
		Structure = s;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("provides", Structure, Item));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", Item));
		effects.Add(new iThinkFact("provides", false, Structure, Item));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

