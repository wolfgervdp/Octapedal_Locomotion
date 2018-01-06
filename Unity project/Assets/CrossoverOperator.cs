using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class CrossoverOperator {

	public abstract List<BotBehavior> crossOver (List<BotBehavior> individuals, int populationGoalSize);
}
