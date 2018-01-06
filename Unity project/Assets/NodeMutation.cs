using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NodeMutation : MutationOperator {
    /*
     * Simple mutation operator that iterates over all nodes and mutates these nodes.
     * 
     */

    // evo params
    private double mutationRate = 0.15;



	public override List<BotBehavior> mutate (List<BotBehavior> individuals, float mutationProbability){
		//mutationRate = mutationProbability;

        foreach (BotBehavior bh in individuals)
        {
            iterate(bh.getRoot());

        }
        return individuals;
	}


     private void iterate(BehaviourNode bn) {

        if ( bn.getNextNodes() != null &&bn.getNextNodes().Count > 0)
        {
            for (int i = 0; i < bn.getNextNodes().Count; i++) {
                if(bn.getNextNodes()[i] != null)
                    iterate(bn.getNextNodes()[i]);
            }
        }

        if (UnityEngine.Random.Range(0.0f, 1.0f) < mutationRate)
        {
			//Debug.Log ("mutate");
            bn.mutate();
        }

       
   

    }
    
}
