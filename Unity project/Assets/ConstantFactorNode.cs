using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ConstantFactorNode : BehaviourNode, ITerminal {

	int constant = 0;
    float mutationRate = 10f;
    float proportionality = 0.07071f;

	public ConstantFactorNode(){
		constant = Random.Range (0, 10000);
	}

	public override int fire(BotController bc){
		
		return constant;
	}

	public override void mutate(){
        updateMutationRate();
        updateConstant();
	}
    public float getRandomNormal() {
        float u1 = 1.0f - Random.Range(0.0f, 1.0f); //uniform(0,1] random doubles
        float u2 = 1.0f - Random.Range(0.0f, 1.0f);
        return  Mathf.Sqrt(-2.0f * Mathf.Log(u1)) *
                     Mathf.Sin(2.0f * Mathf.PI * u2); //random normal(0,1)
    }
    public void updateMutationRate() {
        float oldmut = mutationRate;
        mutationRate = mutationRate*Mathf.Exp(proportionality * getRandomNormal());
    }
    public void updateConstant() {
        int oldconst = constant;
        float adjustment = mutationRate * getRandomNormal();
        constant = constant + Mathf.FloorToInt(adjustment);
        //Debug.Log("constant now is: " + constant +" : "+oldconst + " : "+adjustment);

    }
}
