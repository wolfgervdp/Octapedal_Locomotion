using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class MutationOperator {

	public abstract List<BotBehavior> mutate (List<BotBehavior> individuals, float mutationProbability);
}
