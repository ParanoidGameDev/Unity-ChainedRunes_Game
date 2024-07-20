using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health;
    public List<int> killingRunes;
    public _GameManager Manager;

    void Awake()
    {
        GetKillingRunes();
    }

    public void GetKillingRunes()
    {
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();

        // Loop through the health value to get the runes to be used for killing
        for (int i = 0; i < health; i++)
        {
            killingRunes.Add(Manager.GetRunesPoolIndex()[i]);
        }

        // ^ Debug ========================
        // __CustomGlobalFunctions.DebugList(killingRunes, "Total killingRunes: ", "No killingRunes.");
    }
}
