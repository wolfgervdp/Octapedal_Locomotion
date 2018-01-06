using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class SusSelector : SelectorOperator {

	public override List<BotBehavior> select (List<KeyValuePair<int,BotBehavior>> evaluatedBehavior, float toKeep){
		if (toKeep < 0 || toKeep > 1)
			throw new Exception ("Fraction of population to keep should be between 0 and 1");

		//first create list with cumulative fitness values
		List<KeyValuePair<int,BotBehavior>> cumulativeBehavior = new List<KeyValuePair<int, BotBehavior>>();
		int sum = 0;
		for (int i = 0; i < evaluatedBehavior.Count; i++) {
			sum += evaluatedBehavior [i].Key;
			cumulativeBehavior.Add (new KeyValuePair<int,BotBehavior>(sum,evaluatedBehavior [i].Value));
		}

		int indToKeep = (int) (evaluatedBehavior.Count * toKeep);	//Absolute number of individuals to keep
		int distance = (int) ((float)sum) / (indToKeep);			//Distance between pointers
		int start = UnityEngine.Random.Range (0, distance);	//Start of the pointers

		List<BotBehavior> behaviorsToKeep = new List<BotBehavior>();
		int j = 0; 
		for (int i = 0; i < evaluatedBehavior.Count; i++) {
			if (start + distance * j <= evaluatedBehavior [i].Key) {
				behaviorsToKeep.Add (evaluatedBehavior [i].Value);
				j++;	//Increase pointer
			}
		}
		return behaviorsToKeep;
	}
}
