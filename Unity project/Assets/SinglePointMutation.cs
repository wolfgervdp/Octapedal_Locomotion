using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SinglePointMutation : MutationOperator {


	private static List<System.Type> functions = new List<System.Type>{
		typeof(ExtendLegNode),
		typeof(ModulusNode),
		typeof(InversionNode)
	};

    // evo params
    
    double terminalMutationRate = 0.05;
	double terminalFunctionRate = 0.2; //Proportion of function/terminal probability
	double functionMutationRate;

	public SinglePointMutation(){
		functionMutationRate = terminalMutationRate*terminalFunctionRate;
	}

	public override List<BotBehavior> mutate (List<BotBehavior> individuals, float mutationProbability){
//		double functionMutationRate = mutationProbability;
//		double terminalMutationRate = mutationProbability;

        foreach (BotBehavior bh in individuals)
        {
            mutate(bh.getRoot(),null,0);
        }
        return individuals;
	}

    public void mutate(BehaviourNode current,BehaviourNode parent, int i)
    {
        if (current is IFunction && Random.Range(0,1)< functionMutationRate && parent != null)
        {
            int randomFunction = Random.Range(0, functions.Count - 1);
            BehaviourNode function = (BehaviourNode)System.Activator.CreateInstance(functions[randomFunction]);
            function.setNextNodes(current.getNextNodes());
            parent.getNextNodes()[i] = function;
        }
        else if(Random.Range(0,1)<terminalMutationRate)
        {
            current.mutate();
        }
        int index = 0;
        foreach (BehaviourNode
            child in current.getNextNodes()) {
            index++;
            mutate(child, current, index);
        }
    }
}

