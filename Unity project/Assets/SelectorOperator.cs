using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SelectorOperator {

	public abstract List<BotBehavior> select (List<KeyValuePair<int,BotBehavior>> evaluatedBehavior, float toKeep);
}
