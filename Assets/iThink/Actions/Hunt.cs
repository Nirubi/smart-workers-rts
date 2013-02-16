using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Hunt Item in Structure for For using weapon Weapon
class Hunt : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject Weapon;
	GameObject For;
	
	public Hunt(string name, GameObject i, GameObject s, GameObject f, GameObject w) : base(name)
	{
		Item = i;
		Structure = s;
		For = f;
		Weapon = w;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		//preconditions.Add(new iThinkFact("enabled", Structure));
		preconditions.Add (new iThinkFact("isConvertedTo", Item, For));
		preconditions.Add (new iThinkFact("livesIn", Item, Structure));
		preconditions.Add(new iThinkFact("holding", Weapon));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", For));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + For.name + ", " + Weapon.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

