using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;

public class DepthFS : iThinkPlanner {

	public DepthFS() : base(){ depth = 6; }

	public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
	{
		int it = 0;
		iThinkPlan curStep, nextStep;
		iThinkState CurrentState;
		nodesExpanded = 0;
		nodesVisited++;
		while ( OpenStates.Count != 0 )
		{
			List<iThinkAction> applicableActions = new List<iThinkAction>();
			curStep = new iThinkPlan( OpenStates[0] );
			CurrentState = OpenStates[0].getState();
			VisitedStates.Add( CurrentState );
			OpenStates.RemoveAt( 0 );
			
			applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
			int count=0;
			if (curStep.getActionCount() < depth)
			{
				bool flag = true;
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
						Debug.Log( "Found Plan (DepthFS) " + nextStep.getActionCount());
						Debug.Log("Nodes Visited : " + nodesVisited);
						Debug.Log("Nodes expanded : " + nodesExpanded);
						Plan.setPlan( nextStep );
						return Plan;
					}
		
					if (VisitedStates.Contains(nextStep.getState()))
					{
						found = true;
					}
					
					if ( found == false )
					{
						OpenStates.Insert( count, nextStep );
						count++;
						if (flag == true)
						{
							nodesExpanded++;
							flag = false;
						}
					}
				}
		
				++it;
			}
		}
		//Debug.Log( "Didn't find Plan (DepthFS) " + it  );
		return null;
	}
}
