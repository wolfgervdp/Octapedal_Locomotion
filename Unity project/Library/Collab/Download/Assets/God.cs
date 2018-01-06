using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

//This class will create all life and judge them one by one.
public class God : MonoBehaviour {

	public Transform currentCamera;
	public GameObject individualPrefab;


    //evo param
	public float timeToEvaluate = 10; //In seconds
	public int populationSize = 20;
	public int concurrentTestSize = 10;
	private float currentSelectionProb = 0.6f;
    private float elitePercentage = 0.1f;

    public int randomSeed = 0;

    private static float timeOfCurrentSim = 0;
	private Queue<BotBehavior> behaviorsToEvaluate;
	private List<KeyValuePair<int,BotBehavior>> evaluatedBehavior;
	private int totalSumOfScores;
	private int numberOfGenerations = 0;

	Transform currentIndividual;
	List<Transform> currentIndividuals;

	BotBehavior currentBehavior;
	List<BotBehavior> currentBehaviors;

	// Use this for initialization
	void Start () {
		UnityEngine.Random.InitState (randomSeed);
		createInitialPopulation ();
		currentBehaviors = new List<BotBehavior> ();
		currentIndividuals = new List<Transform> ();
    }

    public static float getCurrentTime(){
		return Time.time - timeOfCurrentSim;
	}
	
	// Update is called once per frame
	void Update () {
		checkSetSpeed ();
		updateCamera ();

		//If time is over for the current individual
		if (Time.time - timeOfCurrentSim > timeToEvaluate) {
			Debug.Log ("Time's up!");

			for(int i = 0; i < currentIndividuals.Count; i++){
				int score = evaluate (currentIndividuals[i]);
				//Debug.Log ("Individual " + i + "had score of " + score);
				evaluatedBehavior.Add (new KeyValuePair<int,BotBehavior> (score, currentBehaviors[i]));
				totalSumOfScores += score;
			}

			Debug.Log ("Destroying individuals.");
			for (int i = 0 ; i < currentIndividuals.Count; i++) {
				UnityEngine.Object.DestroyImmediate (currentIndividuals[i].gameObject);
			}
			currentIndividuals.Clear ();
			currentBehaviors.Clear ();

            //Debug.Log ("Evaluating new individual");
            //If there are still individuals left, replace current individual to evaluate
            if (behaviorsToEvaluate.Count != 0) {
				spawnIndividuals ();
			} else {
				Debug.Log("Average Score: " + (float) totalSumOfScores/evaluatedBehavior.Count);
				totalSumOfScores = 0;
				//No more individuals left, so new generation cycle can start
				startNewGeneration ();
				spawnIndividuals ();
				numberOfGenerations++;
				Debug.Log ("---Currently at generation: " + numberOfGenerations + "---");
			}

            timeOfCurrentSim = Time.time;
		}

	}

	private void spawnIndividuals(){
		//Spawn new individual with new behavior 
		//Todo: look into reusing the same gameobject with new behavior + reset transform (possible performance upgrade)

		int numOfNewIndividuals = Mathf.Min (concurrentTestSize, behaviorsToEvaluate.Count);
		Debug.Log ("Still " +  behaviorsToEvaluate.Count + " individuals left to test.");
		Debug.Log ("Spawning " + numOfNewIndividuals + " new  individuals.");
		for(int i = 0; i < numOfNewIndividuals; i++){
			//Debug.Log ("Spawning individual " + i);
			BotBehavior b = behaviorsToEvaluate.Dequeue ();
			currentBehaviors.Add (b);
			GameObject go = Instantiate (individualPrefab);
			go.GetComponent<BotController> ().setBotBehavior (b);
			go.transform.position = new Vector3 (0,10*i,0); //Set location of individual
			go.SetActive (true);
			currentIndividuals.Add(go.transform);
		}
	}

	void createInitialPopulation ()
	{
		behaviorsToEvaluate = new Queue<BotBehavior> ();
		for (int i = 0; i < populationSize; i++) {
			BotBehavior bb = new BotBehavior ();

            bb.GrowTree(3, new List<Type>{
                typeof(ExtendLegNode),
                typeof(ModulusNode),
                typeof(InversionNode)
            }, new List<Type>{
                typeof(TimeNode),
                typeof(ConstantFactorNode)
            }, 2);


			behaviorsToEvaluate.Enqueue(bb);
		}
		evaluatedBehavior = new List<KeyValuePair<int, BotBehavior>> ();
	}

	int evaluate (Transform currentIndividual)
	{
		
		return (int) (currentIndividual.position.x*10);
	}

	void startNewGeneration ()
	{
		//Chosing the right operators (should probably move this higher up)
		SelectorOperator selector = new TournamentSelection();
		//MutationOperator mutator = new RandomMutationOperator ();
		MutationOperator mutator = new SubtreeMutation();
		CrossoverOperator crossoverOperator = new SubtreeCrossover ();
        MutationOperator nodeMutation = new NodeMutation();

		//Perform crossover and mutation operators
		List<BotBehavior> selectedBehavior = selector.select(evaluatedBehavior, currentSelectionProb);

		List<BotBehavior> newGen =  mutator.mutate(crossoverOperator.crossOver(selectedBehavior));

        List<BotBehavior> testing = new List<BotBehavior>();

        
          
        List<BotBehavior> mutatedNewGen = nodeMutation.mutate(newGen);
        List<BotBehavior> nextGen = elite(evaluatedBehavior, mutatedNewGen, elitePercentage);

        

		//Adjust selection probability
		if (nextGen.Count > populationSize) {
			currentSelectionProb -= 0.02f*((float)nextGen.Count/(float)populationSize);
		} else {
			if (nextGen.Count <= 0.3*populationSize) {
				//Panic! almost extinct!
				currentSelectionProb = 1;
			}

			currentSelectionProb += 0.02f*((float)populationSize/(float)nextGen.Count);
		}

		evaluatedBehavior.Clear ();
		Debug.Log ("New generation contains " + nextGen.Count + " behaviors.");
		//Set new generation
		behaviorsToEvaluate = new Queue<BotBehavior> (nextGen);

	}
    List<BotBehavior> elite(List<KeyValuePair< int,BotBehavior>> parent, List<BotBehavior> children, float perc) {

        int numberToKeep = (int)Mathf.Floor(perc * parent.Count);

        List<BotBehavior> nextGen = new List<BotBehavior>();
        parent.Sort((x, y) => y.Key.CompareTo(x.Key));


        for (int i = 0; i < numberToKeep; i++) {
            nextGen.Add(parent[numberToKeep].Value);
        }

        nextGen.AddRange(children);


        Debug.Log("parents: " + parent.Count + "  children: " + children.Count + "   nextgen: " + nextGen.Count);
        return nextGen;
    }
	List<BotBehavior> selectIndividuals (){
		

		return new List<BotBehavior> ();
		//Todo
	}

	void updateCamera ()
	{
		/*
		if(currentIndividual != null)
			currentCamera.transform.position = currentIndividual.transform.position + new Vector3(0,0,-10);*/
	}

	void checkSetSpeed ()
	{
		if (Input.GetKeyDown (KeyCode.Alpha1)) {
			Time.timeScale = 1;
		}
		if (Input.GetKeyDown (KeyCode.Alpha2)) {
			Time.timeScale = 2;
		}
		if (Input.GetKeyDown (KeyCode.Alpha3)) {
			Time.timeScale = 5;
		}
		if (Input.GetKeyDown (KeyCode.Alpha4)) {
			Time.timeScale = 10;
		}
	}

	void OnGUI(){

		GUI.skin.button.fontSize = 10;
		if(GUI.Button(new Rect(10, 10, 80, 30), "Save best bot")){

		}

		if(GUI.Button(new Rect(10, 40, 80, 30), "Save all bots")){

		}

		if(GUI.Button(new Rect(10, 70, 80, 30), "Load bots")){
			
		}
		if(GUI.Button(new Rect(10, 100, 80, 30), "Pause")){
			Time.timeScale = 0;
		}
	}
}
