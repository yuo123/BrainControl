
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public static class MyUtil
{

    public static SignalType GetRandomSignalType(SignalType[] options, int total)
    {
        int rand = Random.Range(0, total - 1);
        foreach (SignalType type in options)
        {
            int weight = type.GetSelectionWeight();
            if (rand < weight)
                return type;
            rand -= weight;
        }
        throw new System.ArgumentException("Invalid Options", "options");
    }

    public static SignalType GetRandomSignalType(SignalType[] options, int total, int rand)
    {
        foreach (SignalType type in options)
        {
            int weight = type.GetSelectionWeight();
            if (rand < weight)
                return type;
            rand -= weight;
        }
        throw new System.ArgumentException("Invalid Options", "options");
    }

    public static int GetImportance(this SignalType type)
    {
        return (int)type & 0x00001111;
    }

    public static int GetSelectionWeight(this SignalType type)
    {
        return SignalController.MAX_HEALTH - type.GetImportance();
    }
}
