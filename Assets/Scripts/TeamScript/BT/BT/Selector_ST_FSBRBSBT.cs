using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTSelector : BTBaseNode
{
    protected List<BTBaseNode> btNodes = new List<BTBaseNode>();

    public BTSelector(List<BTBaseNode> btNodes)
    {
        this.btNodes = btNodes;
    }


    public override BTNodeStates Evaluate()
    {
        foreach(BTBaseNode btNode in btNodes)
        {
            switch (btNode.Evaluate())
            {
                case BTNodeStates.FAILURE:
                    continue;
                case BTNodeStates.SUCCESS:
                    btCurrentNodeState = BTNodeStates.SUCCESS;
                    return btCurrentNodeState;
                default:
                    continue;
            }
        }
        btCurrentNodeState = BTNodeStates.FAILURE;
        return btCurrentNodeState;
    }
}
