﻿


using System.Collections.Generic;
using UnityEngine;

public class TournamentSelection : SelectorOperator
{
    // evo parameters

    int tournementSize = 4;
    bool uniqueParents = false;


    public void disableMultipleInstances() {
        uniqueParents = true;
    }

	public override List<BotBehavior> select (List<KeyValuePair<int,BotBehavior>> evaluatedBehavior, float toKeep){
        int parents = (int)(evaluatedBehavior.Count * toKeep);
        int currentNumParents = 0;
		List<BotBehavior> winners = new List<BotBehavior>();
        List<KeyValuePair<int, BotBehavior>> parentsCopy = new List<KeyValuePair<int, BotBehavior>>(evaluatedBehavior);

        if (evaluatedBehavior.Count < tournementSize)
        {
            foreach (KeyValuePair<int, BotBehavior> pair in parentsCopy)
            {
                winners.Add(pair.Value);
            }
        }
        else {
            while (currentNumParents < parents) {
                int starting = UnityEngine.Random.Range(0, parentsCopy.Count - tournementSize);
                //Debug.Log("starting pos: " + starting);
                BotBehavior max = parentsCopy[starting].Value;
                int maxValue = parentsCopy[starting].Key;
//                Debug.Log("init maxv: " + maxValue);
                int index = 0;
                for(int i = starting+1; i< starting + tournementSize; i++)
                {
                   // Debug.Log("value: " + evaluatedBehavior[i].Key);

                    if (maxValue < parentsCopy[i].Key) {
                        maxValue = parentsCopy[i].Key;
                        max = parentsCopy[i].Value;
                        index = i;
                    }
                }
               // Debug.Log("maxv: " + maxValue);


                winners.Add(max);
                if (uniqueParents) {
                    parentsCopy.Remove(parentsCopy[index]);
                }
                currentNumParents++;
            }

        }

		return winners;
	}
}