using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AdaptationNode : BehaviourNode, ITerminal {

	int constant = 0;

	public AdaptationNode(){
		constant = Random.Range (0, 10000);
	}

    public override int fire(BotController bc)
    {
        throw new System.NotImplementedException();
    }

    public override void mutate(){
		constant += Random.Range (-1, 1);
	}
}
