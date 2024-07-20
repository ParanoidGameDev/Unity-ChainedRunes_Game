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
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();
        // health = 4;

        // Loop through the health value to determine the runes to be used for killing
        for (int i = 0; i < health; i++)
        {
            int randomIndex;
            do
            {
                // Get a random index within the range of available rune names
                randomIndex = Random.Range(0, Manager.RunesPool.Count);
            } while (killingRunes.Contains(randomIndex));

            // Add the selected random index to the list of killing rune indices
            killingRunes.Add(randomIndex);
        }

        killingRunes.Sort();
    }
}
