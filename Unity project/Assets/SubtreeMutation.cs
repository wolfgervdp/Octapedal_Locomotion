using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtreeMutation : MutationOperator {

    // evo params
    private double mutationRate = 0.4, depthOfMutation = 0.3, chanceOfTerminal = 0.5, depthIncrease = 0.5, maxDepthIncrease = 0.15;

    private static List<System.Type> functions = new List<Type>{
                typeof(ExtendLegNode),
				typeof(ModulusNode),
				typeof(ModulusNode),
                typeof(InversionNode)
	};

    private static List<System.Type> terminals = new List<Type>{
                typeof(TimeNode),
				typeof(TimeNode),
                typeof(ConstantFactorNode) 
	};

	public override List<BotBehavior> mutate (List<BotBehavior> individuals, float mutationProbability){

        foreach (BotBehavior bh in individuals) {
            bool proceedDepth = UnityEngine.Random.Range(0.0f, 1.0f) <= depthOfMutation;
			bool excuteMutation = UnityEngine.Random.Range(0.0f, 1.0f)<= mutationRate;

            BehaviourNode childNode = bh.getRoot();

            if (excuteMutation) {

                int indexOfRemoval = 0;
                BehaviourNode parent = null;
                int depthOfCurrentNode = 0;
				while (proceedDepth && childNode.getNextNodes() != null && childNode.getNextNodes().Count != 0)
                {
					//Debug.Log ("Childnode count " + childNode.getNextNodes ().Count);
                    indexOfRemoval = UnityEngine.Random.Range(0, childNode.getNextNodes().Count);
                    parent = childNode;
					//Debug.Log ("indexOfRemoval " + indexOfRemoval);
                    childNode = parent.getNextNodes()[indexOfRemoval];
                    depthOfCurrentNode++;
					proceedDepth = UnityEngine.Random.Range(0.0f, 1.0f) <= depthOfMutation;
                }
                if (parent != null) {
                    // remove node and grow new tree starting from this new node
                    int depth = calculateDepth();
                    if (depth > (bh.getDepth() - depthOfCurrentNode) * maxDepthIncrease) {
                        depth = Mathf.FloorToInt((float)((bh.getDepth() - depthOfCurrentNode) * maxDepthIncrease));
                    }
                    bool isTerminal = UnityEngine.Random.Range(0.0f, 1.0f) <= chanceOfTerminal;
                    parent.getNextNodes()[indexOfRemoval] = bh.innerGrowTree(depth, functions, terminals, isTerminal);
                }
            }

        }
		return individuals;
	}


    private int calculateDepth()
    {
        int depth = 0;
        bool proceed = UnityEngine.Random.Range(0.0f, 1.0f) <= depthIncrease;

        while (proceed) {
            depth++;
            proceed = UnityEngine.Random.Range(0.0f, 1.0f) <= depthIncrease;
        }
        return depth;
    }
}
