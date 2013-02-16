using System;
using System.Collections;
using System.Collections.Generic;
using iThinkLibrary;
using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using UnityEngine;

//Harvest Item from Structure using pick
class Produce : iThinkAction
{
	GameObject Item;
	GameObject Structure;
	GameObject Ingredient;
	
	public Produce(string name, GameObject i, GameObject s, GameObject ingr) : base(name)
	{
		Item = i;
		Structure = s;
		Ingredient = ingr;
		preconditions = new List<iThinkFact>();
		effects = new List<iThinkFact>();
		initPreconditions();
		initEffects();
	}
	
	public override void initPreconditions()
	{
		//base.initPreconditions();
		preconditions.Add(new iThinkFact("processes", Structure, Ingredient));
		preconditions.Add(new iThinkFact("isConvertedTo", Ingredient, Item));
		preconditions.Add(new iThinkFact("holding", Ingredient));
	}
	
	public override void initEffects()
	{
		//base.initEffects();
		effects.Add(new iThinkFact("holding", Item));
		effects.Add(new iThinkFact("holding", false, Ingredient));
	}
	
	public override String toString()
	{
		return name + "(" + Item.name + ", " + Structure.name + ", " + Ingredient.name + ")";
	}
	
	public override GameObject getPlace()
	{
		return Structure;
	}
}

