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

    // ! Difficulty
    public bool runesCountDoesMatter = true;
    public bool runesOrderDoesMatter = true;

    // ! Managing Runes
    public List<int> userRunesIndex; // This preloads from RunesPool
    public List<Sprite> userRunesSprites; // Mirror variable to set sprite when called from Rune.cs
    public List<string> tempRuneList; // Temp list to iterate usable runes
    public List<int> runesClickedByUser; // List that will fill with every Rune click
    public List<int> killingRunes; // This will be filled later when Enemy.cs is called

    // ! Managin Enemies
    public Transform enemyZone; // This preloads from te Unity inspector
    public GameObject enemy; // This will be generated after
    public GameObject enemyPrefab; // This preloads from te Unity inspector

    // ! Managin Player
    public Player player; // This preloads from te Unity inspector

    void Update()
    {
        if (runesCountDoesMatter && runesClickedByUser.Count == killingRunes.Count)
        {
            CompareRunes();
        }
    }

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
            int randomIndex = Random.Range(0, tempRuneList.Count); // Del 0 a las 17 runas

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
            // Instanciar las Runas manualmente en una lista
        }

        // Lastly, we preload an Enemy
        enemy = Instantiate(enemyPrefab, enemyZone.position, Quaternion.identity, enemyZone);
        killingRunes = enemy.GetComponent<Enemy>().killingRunes;

        // ^ Debug ========================
        Debug.Log($"Required runes pressed (enemy health): {enemy.GetComponent<Enemy>().health}");
        __CustomGlobalFunctions.DebugList(
            enemy.GetComponent<Enemy>().killingRunes,
            "Enemy killing runes: "
        );
    }

    public void CompareRunes()
    {
        bool youWin = runesCountDoesMatter && runesClickedByUser.Count == killingRunes.Count;

        youWin = runesOrderDoesMatter
            // ^ Order DOES matters
            ? youWin && IsRunesExact(runesClickedByUser, killingRunes)
            // ^ Order DOESN'T matters
            : youWin && IsRunesAnyOrder(runesClickedByUser, killingRunes);

        // ***************************************
        // ** For future updates and features
        // ***************************************
        // ^ Count DOESN'T matters and order matters
        // youWin = (runesOrderDoesMatter && IsRunesAnyOrder(runesClickedByUser, killingRunes));
        // ^ Count DOESN'T matters and order DOESN'T matters
        // youWin = (IsRunesAnyOrder(runesClickedByUser, killingRunes));
        // ***************************************
        // ***************************************

        Debug.Log(
            $"===================== DID YOU WIN? ===================== >> {youWin.ToString().ToUpper()}"
        );

        if (!youWin)
            player.playerHealth -= 1;

        // Enemy death animation plays here
        runesClickedByUser.Clear();
        killingRunes.Clear();
        CreateNewEnemy();

        // ? And nothing else matters ♫ ♪ ♫ ...
    }

    private bool IsRunesExact(List<int> userRunes, List<int> enemyRunes)
    {
        bool areEqualResults = false;

        for (int i = 0; i < userRunes.Count; i++)
        {
            areEqualResults = userRunes[i] == enemyRunes[i];
            if (!areEqualResults)
                break;
        }

        return areEqualResults;
    }

    private bool IsRunesAnyOrder(List<int> userRunes, List<int> enemyRunes)
    {
        HashSet<int> clickedHash = new(userRunes);
        HashSet<int> enemyHash = new(enemyRunes);
        return enemyHash.SetEquals(clickedHash);
        // return new HashSet<int>(clickedHash).IsSupersetOf(enemyHash);
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
