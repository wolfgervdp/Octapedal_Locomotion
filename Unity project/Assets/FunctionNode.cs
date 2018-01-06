using System;
using UnityEngine;
[System.Serializable]

public abstract class FunctionNode: BehaviourNode, IFunction
{

    private int param;

    int filledParams = 0;
    int[] internalParams;
    int currentIndex = 0;
    bool filled = false;
    public FunctionNode(int param)
    {
        this.param = param;
        this.internalParams = new int[param];
    }
    public override void mutate() {
        //all functions use the same mutation method--> replace node with different function node, no "internal" mutation

    }
    protected int getInput() {

        //if (filled()) {
            int value = internalParams[currentIndex];
            currentIndex = (currentIndex + 1) % param;
            return value;
       // }
       // return 0;
        
    }
    public void fillParams(int input)
    {
        this.internalParams[filledParams % param] = input;
      //  Debug.Log("filling: " + filledParams % param);
       // Debug.Log("filling with: " + input);

        filledParams++;
        if (filledParams == param)
        {
            filled = true;
            filledParams = 0;
        }
        else {
            filled = false;
        }
      // Debug.Log("is full? " + isFilled());
    }
    protected bool isFilled() {
        return filled;
    }
    public int parameters
    {
        get
        {
            return param;
        }
    }

}
