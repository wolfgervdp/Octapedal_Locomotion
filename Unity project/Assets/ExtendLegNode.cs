using System.Collections.Generic;
using UnityEngine;
[System.Serializable]

public class ExtendLegNode : FunctionNode
{

	int legId;
    private static int param = 1;
    public ExtendLegNode(): base(param){
		legId = Random.Range (0, 7);
	}

    public override int fire(BotController bc){
		//Debug.Log ("fire: " + input + " ");
		bc.scaleLeg (legId, ((float) getInput())/100);
		return getInput();
	}

	public override void mutate(){
		legId = Random.Range (0, 7); //Just an example of a mutation. A more local mutation might be more appropriate
	}


}
