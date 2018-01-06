using System;
using System.Collections.Generic;

[Serializable]
public abstract class BehaviourNode {

	List<BehaviourNode> nextNodes;

	public abstract void mutate();
	public abstract int fire(BotController bc);

    public List<BehaviourNode> getNextNodes(){
		return nextNodes;
	}
	public void setNextNodes(List<BehaviourNode> nextNodes){
		this.nextNodes = nextNodes;
	}

}