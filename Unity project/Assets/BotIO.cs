using System;
using System.Collections.Generic;
using System.Xml.Serialization;
using System.Xml;
using UnityEngine;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;

public class BotIO
{
	public static void saveBotToFile(BotBehavior b, String filename){
		using (Stream tw = File.Open (filename, FileMode.Create))
		{
			var formatter = new BinaryFormatter();
			formatter.Serialize(tw, b);
		}
	}

	public static BotBehavior loadBotFromFile(String filename){
		
		BotBehavior bb = new BotBehavior ();
		using (Stream tw = File.Open (filename, FileMode.Open))
		{
			var formatter = new BinaryFormatter();
			bb = (BotBehavior)formatter.Deserialize(tw);
		}
		return bb;
	}

	public static void savePopulationToFile(List<BotBehavior> pop, String filename){

		using (Stream tw = File.Open (filename, FileMode.Create))
		{
			var formatter = new BinaryFormatter();
			formatter.Serialize(tw, pop);
		}
	}
	public static List<BotBehavior> loadPopulationFromFile(String filename){
		List<BotBehavior>  pop = new List<BotBehavior> ();	//Return empty botbehavior if it didn't work
		using (Stream tw = File.Open (filename, FileMode.Open))
		{
			var formatter = new BinaryFormatter();
			pop = (List<BotBehavior>)formatter.Deserialize(tw);
		}
		return pop;
	}


}



