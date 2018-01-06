using System.IO;
using System.Collections.Generic;
using UnityEngine;

public class Monitor {

	private List<List<int>> statistics = new List<List<int>>();
	private int currentLine;
	private int totalSumOfScores;
	private int maxScore;
	private int numberOfIndividuals;
	private float variance;

	public int MaxScore
	{
		get{
			return maxScore;
		}
	}

	public float Variance
	{
		get{
			float sum = 0;
			foreach (int i in statistics[currentLine]) {
				sum += ((float)i - AverageScore) * ((float)i - AverageScore);
			}

			return sum/(float)numberOfIndividuals;
		}
	}

	public float AverageScore
	{
		get{
			return ((float)totalSumOfScores/ (float)numberOfIndividuals);
		}
	}

	public Monitor(){
		resetMetrics ();
	}

	private void resetMetrics(){
		totalSumOfScores = 0;
		maxScore = 0;
		numberOfIndividuals = 0;
	}

	//Generates statistics such as max score, average score. Also adds entries to the table.
	public void generateStatistics(List<KeyValuePair<int,BotBehavior>> evaluatedBehavior){
		statistics.Add (new List<int> ());

		numberOfIndividuals += evaluatedBehavior.Count;

		foreach(KeyValuePair<int,BotBehavior> kvp in evaluatedBehavior){
			if (kvp.Key > maxScore) {
				maxScore = kvp.Key;
			}
			totalSumOfScores += kvp.Key;
			statistics [currentLine].Add (kvp.Key);
		}
	}
		
	public void increaseGeneration(){
		resetMetrics ();
		currentLine++;
	}

	//Prints statistics to logger
	public void printStatistics(){
		Debug.Log ("Maximum score: " + MaxScore);
		Debug.Log ("Average score: " + AverageScore);
	}


	//Write results to file (CSV, 1 line per generation)
	public void writeStatistics(string filename){
		StreamWriter writer = new StreamWriter (filename, true);
		foreach(List<int> line in statistics){
			foreach (int score in line) {
				writer.Write (score + ",");
			}
			writer.Write ("\n");
		}
		writer.WriteLine ("------");
		writer.Close ();
	}
}
