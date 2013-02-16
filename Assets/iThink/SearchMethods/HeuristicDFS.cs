using System;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

public class HeuristicDepthFS : iThinkPlanner 
{
	public iThinkActionManager actionManager;
	
	public HeuristicDepthFS() : base(){ depth = 6; nonValidHValue = -1;}
	
	public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
	{
		int it = 0;
		iThinkPlan curStep, nextStep;
		iThinkState CurrentState;
		nodesVisited++;
		actionManager = ActionManager;
	
		while ( OpenStates.Count != 0 )
		{
			List<iThinkAction> applicableActions = new List<iThinkAction>();
	
			curStep = new iThinkPlan( OpenStates[0] );
			CurrentState = OpenStates[0].getState();
			VisitedStates.Add( CurrentState );
			OpenStates.RemoveAt( 0 );
			
			applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
			
			if (curStep.getActionCount() < depth)
			{
				bool flag = true;
				List<iThinkPlan> successors = new List<iThinkPlan>();
				foreach ( iThinkAction action in applicableActions )
				{
					bool found = false;             
			
					nextStep = progress( curStep, action );
		
					nodesVisited++;
					if ( compareStates( nextStep.getState(), GoalState ) )
					{
						if (flag == true)
						{
							nodesExpanded++;
						}
						Debug.Log( "Found Plan (H-DepthFS) " + nextStep.getActionCount());
						//Debug.Log("Nodes visited : " + nodesVisited);
						//Debug.Log("Nodes expanded : " + nodesExpanded);
						Plan.setPlan( nextStep );
						return Plan;
					}
		
					if (VisitedStates.Contains(nextStep.getState()))
					{
						found = true;
					}
		
					if ( found == false )
					{
						int Cost = hFunction( nextStep.getState(), GoalState );
						if (Cost != nonValidHValue)
						{
		                    nextStep.getState().setCost( Cost );
							successors.Add(nextStep);
							if (flag == true)
							{
								flag = false;
								nodesExpanded++;
							}
						}
					}
				}
            	successors.Sort( delegate( iThinkPlan obj1, iThinkPlan obj2 )
                            {
                                if ( obj1.getState().getCost() == obj2.getState().getCost() )
                                    return 0;
                                else if ( obj1.getState().getCost() < obj2.getState().getCost() )
                                    return -1;
                                else
                                    return 1;
                            }
                           );
				OpenStates.InsertRange(0, successors);
				++it;
			}
		}
		Debug.Log( "Didn't find Plan (H-DepthFS)" );
		return null;
	}
	
//	public override int hFunction( iThinkState nextState, iThinkState GoalState )
//    {
//    	return base.hFunction(nextState,GoalState);
//
//    }
	
	public override int hFunction(iThinkState nextState, iThinkState GoalState)
	{
		iThinkState tempState = new iThinkState(GoalState);
		iThinkState curState = new iThinkState(nextState);
		for (int i=0 ; i<depth ; i++)
		{
			foreach (iThinkFact f in curState.getFactList())
			{
				tempState.delFact(f);
			}
			
			if (tempState.getFactList().Count == 0)
			{
				return i;
			}
			
			List<iThinkAction> applicableActions = getApplicable(curState, actionManager.getActions());
			iThinkPlan curStep = new iThinkPlan(curState);
			foreach (iThinkAction act in applicableActions)
			{
				iThinkPlan nextStep = progressPositive(curStep, act);
				curStep = nextStep;
			}                                                                                                                   
			curState = curStep.getState();
		}
		
		return nonValidHValue;
	}
}
