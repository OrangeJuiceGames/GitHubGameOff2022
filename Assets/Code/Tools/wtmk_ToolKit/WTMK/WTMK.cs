using System;
using System.Linq;
using System.Collections.Generic;

public sealed class WTMK
{
    private static readonly WTMK _Instance = new WTMK();
    private System.Random _Rando = new System.Random();

    public static WTMK Instance 
    { 
        get { return _Instance; }
    }

    public System.Random Rando
    {
        get { return _Rando; }
    }

    public int Pick(int max)
    {
        return _Rando.Next(max);
    }

    public int Range(int min, int max)
    {
        return _Rando.Next(min,max);
    }

    public void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = _Rando.Next(n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    public List<T> ShiftLeft<T>(List<T> list, int shiftBy)
    {
        if (list.Count <= shiftBy)
        {
            return list;
        }

        var result = list.GetRange(shiftBy, list.Count - shiftBy);
        result.AddRange(list.GetRange(0, shiftBy));
        return result;
    }

    public List<T> ShiftRight<T>(List<T> list, int shiftBy)
    {
        if (list.Count <= shiftBy)
        {
            return list;
        }

        var result = list.GetRange(list.Count - shiftBy, shiftBy);
        result.AddRange(list.GetRange(0, list.Count - shiftBy));
        return result;
    }

    public List<T> GetEnumValues<T>()
    {
        List<T> enumList = Enum.GetValues(typeof(T)).Cast<T>().ToList();
        return enumList;
    }
}