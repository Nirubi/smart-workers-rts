using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Buy Item from Structure Using money
class Buy : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject Money;
	
	public Buy(string name, GameObject i, GameObject s, GameObject m) : base(name)
	{
		Item = i;
		Structure = s;
		Money = m;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("sells", Structure, Item));
		preconditions.Add(new iThinkFact("accepts", Structure, Money));
		preconditions.Add(new iThinkFact("holding", Money));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", Item));
		effects.Add (new iThinkFact("holding", false, Money));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + Money.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

