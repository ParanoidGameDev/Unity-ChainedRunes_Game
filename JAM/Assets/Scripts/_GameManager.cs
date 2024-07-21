using System.Collections.Generic;
using TMPro;
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
    public Transform enemyZone; // This preloads from the Unity inspector
    public GameObject enemy; // This will be generated after
    public GameObject enemyPrefab; // This preloads from te Unity inspector

    // ! Managin Gameplay
    public Player player; // This preloads from te Unity inspector
    public float enemyTimer = 6f; // Time (in seconds?) for each enemy
    public TextMeshProUGUI timerText; // This preloads from te Unity inspector
    public int score = 0; // This will update on each
    public TextMeshProUGUI scoreText; // This will update on each

    // ! Managin Runes

    public GameObject runePrefab; // This preloads from the Unity inspector
    public Transform gameCanvas; // This preloads from the Unity inspector
    public List<GameObject> runesList; // Rune objects generated per enemy

    void Update()
    {
        if (player.playerHealth > 0)
        {
            if (
                runesCountDoesMatter
                && killingRunes.Count > 0 // This must be > 0 to avoid looping when resseting
                && runesClickedByUser.Count == killingRunes.Count
            )
            {
                Debug.Log("First set");
                CompareRunes();
            }
            if (enemy && enemyTimer > 0f)
            {
                enemyTimer -= Time.deltaTime;
                string enemyTimerStr = enemyTimer.ToString();
                timerText.text =
                    enemyTimerStr.Length >= 4 ? enemyTimer.ToString()[..4] : enemyTimerStr;
            }

            if (enemyTimer <= 0f && enemyTimer > -1f)
            {
                enemyTimer = -2f;
                Debug.Log("Third set");
                StageEnds(false);
                return;
            }
        }
    }

    void Awake()
    {
        scoreText.text = $"{score}";
        CreateNewEnemy();
        // InvokeRepeating("CreateNewEnemy", 0f, 5f);
    }

    public void CreateNewEnemy()
    {
        if (enemy)
            Destroy(enemy);

        enemyTimer = 6f;
        timerText.text = enemyTimer.ToString();

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

        int r = 0;
        // Adding Runes sprites to its var
        foreach (var x in userRunesIndex)
        {
            userRunesSprites.Add(runeSprites[x]);
            GameObject newRune = Instantiate(runePrefab, gameCanvas);
            newRune.transform.localPosition = Vector3.zero;
            r++;
            // Instanciar las Runas manualmente en una lista
            runesList.Add(newRune);
            runesList[r-1].GetComponent<Animator>().Play("rune" + r);
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
        bool youWin = IsRunesExact(runesClickedByUser, killingRunes);
        // ***************************************
        // ** For future updates and features
        // ***************************************
        // youWin =
        // runesCountDoesMatter
        // && runesOrderDoesMatter
        // && runesClickedByUser.Count == killingRunes.Count
        // ^ Order DOES matters
        // ? youWin && IsRunesExact(runesClickedByUser, killingRunes)
        // ^ Order DOESN'T matters
        // : youWin && IsRunesAnyOrder(runesClickedByUser, killingRunes);
        // ^ Count DOESN'T matters and order matters
        // youWin = (runesOrderDoesMatter && IsRunesAnyOrder(runesClickedByUser, killingRunes));
        // ^ Count DOESN'T matters and order DOESN'T matters
        // youWin = (IsRunesAnyOrder(runesClickedByUser, killingRunes));
        // ***************************************
        // ***************************************

        // ? And nothing else matters ♫ ♪ ♫ ...

        Debug.Log(
            $"===================== DID YOU WIN? ===================== >> {youWin.ToString().ToUpper()}"
        );

        StageEnds(youWin);
    }

    private void StageEnds(bool winStatus)
    {
        timerText.text = "END!";
        enemyTimer = -2f;

        if (enemy)
            Destroy(enemy);

        if (!winStatus)
        {
            Debug.Log("Player gets 1 damage");
            player.playerHealth -= 1;
            // TODO: Enemy fleeing animation plays here
            Debug.Log("Enemy flee away!");
        }
        else
        {
            // TODO: Enemy death animation plays here
            Debug.Log("Enemy dies!");
            // +1 to score
            score += 1;
            scoreText.text = $"{score}";
        }

        Debug.Log("Cleaning runes lists");
        userRunesIndex = new();
        userRunesSprites = new();
        runesClickedByUser = new();
        killingRunes = new();
        runesList = new();

        Debug.Log("Cleaning runes lists");
        userRunesIndex = new();
        userRunesSprites = new();
        runesClickedByUser = new();
        killingRunes = new();
        runesList = new();

        if (player.playerHealth > 0)
        {
            // Wait 2 seconds then create the next enemy
            Invoke(nameof(CreateNewEnemy), 2f);
        }
        else
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        // TODO: Player death animation plays here
        timerText.text = "GAME OVER";

        // TODO: Navigate to GameOver scene
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
