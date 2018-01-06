using UnityEngine;
[System.Serializable]

public class ModulusNode : FunctionNode
{
	public ModulusNode():base(2){}


    public override int fire(BotController bc){
        //Debug.Log ("Modulus: " + input % modulus);

        int x = this.getInput();
        int y = this.getInput();
        if (y > 0) {
            return x % y;
        }

        //modulus 0 is undefined
        return 0;
	}

}
