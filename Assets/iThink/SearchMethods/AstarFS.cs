using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using iThinkLibrary.KnowledgeRepresentation;
using iThinkLibrary.iThinkActionRepresentation;
using iThinkLibrary.iThinkActionManagerUtility;
using iThinkLibrary.iThinkPlannerUitility;


class AstarFS : iThinkPlanner {
	
	public iThinkActionManager actionManager;
	
    public AstarFS():base()
	{ 
		depth = 6;
	}

    public override iThinkPlan SearchMethod( iThinkState GoalState, iThinkActionManager ActionManager, List<iThinkPlan> OpenStates, List<iThinkState> VisitedStates )
    {
        int it = 0;
		actionManager = ActionManager;
        iThinkPlan curStep, nextStep;
        iThinkState CurrentState = null;
		
		List<iThinkPlan> stateList = null;
		nodesVisited++;
		        
		while ( OpenStates.Count != 0 )
        {
			List<iThinkAction> applicableActions = new List<iThinkAction>();
		            
            stateList = new List<iThinkPlan>();
		            
            curStep = new iThinkPlan( OpenStates[0] );
            CurrentState = OpenStates[0].getState();
            VisitedStates.Add(CurrentState); 
			
		
            OpenStates.RemoveAt( 0 );
	        if (curStep.getActionCount() < depth)
			{
				
				bool flag = true;
	            applicableActions = getApplicable( CurrentState, ActionManager.getActions() );
			               
	            foreach ( iThinkAction action in applicableActions )
	            {
	            	bool found = false;
	                // todo: Add "statnode" for statistics retrieval
	                nextStep = progress( curStep, action );
	                nodesVisited++;
					if ( compareStates( nextStep.getState(), GoalState ) )
				    {
						if (flag == true)
						{
							nodesExpanded++;
						}
				        Debug.Log( "Found Plan (AstarFS) "+nextStep.getPlanActions().Count + ", Nodes Expanded :" + nodesExpanded);
						//Debug.Log( "Nodes visited : " + nodesVisited);
				        Plan.setPlan( nextStep );
				        repoFunct.completed=true;
									
				        return Plan;
				    }
					
					if (VisitedStates.Contains(nextStep.getState()))
					{
						found = true;
					}
	
	                if ( found == false )
	                {
	                    int Cost = hFunction( nextStep.getState(), GoalState );
	                    Cost = Cost + nextStep.getPlanActions().Count;
	                    nextStep.getState().setCost( Cost );
	                    stateList.Add( nextStep );
						if (flag == true)
						{
							nodesExpanded++;
							flag = false;
						}
	                }
	
	            }
				
				OpenStates.AddRange( stateList );
            	OpenStates.Sort( delegate( iThinkPlan obj1, iThinkPlan obj2 )
                            {
                                if ( obj1.getState().getCost() == obj2.getState().getCost() )
                                    return 0;
                                else if ( obj1.getState().getCost() < obj2.getState().getCost() )
                                    return -1;
                                else
                                    return 1;
                            }
                           );
            	++it;
			}
		
        }
		//Debug.Log( "Didn't find Plan (AstarFS)" );
        return null;
    }
    
    public override int hFunction( iThinkState nextState, iThinkState GoalState )
    {
    	return base.hFunction(nextState,GoalState);

    }
	
	
//	public override int hFunction(iThinkState nextState, iThinkState GoalState)
//	{
//		iThinkState tempState = new iThinkState(GoalState);
//		iThinkState curState = new iThinkState(nextState);
//		for (int i=0 ; i<depth ; i++)
//		{
//			//remove goal literals from tempState that exist in curState
//			//if tempState is empty return i;
//			//else progress curState
//			foreach (iThinkFact f in curState.getFactList())
//			{
//				tempState.delFact(f);
//			}
//			
//			if (tempState.getFactList().Count == 0)
//			{
//				return i;
//			}
//			
//			List<iThinkAction> applicableActions = getApplicable(curState, actionManager.getActions());
//			iThinkPlan curStep = new iThinkPlan(curState);
//			foreach (iThinkAction act in applicableActions)
//			{
//				iThinkPlan nextStep = progressPositive(curStep, act);
//				curStep = nextStep;
//			}                                                                                                                   
//			curState = curStep.getState();
//		}
//		
//		return depth;
//	}
	
	
	
}
