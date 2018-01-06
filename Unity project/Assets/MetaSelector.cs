using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/*
 * The idea of this selector is to automatically tune the selector based on whether previous selections were good or not.
 */
using System;


public class MetaSelector : SelectorOperator {

	List<SelectorOperator> selectors;

	public MetaSelector(List<SelectorOperator> selectors){
		this.selectors = selectors;
	}

	public override List<BotBehavior> select (List<KeyValuePair<int,BotBehavior>> evaluatedBehavior, float toKeep){
		//Somehow keep track of every individual, and see 
		throw new NotImplementedException();
	}
}
