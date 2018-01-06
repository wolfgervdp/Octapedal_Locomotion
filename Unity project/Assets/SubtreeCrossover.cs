using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubtreeCrossover : CrossoverOperator {

    //evo parameters

    double probabilityFunctionNode = 0.4;
//    double crossoverRate = 0.9;


	public override List<BotBehavior> crossOver(List<BotBehavior> individuals, int populationGoalSize){
		
		int numberOfCrossovers = populationGoalSize;//(int)Mathf.Floor(individuals.Count * (float)crossoverRate);
		List<BotBehavior> children =  new List<BotBehavior> ();

		for(int i= numberOfCrossovers/2; i > 0; i--)
        {
            int parentx = UnityEngine.Random.Range(0, individuals.Count);
            int parenty = UnityEngine.Random.Range(0, individuals.Count);

            BotBehavior p1 = BotBehavior.deepClone<BotBehavior>(individuals[parentx]);
            BotBehavior p2 = BotBehavior.deepClone<BotBehavior>(individuals[parenty]);

            Tuple<BehaviourNode,int> firstPoint = selectNodeOf(p1);
            Tuple<BehaviourNode,int> secPoint = selectNodeOf(p2);

            //doing the crossover

            //preventing swapping of root nodes --> pointless and undefined
            if (firstPoint.First != null && secPoint.First != null) {
               
                BehaviourNode temp = firstPoint.First.getNextNodes()[firstPoint.Second];
                firstPoint.First.getNextNodes()[firstPoint.Second] = secPoint.First.getNextNodes()[secPoint.Second];
                secPoint.First.getNextNodes()[secPoint.Second] = temp;
            }


            children.Add(p1);
            children.Add(p2);

        }

        return children;
	}


    private void iterate(BehaviourNode bn, int indexInParent, BehaviourNode parent, ref BehaviourNode res,ref int resIndex ,int count) {

        // add self as possible sample
        // check if their are childeren
        // first itterate over childeren


        count++;// new element discovered
        double factor = bn is IFunction == true ? probabilityFunctionNode : 1- probabilityFunctionNode;
        bool replace = UnityEngine.Random.Range(0, count) < factor;

        if (replace)
        {
            res = parent;
            resIndex = indexInParent;
        }
        if (bn.getNextNodes().Count > 0)
        {
            for (int i = 0; i < bn.getNextNodes().Count; i++) {

                iterate(bn.getNextNodes()[i],i,bn,ref res,ref resIndex, count);
            }
        }
       
   

    }

    private Tuple<BehaviourNode, int> selectNodeOf(BotBehavior bot)
    {
        BehaviourNode parentNode = null;
        int index = 0;
        iterate(bot.getRoot(), 0, null, ref parentNode, ref index, 0);

        return new Tuple<BehaviourNode, int>(parentNode, index);


    }
}
