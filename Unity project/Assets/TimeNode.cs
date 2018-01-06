using UnityEngine;
[System.Serializable]

public class TimeNode : BehaviourNode, ITerminal {

	public override int fire(BotController bot){
		//Debug.Log ("Time: " + (int) (God.getCurrentTime() * 100));
		return (int) (God.getCurrentTime()*100);	//multiplication allows for better precision
	}

	public override void mutate(){
		//No internal mutation operator on time node
	}
}
