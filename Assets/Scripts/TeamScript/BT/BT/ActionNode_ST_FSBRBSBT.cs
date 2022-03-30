using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BTActionNode : BTBaseNode
{
    public delegate BTNodeStates ActionNodeFunction();

    private ActionNodeFunction btActionNode;

    public BTActionNode(ActionNodeFunction btActionNode)
    {
        this.btActionNode = btActionNode;
    }

    public override BTNodeStates Evaluate()
    {
        {
            switch (btActionNode())
            {
                case BTNodeStates.SUCCESS:
                    btCurrentNodeState = BTNodeStates.SUCCESS;
                    return btCurrentNodeState;
                case BTNodeStates.FAILURE:
                    btCurrentNodeState = BTNodeStates.FAILURE;
                    return btCurrentNodeState;
                default:
                    btCurrentNodeState = BTNodeStates.FAILURE;
                    return btCurrentNodeState;
            }
        }
    }
}
