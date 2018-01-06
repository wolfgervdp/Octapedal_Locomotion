using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstantFactorNode : BehaviourNode, ITerminal {

	int constant = 0;

	public ConstantFactorNode(){
		constant = Random.Range (0, 10000);
	}

	public override int fire(BotController bc){
		
		return constant;
	}

	public override void mutate(){
		constant += Random.Range (-10, 10);
	}
}
