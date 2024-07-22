using System.Collections.Generic;
using UnityEngine;

public class __CustomGlobalFunctions
{
    public static void DebugList<T>(
        List<T> list,
        string prefixMsg = "",
        string msgIfEmpty = "Provided List is empty."
    )
    {
        if (list.Count == 0)
        {
            Debug.Log($"{msgIfEmpty}");
            return;
        }

        string items = string.Join(", ", list);
        Debug.Log($"{prefixMsg} {items}.");
    }
}
