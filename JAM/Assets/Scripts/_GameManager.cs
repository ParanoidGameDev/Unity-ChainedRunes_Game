using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    // ! Runes declarations
    public readonly List<string> runesPool =
        new()
        {
            "A",
            "B",
            "C",
            "D",
            "F",
            "I",
            "K",
            "M",
            "N",
            "O",
            "P",
            "R",
            "S",
            "T",
            "X",
            "Y",
            "Z",
        };
    public List<Sprite> runeSprites; // This preloads from te Unity inspector

    // ! Managing Runes
    public List<int> userRunesIndex; // This preloads from RunesPool
    public List<Sprite> userRunesSprites; // Mirror variable to set sprite when called from Rune.cs
    public List<string> tempRuneList; // Temp list to iterate usable runes

    // ! Enemies
    public Transform enemyZone; // This preloads from te Unity inspector
    public GameObject enemy; // This will be generated after
    public GameObject enemyPrefab; // This preloads from te Unity inspector

    // Start is called before the first frame update
    // Update is called once per frame
    void Awake()
    {
        CreateNewEnemy();
        // InvokeRepeating("CreateNewEnemy", 0f, 5f);
    }

    public void CreateNewEnemy()
    {
        if (enemy)
            Destroy(enemy);

        // Create temp list to iterate runes
        tempRuneList = new List<string>(runesPool);

        // Get 6 random runes checking that are not repeated
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, tempRuneList.Count);

            // Check if this randomIndex is already in the list
            while (userRunesIndex.Contains(randomIndex))
            {
                // If it IS in the list, generate another random to try again
                randomIndex = Random.Range(0, tempRuneList.Count);
            }

            // If it is NOT in the list, then add it
            userRunesIndex.Add(randomIndex);
        }

        // Adding Runes sprites to its var
        foreach (var x in userRunesIndex)
        {
            userRunesSprites.Add(runeSprites[x]);
        }

        // Lastly, we preload an Enemy
        enemy = Instantiate(enemyPrefab, enemyZone.position, Quaternion.identity, enemyZone);

        // ^ Debug ========================
        // __CustomGlobalFunctions.DebugList(UserRunesIndex, "Final UserRunesIndex: ", ".");
    }

    public int GetRandomRuneIndex()
    {
        return Random.Range(0, userRunesIndex.Count);
    }

    public List<int> GetRunesPoolIndex()
    {
        return userRunesIndex;
    }
}
