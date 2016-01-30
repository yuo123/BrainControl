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

#if DEBUG
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
#endif

    /// <summary>
    /// Get the enumration number of this type, stored in the left 16 bits of the number
    /// </summary>
    public static int GetEnumIndex(this SignalType type)
    {
        //clear the right 16 bits of the number, by AND-ing it with 11111111111111110000000000000000 (in the code it is written as hexadecimal)
        //then, shift it back to the right so it's not multiplied by 256
        return unchecked((int)(((uint)type & 0xFFFF0000) >> 16));
    }

    /// <summary>
    /// Get the importance of this signal type, stored in the right 16 bits of the number
    /// </summary>
    public static int GetImportance(this SignalType type)
    {
        //clear the left 16 bits of the number, by AND-ing it with 00000000000000001111111111111111 (in the code it is written as hexadecimal)
        return (int)type & 0x0000FFFF;
    }

    /// <summary>
    /// Get the weight used for selecting this signal type. This is inversly proportional to its importance
    /// </summary>
    public static int GetSelectionWeight(this SignalType type)
    {
        return SignalController.MAX_HEALTH - type.GetImportance();
    }
}
