using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Buy Item from Structure Using money
class CraftSpell : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject Resource;
	
	public CraftSpell(string name, GameObject i, GameObject s, GameObject r) : base(name)
	{
		Item = i;
		Structure = s;
		Resource = r;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("accepts", Structure, Resource));
		preconditions.Add(new iThinkFact("isConvertedTo", Resource, Item));
		preconditions.Add(new iThinkFact("holding", Resource));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", false, Resource));
		effects.Add (new iThinkFact("holding", Item));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + Resource.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}



