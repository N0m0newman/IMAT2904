using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
    public abstract class BTBaseNode
    {
        //Current state of a node
        protected BTNodeStates btCurrentNodeState;


    //Returns node state
    public BTNodeStates nodeState
        {
            get { return btCurrentNodeState; }
        }

    //Evaluates a set of conditions
    public abstract BTNodeStates Evaluate();
}
