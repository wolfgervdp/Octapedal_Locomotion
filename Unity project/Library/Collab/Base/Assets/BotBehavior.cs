using System.Collections.Generic;
using UnityEngine;

public class BotBehavior {
	

	private BehaviorNode root;
	private int depth; 

	public void behave(BotController bc){
		behave (root, 0, bc);
	}

	//Depth first "action propagation"
	public void behave(BehaviorNode bn, int previousInput, BotController bc){
		
		int newInput = bn.fire (bc, previousInput);
		foreach (BehaviorNode nextNode in bn.getNextNodes ()) {
			if(nextNode != null)
				behave (nextNode, newInput, bc);
		}
		
	}


	bool rollDice (float probability)
	{
		return Random.Range (0, 10000) < probability*10000;
	}

	public void mutate(float mutationProb){
		Queue<BehaviorNode> toLoop = new Queue<BehaviorNode> ();
		while(toLoop.Count != 0){
			BehaviorNode bn = toLoop.Dequeue ();
			if (rollDice (mutationProb)) {
				bn.mutate ();
			}
			foreach(BehaviorNode bhvn in bn.getNextNodes ())
				toLoop.Enqueue(bhvn);
		}
	}

	//Constructs a random tree as behavior
	public void constructRandom(int depth, List<System.Type> types, int branchingFactor) {
		this.depth = depth;
		root = constructRandomInner (depth, types, branchingFactor);
	}

	//Recursive method used by constructRandom()
	private BehaviorNode constructRandomInner(int depth, List<System.Type> types, int branchingFactor){
		
		if (depth <= 0)
			return null;
		
		List<BehaviorNode> l = new List<BehaviorNode>();
		int nextNode = Random.Range (0,types.Count-1);
		BehaviorNode newNode = (BehaviorNode) System.Activator.CreateInstance(types[nextNode]);
		for (int i = 0; i < branchingFactor; i++) {
			
			l.Add (constructRandomInner (depth - 1, types, branchingFactor));
		}
		newNode.setNextNodes (l);
		return newNode;
	}

	public BehaviorNode getNodeAtDepth(int depthOfRandomNode){

		BehaviorNode currentNode = root;
		for (int i = 0; i < depth; i++) {
			currentNode = currentNode.getNextNodes () [Random.Range(0,1)];
		}
		return currentNode;
	}

	public int getDepth(){
		return depth;
	}
	
}
