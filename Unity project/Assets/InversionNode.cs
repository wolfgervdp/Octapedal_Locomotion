using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]

// not really useful --> same as division with x=1 and y= input
public class InversionNode : FunctionNode {
	
    public InversionNode() : base(1) {}

    public override int fire(BotController bot){
		int input = getInput ();
        if (input != 0) {

			return 1 / input;
        }
        else {
            return input;
        }
	}

	public override void mutate(){
		//No internal mutation operator on inversion node
	}
}
