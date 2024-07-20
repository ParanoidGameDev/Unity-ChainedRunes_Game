using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    // ! Runes declarations
    public readonly List<string> RunesPool =
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
    public List<Sprite> RuneSprites; // This preloads from te Unity inspector

    // ! Managing Runes
    public List<int> UserRunesIndex; // This preloads from RunesPool
    public List<Sprite> UserRunesSprites; // Mirror variable to set sprite when called from Rune.cs
    public List<string> TempRuneList; // Temp list to iterate usable runes

    // ! Enemies
    public Transform EnemyZone; // This preloads from te Unity inspector
    public GameObject Enemy; // This will be generated after
    public GameObject EnemyPrefab; // This preloads from te Unity inspector

    // Start is called before the first frame update
    // Update is called once per frame
    void Awake()
    {
        CreateNewEnemy();
        // InvokeRepeating("CreateNewEnemy", 0f, 5f);
    }

    public void CreateNewEnemy()
    {
        if (Enemy)
            Destroy(Enemy);

        // Create temp list to iterate runes
        TempRuneList = new List<string>(RunesPool);

        // Get 6 random runes checking that are not repeated
        for (int i = 0; i < 6; i++)
        {
            int randomIndex = Random.Range(0, TempRuneList.Count);

            // Check if this randomIndex is already in the list
            while (UserRunesIndex.Contains(randomIndex))
            {
                // If it IS in the list, generate another random to try again
                randomIndex = Random.Range(0, TempRuneList.Count);
            }

            // If it is NOT in the list, then add it
            UserRunesIndex.Add(randomIndex);
        }

        // Adding Runes sprites to its var
        foreach (var x in UserRunesIndex)
        {
            UserRunesSprites.Add(RuneSprites[x]);
        }

        // Lastly, we preload an Enemy
        Enemy = Instantiate(EnemyPrefab, EnemyZone.position, Quaternion.identity, EnemyZone);

        // ^ Debug ========================
        __CustomGlobalFunctions.DebugList(UserRunesIndex, "Final UserRunesIndex: ", ".");
    }

    public int GetRandomRuneIndex()
    {
        return Random.Range(0, UserRunesIndex.Count);
    }

    public List<int> GetRunesPoolIndex()
    {
        return UserRunesIndex;
    }
}
