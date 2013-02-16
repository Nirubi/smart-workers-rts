using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Hunt Item in Structure for For using weapon Weapon
class CraftSimpleWeapon : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject M1;
	
	public CraftSimpleWeapon(string name, GameObject i, GameObject s, GameObject m1) : base(name)
	{
		Item = i;
		Structure = s;
		M1 = m1;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("isConvertedTo", M1, Item));
		preconditions.Add(new iThinkFact("processes", Structure, M1));
		preconditions.Add(new iThinkFact("holding", M1));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", Item));
		effects.Add(new iThinkFact("holding", false, M1));
	}
		
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + M1.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

