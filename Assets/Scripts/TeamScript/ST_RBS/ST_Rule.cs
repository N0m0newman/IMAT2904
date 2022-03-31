using System;
using System.Collections.Generic;

public class ST_Rule 
{
    public string antecedentA;
    public string antecedentB;
    public Type consequentAction;
    public Predicate comparitor;
    public enum Predicate
    { AND, OR, NAND}

    public ST_Rule(string antecedentA, string antecedentB, Type consequentAction, Predicate comparitor)
    {
        this.antecedentA = antecedentA;
        this.antecedentB = antecedentB;
        this.consequentAction = consequentAction;
        this.comparitor = comparitor;
    }

    public Type CheckRule(Dictionary<string, bool> stats)
    {
        bool antecedentABool = stats[antecedentA];
        bool antecedentBBool = stats[antecedentB];
        switch (comparitor)
        {
            case Predicate.AND:
                if(antecedentABool && antecedentBBool)
                {
                    return consequentAction;
                }else
                {
                    return null;
                }
            case Predicate.OR:
                if(antecedentABool || antecedentBBool)
                {
                    return consequentAction;
                }
                else
                {
                    return null;
                }
            case Predicate.NAND:
                if(!antecedentABool && !antecedentBBool)
                {
                    return consequentAction;
                }
                else
                {
                    return null;
                }
            default:
                return null;
        }
    }
}
