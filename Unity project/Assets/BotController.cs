using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BotController : MonoBehaviour {

	Vector3 startScale;
	private BotBehavior behavior;


	// Use this for initialization
	void Start () {
	}

	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate(){
		behavior.behave(this);
	}

	public void setBotBehavior(BotBehavior behavior){
		this.behavior = behavior;
	}

	public void scaleLeg(int legId, float scale){
		transform.Find ("Leg"+legId).GetComponent<LegController>().scaleLeg(scale);
	}
		
}
