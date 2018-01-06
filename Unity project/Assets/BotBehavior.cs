using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

[System.Serializable]
public class BotBehavior {


    private BehaviourNode root;
    private int depth;


    // evo params
    private double depthIncreaseProb = 50;

    public void behave(BotController bc) {
        //behave(root, 0, bc);
		if(root != null)
        	iterate(root, null, bc);
    }

    public void iterate(BotController bc) {
        iterate(root, null, bc);
    }
		
    //iterate over tree
    public void iterate(BehaviourNode bn, BehaviourNode parent, BotController bc) {

        if (bn.getNextNodes() != null) {
            //Debug.Log("iterating over children");

            foreach (BehaviourNode child in bn.getNextNodes()) {
                if (child != null)
                    iterate(child, bn ,bc);
            }

        }
        if (parent != null && parent is IFunction){
            ((FunctionNode)parent).fillParams(bn.fire(bc)); // should not be like this! 
            //--> if everything is write rewrite fire => fire(bc)
        }

    }

    public BehaviourNode getRoot() {
        return root;
    }

	bool rollDice (float probability)
	{
		return Random.Range (0, 10000) < probability*10000;
	}

	public void mutate(float mutationProb){
		Queue<BehaviourNode> toLoop = new Queue<BehaviourNode> ();
		while(toLoop.Count != 0){
            BehaviourNode bn = toLoop.Dequeue ();
			if (rollDice (mutationProb)) {
				bn.mutate ();
			}
			foreach(BehaviourNode bhvn in bn.getNextNodes ())
				toLoop.Enqueue(bhvn);
		}
	}

	//Constructs a random tree as behavior
	public void constructRandom(int depth, List<System.Type> types, int branchingFactor) {
		this.depth = depth;
		root = constructRandomInner (depth, types, branchingFactor);
	}

	//Recursive method used by constructRandom()
    //full method
	private BehaviourNode constructRandomInner(int depth, List<System.Type> types, int branchingFactor){
		
		if (depth <= 0)
			return null;
		
		List<BehaviourNode> l = new List<BehaviourNode>();
		int nextNode = Random.Range (0,types.Count);
		BehaviourNode newNode = (BehaviourNode) System.Activator.CreateInstance(types[nextNode]);
        //Debug.Log("created node: " + types[nextNode]);
		for (int i = 0; i < branchingFactor; i++) {
			
			l.Add (constructRandomInner (depth - 1, types, branchingFactor));
		}
		newNode.setNextNodes (l);
		return newNode;
	}

    //ramped half-and-half: grow method

    public BehaviourNode innerGrowTree(int maxDepth, List<System.Type> Functions, List<System.Type> terminals, bool isTerminal) {

        if (maxDepth < 0)
            return null;

        if (maxDepth == 0) {
            isTerminal = true;
        }          
        int nextNode = UnityEngine.Random.Range(0, (isTerminal == true? terminals.Count : Functions.Count));
        List<BehaviourNode> l = new List<BehaviourNode>();
        BehaviourNode rootNode = (BehaviourNode)System.Activator.CreateInstance(isTerminal == true? terminals[nextNode] : Functions[nextNode]);
        if (!isTerminal) {
            
            int numberOfBranches = ((IFunction)rootNode).parameters;
            for (int i = 0; i < Mathf.Max(numberOfBranches,2); i++)
            {
                bool isTerm = UnityEngine.Random.Range(0, 100) > depthIncreaseProb;
                l.Add(innerGrowTree(maxDepth - 1, Functions, terminals,isTerm));
            }

        }
		rootNode.setNextNodes(l);
        return rootNode;
    }

    public void GrowTree(int maxDepth, List<System.Type> Functions, List<System.Type> terminals, int branchFactor)
    {
        root = innerGrowTree(maxDepth, Functions,terminals, false);
        this.depth = calculateHeight(root);
    }

    private int calculateHeight(BehaviourNode root)
    {
        int max = 0;

        if (root != null && root.getNextNodes() != null) {
            foreach (BehaviourNode childNode in root.getNextNodes())
            {
                if (childNode != null)
                {
                    int height = calculateHeight(childNode);
                    if (height > max)
                        max = height;
                }
            }
        }
        return max+1;
    }

	public int calculateHeight()
	{
		return calculateHeight (root);
	}

    public static void PrintTree(BehaviourNode tree, string indent, bool last)
    {
        Debug.Log(indent + "+- " + tree.GetType());
        
        indent += last ? "   " : "|  ";
        if (tree.getNextNodes() != null) {
            for (int i = 0; i < tree.getNextNodes().Count; i++)
            {
                if(tree.getNextNodes()[i] != null)
                    PrintTree(tree.getNextNodes()[i], indent, i == tree.getNextNodes().Count - 1);
            }
        }
        
    }
    public BehaviourNode getNodeAtDepth(int depthOfRandomNode){

		BehaviourNode currentNode = root;
		for (int i = 0; i < depth; i++) {
			currentNode = currentNode.getNextNodes () [Random.Range(0,1)];
		}
		return currentNode;
	}

	public int getDepth(){
		return depth;
	}

    public static T deepClone<T>(T obj)
    {
        using (var ms = new MemoryStream())
        {
            var formatter = new BinaryFormatter();
            formatter.Serialize(ms, obj);
            ms.Position = 0;

            return (T)formatter.Deserialize(ms);
        }
    }

}
