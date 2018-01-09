using System.Collections;
using System.Collections.Generic;
using UnityEngine;


using System;

//This class will create all life and judge them one by one.
public class God : MonoBehaviour {

	public Transform mainCamera;
	public Transform followCamera;

	public GameObject individualPrefab;

	//Main evolutionary parameters (name is self explanatory):
	public float timeToEvaluate = 10; //In seconds
	public int populationSize = 20;
	public int concurrentTestSize = 10;
    public float elitePercentage = 0.1f;
	public float selectionProb = 0.6f;

    public int randomSeed = 0;

    private static float timeOfCurrentSim = 0;
	private Queue<BotBehavior> behaviorsToEvaluate;					//Behaviors which still need to be evaluated (primarily used when concurrentTestSize < populationSize)
	private List<KeyValuePair<int,BotBehavior>> evaluatedBehavior;	//Behavios which were evaluated with their corresponding score

	private int numberOfGenerations = 0;							//Total number of generations which have passed

	private Tuple<int,BotBehavior> lastBestBot;						//Keep track of the best bot which has been existence (used for saving purposes)

	List<Transform> currentIndividuals;								//Keep track of the transform of the current individuals being tested
	List<BotBehavior> currentBehaviors;								//Keep track of current behaviors which are being tested
	private String currentPopText;									//Used for GUI (changing population size)
	private String savePopFileName = "pop";							//Used for GUI (filename for saving population)
	private String loadPopFileName = "pop";							//Used for GUI (filename for loading population)
	private String saveBotFileName = "bot";							//Used for GUI (filename for saving best bot)
	private String statisticsFileName = "stat";
	private Transform bestCurrentBot;								//Keep track of best current bot (for camera tracking)
	private Rect windowRect = new Rect (0, 0, 140, 240);			//Used for GUI (draggable window size)

	private float explorationParameter = 0.5f;
	Monitor monitor = new Monitor();

	/*
	 * Used for initialization 
	 */
	void Start () {
		UnityEngine.Random.InitState (randomSeed);		
		createInitialPopulation ();	
		currentBehaviors = new List<BotBehavior> ();	
		currentIndividuals = new List<Transform> ();
		currentPopText = "" + populationSize;
    }

    public static float getCurrentTime(){
		return Time.time - timeOfCurrentSim;
	}
	
	// Update is called once per frame
	void Update () {
		checkSetSpeed ();	//Check whether simulation speed should be updated
		updateCamera ();

		//If time is over for the current individuals
		if (isTimeUp()) {
			//Debug.Log ("Time's up!");

			//Evaluate all current individuals
			for(int i = 0; i < currentIndividuals.Count; i++){
				int score = evaluate (currentIndividuals[i], currentBehaviors[i]);

				if (score > lastBestBot.First) {
					lastBestBot = new Tuple<int,BotBehavior>(score, currentBehaviors[i]);
				}
				evaluatedBehavior.Add (new KeyValuePair<int,BotBehavior> (score, currentBehaviors[i]));
			}
			monitor.generateStatistics (evaluatedBehavior);

			//Destroy generation
			//Debug.Log ("Destroying individuals.");
			for (int i = 0 ; i < currentIndividuals.Count; i++) {
				UnityEngine.Object.DestroyImmediate (currentIndividuals[i].gameObject);
			}

			currentIndividuals.Clear ();
			currentBehaviors.Clear ();

            //If there are still individuals left, spawn these
            if (behaviorsToEvaluate.Count != 0) {
				spawnIndividuals ();
			} else {	//If none are left, create new generation
				
				//No more individuals left, so new generation cycle can start
				startNewGeneration ();
				spawnIndividuals ();
				monitor.printStatistics ();
				//Debug.Log ("Current variance: " + monitor.Variance);
				monitor.increaseGeneration ();
				numberOfGenerations++;
				Debug.Log ("---Currently at generation: " + numberOfGenerations + "---");
			}
            timeOfCurrentSim = Time.time;
		}
	}

	private bool isTimeUp(){
		return (Time.time - timeOfCurrentSim > timeToEvaluate);
	}

	private void spawnIndividuals(){
		//Spawn new individuals with new behavior 
		int numOfNewIndividuals = Mathf.Min (concurrentTestSize, behaviorsToEvaluate.Count);
		//Debug.Log ("Still " +  behaviorsToEvaluate.Count + " individuals left to test.");
		//Debug.Log ("Spawning " + numOfNewIndividuals + " new  individuals.");
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
		bestCurrentBot = currentIndividuals [0];	//Initialize on random individual
	}

	void createInitialPopulation ()
	{
		
		behaviorsToEvaluate = new Queue<BotBehavior> ();
		for (int i = 0; i < populationSize; i++) {
			BotBehavior bb = new BotBehavior ();

            bb.GrowTree(4, new List<Type>{
                typeof(ExtendLegNode),	
                typeof(ModulusNode),
                typeof(InversionNode)
            }, new List<Type>{
                typeof(TimeNode),	
				typeof(TimeNode),	//Second TimeNode to increase likelihood it is chosen
                typeof(ConstantFactorNode)	
            }, 2);


			behaviorsToEvaluate.Enqueue(bb);
		}
		lastBestBot = new Tuple<int,BotBehavior>(0, behaviorsToEvaluate.Peek());

		evaluatedBehavior = new List<KeyValuePair<int, BotBehavior>> ();
	}

	int evaluate (Transform currentIndividual, BotBehavior currentBehavior)
	{
		return (int)( (currentIndividual.position.x*10) - currentBehavior.calculateHeight()*0.2f);
	}

	//This method is not optimally using the strategy pattern yet
	void startNewGeneration ()
	{
		//Chosing the right operators (should probably move this higher up)
		SelectorOperator selector = new TournamentSelection();
		//SelectorOperator selector = new SusSelector();
		//MutationOperator mutator = new RandomMutationOperator ();
		MutationOperator subtreeMutator = new SubtreeMutation();
		CrossoverOperator crossoverOperator = new SubtreeCrossover ();
        MutationOperator nodeMutation = new NodeMutation();

		//Perform crossover and mutation operators
		List<BotBehavior> selectedBehavior = selector.select(evaluatedBehavior, selectionProb);
		List<BotBehavior> crossOveredBehavior = crossoverOperator.crossOver (selectedBehavior, (int)(populationSize * (1f - elitePercentage)));
		List<BotBehavior> newGen = subtreeMutator.mutate(crossOveredBehavior, explorationParameter);
		List<BotBehavior> mutatedNewGen = nodeMutation.mutate(newGen, explorationParameter);
		List<BotBehavior> nextGen = elite(evaluatedBehavior, mutatedNewGen, elitePercentage);


		evaluatedBehavior.Clear ();
		//Debug.Log ("New generation contains " + nextGen.Count + " behaviors.");
		//Set new generation
		behaviorsToEvaluate = new Queue<BotBehavior> (nextGen);

	}

    List<BotBehavior> elite(List<KeyValuePair< int,BotBehavior>> parent, List<BotBehavior> children, float perc) {

        int numberToKeep = (int) Mathf.Floor(perc * parent.Count);

        List<BotBehavior> nextGen = new List<BotBehavior>();
        parent.Sort((x, y) => y.Key.CompareTo(x.Key));

        for (int i = 0; i < numberToKeep; i++) {
            nextGen.Add(parent[i].Value);
        }

        nextGen.AddRange(children);
        return nextGen;
    }

	void updateCamera ()
	{
		if (bestCurrentBot != null) {
			followCamera.position = bestCurrentBot.position + new Vector3 (0, 0, -10);
		}
		if (Input.GetKeyDown (KeyCode.DownArrow)) {
			mainCamera.position += new Vector3(0,-300,0);
		}
		if (Input.GetKeyDown (KeyCode.UpArrow)) {
			mainCamera.position += new Vector3(0,300,0);
		}
	}

	/*
	 * Changes speed based on numeric keys 1=>4 (not the ones at the numpad)
	 */
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

	//Is called by UnityEngine to draw GUI elements
	void OnGUI(){
		//Draw draggable window for controls
		windowRect = GUI.Window (0, windowRect, GuiWindow, "Controls");
	}

	public void GuiWindow(int id){

		GUI.skin.button.fontSize = 10;
		if(GUI.Button(new Rect(40, 20, 80, 30), "Save best bot")){
			BotIO.saveBotToFile (lastBestBot.Second, saveBotFileName);
		}
		saveBotFileName = GUI.TextField (new Rect (10, 20, 30, 30), saveBotFileName);

		if(GUI.Button(new Rect(40, 50, 80, 30), "Save all bots")){
			BotIO.savePopulationToFile (currentBehaviors, savePopFileName);
		}
		savePopFileName = GUI.TextField (new Rect (10, 50, 30, 30), savePopFileName);

		if(GUI.Button(new Rect(40, 80, 80, 30), "Load bots")){
			foreach (BotBehavior bb in BotIO.loadPopulationFromFile (loadPopFileName)) {
				behaviorsToEvaluate.Enqueue (bb);
			}
		}
		loadPopFileName = GUI.TextField (new Rect (10, 80, 30, 30), loadPopFileName);


		if(GUI.Button(new Rect(10, 110, 110, 30), "Pause")){
			Time.timeScale = 0;
		}

		if(GUI.Button(new Rect(40, 140, 80, 30), "Set pop")){
			Int32.TryParse (currentPopText, out populationSize);
		}
		currentPopText = GUI.TextField (new Rect (10, 140, 30, 30), currentPopText);

		if(GUI.Button(new Rect(10, 170, 110, 30), "Find best bot")){
			findBestBot ();
		}

		if(GUI.Button(new Rect(40, 200, 80, 30), "Write stats")){
			monitor.writeStatistics (statisticsFileName);
		}
		statisticsFileName = GUI.TextField (new Rect (10, 200, 30, 30), statisticsFileName);

		GUI.DragWindow (new Rect (0, 0, 150, 150));	

	}

	//Helper method to find best current bot (in order to change camera to that bot)
	private void findBestBot(){
		foreach(Transform tr in currentIndividuals){
			if (tr.position.x > bestCurrentBot.position.x) {
				bestCurrentBot = tr;
			}
		}
	}
}
