using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ST_Rules
{
    public void AddRule(ST_Rule rule)
    {
        Rules.Add(rule);
    }

    public List<ST_Rule> Rules { get; } = new List<ST_Rule>();
}
