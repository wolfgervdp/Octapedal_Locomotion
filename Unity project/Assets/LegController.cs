using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LegController : MonoBehaviour {

	Vector3 startScale;
	Vector3 goalScale;

	public float maxScale = 1.5f;
	public float minScale = 0.85f;
	public float maxScaleDelta = 1.015f;//Make extension a bit more "continuous"
	public float minScaleDelta = 0.985f;

	public float speed = 1.0f;
	private float startTime;
	private float journeyLength;

	private float minLength;
	private float maxLength;

	//private float previousTime;

	// Use this for initialization
	void Start () {
		startScale = transform.localScale;
		goalScale = startScale;
		minLength = minScale * transform.localScale.x;
		maxLength = maxScale * transform.localScale.x;
	}
	
	// Update is called once per frame
	void Update () {
		/* //This is to linearly interpolate the length of the leg
		 * if (transform.localScale == goalScale && journeyLength != 0) {
			float t = (Time.time - startTime) * speed / journeyLength;
			Debug.Log (t);
			transform.localScale = Vector3.Lerp (transform.localScale, goalScale, t);
		}*/
	}
	public void scaleLeg(float scale){
		//todo: add lerp

		//Provide "physical" limit to extension of leg 
		float boundedScale = Mathf.Min(Mathf.Max(minScaleDelta, scale), maxScaleDelta);
		//Debug.Log ("boundedScale: " + boundedScale);
		goalScale = transform.localScale;
		goalScale.x = Mathf.Min(Mathf.Max(minLength, boundedScale*goalScale.x), maxLength);

		//Debug.Log ("TimeDelta: " + Time.deltaTime);
		//startTime = Time.time;
		//journeyLength = Vector3.Distance (transform.localScale, goalScale);

		//Vector3.Lerp(scaleFrom, scaleTo, Time.);
		transform.localScale = goalScale;


	}
}
