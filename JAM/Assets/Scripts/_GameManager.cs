using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

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
    public bool runesCountDoesMatter = true; // This rule could be implemented to change difficulty
    public bool runesOrderDoesMatter = true; // This rule could be implemented to change difficulty

    // ! Managing Runes
    public List<int> userRunesIndex; // This preloads from RunesPool
    public List<Sprite> userRunesSprites; // Mirror variable to set sprite when called from Rune.cs
    public List<string> tempRuneList; // Temp list to iterate usable runes
    public List<int> runesClickedByUser; // List that will fill with every Rune click
    public List<int> killingRunes; // This will be filled later when Enemy.cs is called

    // ! Managing chains
    public GameObject chainLinkPrefab; // This preloads from te Unity inspector
    public GameObject chainConnectionPrefab; // This preloads from te Unity inspector
    public Transform chainsLocation; // This preloads from te Unity hierarchy
    public List<GameObject> chainsSpawned; //
    public List<Sprite> poolChainsSprites; // This preloads from te Unity inspector;
    public List<Vector2> chainPositions; //This loads when player clicks on runes

    // ! Managin Enemies
    public Transform enemyZone; // This preloads from the Unity inspector
    public GameObject enemy; // This will be generated after

    public List<GameObject> poolEnemies; // This preloads from te Unity inspector
    public int nextEnemyType; // This will dictaminate the next Enemy difficulty

    // ! Managin Gameplay
    public Player player; // This preloads from te Unity inspector
    public float enemyTimerToDefeat; // Time (in seconds?) for each enemy
    public TextMeshProUGUI timerText; // This preloads from te Unity inspector
    public int score = 0; // This is the numeric value
    public TextMeshProUGUI scoreText; // This is the string shown on screen

    // ! Managin Runes
    public GameObject runePrefab; // This preloads from the Unity inspector
    public Transform gameCanvas; // This preloads from the Unity inspector
    public List<GameObject> runesList; // Rune objects generated per enemy

    // ! Managin audio
    public AudioSource enemyZoneAudio1;
    public AudioSource playerZoneAudio;
    public AudioSource enemyZoneAudio2;
    public AudioClip gameOver;
    public AudioClip lose;
    public AudioClip win1;
    public AudioClip win2;
    public AudioClip win3;
    public AudioClip attack;

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
                CompareRunes();
            }
            if (enemy && enemyTimerToDefeat > 0f)
            {
                enemyTimerToDefeat -= Time.deltaTime;
                string enemyTimerStr = enemyTimerToDefeat.ToString();
                timerText.text =
                    enemyTimerStr.Length >= 4 ? enemyTimerToDefeat.ToString()[..4] : enemyTimerStr;
            }

            if (enemyTimerToDefeat <= 0f && enemyTimerToDefeat > -1f)
            {
                enemyTimerToDefeat = -2f;
                StageEnds(false);
                return;
            }
        }
    }

    void Awake()
    {
        scoreText.text = $"{score}";
        Invoke(nameof(CreateNewEnemy), 1);
        // InvokeRepeating("CreateNewEnemy", 0f, 5f);
    }

    public void CreateNewEnemy()
    {
        enemyZone.GetComponent<Animator>().Play("enemyApproaches");

        enemyTimerToDefeat = 6f;

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

        int r = 0;
        // Adding Runes sprites to its var
        foreach (var x in userRunesIndex)
        {
            userRunesSprites.Add(runeSprites[x]);
            GameObject newRune = Instantiate(runePrefab, gameCanvas);
            newRune.transform.localPosition = Vector3.zero;

            runesList.Add(newRune);
            runesList[r].GetComponent<Animator>().Play("rune" + (6 - r));
            r++;
        }

        // Creating a pseudo random value to determine next Enemy type
        int pseudoRandom = Random.Range(0, 100);
        nextEnemyType =
            (pseudoRandom > 80)
                ? 2 // Boss
                : (pseudoRandom > 30)
                    ? 1 // Mage
                    : 0; // Cyclop

        // Lastly, we preload a random Enemy
        enemy = Instantiate(
            poolEnemies[nextEnemyType],
            enemyZone.position,
            Quaternion.identity,
            enemyZone
        );
        killingRunes = enemy.GetComponent<Enemy>().killingRunes;

        // ^ Debug ========================
        // Debug.Log($"Required runes pressed (enemy health): {enemy.GetComponent<Enemy>().health}");
        // __CustomGlobalFunctions.DebugList(
        //     enemy.GetComponent<Enemy>().killingRunes,
        //     "Enemy killing runes: "
        // );
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

        // Debug.Log(
        //     $"===================== DID YOU WIN? ===================== >> {youWin.ToString().ToUpper()}"
        // );

        StageEnds(youWin);
    }

    public void GetClickCoordinates(Vector2 pos)
    {
        chainsLocation = GameObject.Find("chainsLocation").transform;

        chainPositions.Add(pos);

        GameObject newChain = Instantiate(chainLinkPrefab, chainsLocation);

        GameObject newConnection = null;

        chainsSpawned.Add(newChain);

        Vector2 linkPos = new();
        Vector2 connectionPos = new();
        float linkAngle;
        float connectionAngle;
        Quaternion linkQAngle;
        Quaternion connectionQAngle;

        // TODO: Remove rotation from chain link

        for (int c = 0; c < chainPositions.Count; c++)
        {
            if (c < chainsSpawned.Count - 1)
            {
                chainsSpawned[c].GetComponent<SpriteRenderer>().sprite = poolChainsSprites[0];

                linkPos.x = chainPositions[c + 1].x - chainPositions[c].x;
                linkPos.y = chainPositions[c + 1].y - chainPositions[c].y;

            }

            if (c == 0)
            {
                GetComponent<AudioSource>().Play();
                Vector2 mousePositions = Camera.main.ScreenToWorldPoint(Input.mousePosition);

                linkPos.x = mousePositions.x - chainPositions[c].x;
                linkPos.y = mousePositions.y - chainPositions[c].y;
            }
    

            if (c == chainsSpawned.Count - 1)
            {
                if(chainsSpawned.Count > 1)
                {

                    connectionPos.x = chainPositions[c - 1].x - chainPositions[c].x;
                    connectionPos.y = chainPositions[c - 1].y - chainPositions[c].y;

                    newConnection = Instantiate(chainConnectionPrefab, chainsLocation);

                    Vector2 newSize =
                        new()
                        {
                            x = newConnection.GetComponent<SpriteRenderer>().size.x,
                            y = Vector2.Distance(chainPositions[c - 1], chainPositions[c])
                        };

                    newConnection.GetComponent<SpriteRenderer>().size = newSize;

                    if (chainsSpawned.Count < enemy.GetComponent<Enemy>().health)
                    {
                        linkPos.x = chainPositions[0].x - chainPositions[c].x;
                        linkPos.y = chainPositions[0].y - chainPositions[c].y;
                    }
                    else
                    {
                        linkPos.x = chainPositions[c - 1].x - chainPositions[c].x;
                        linkPos.y = chainPositions[c - 1].y - chainPositions[c].y;

                        GameObject finalConnection = Instantiate(
                            chainConnectionPrefab,
                            chainsLocation
                        );

                        newSize = new()
                        {
                            x = newConnection.GetComponent<SpriteRenderer>().size.x,
                            y = Vector2.Distance(chainPositions[0], chainPositions[c])
                        };

                        finalConnection.GetComponent<SpriteRenderer>().size = newSize;

                        Vector2 fconnectionPos =
                            new()
                            {
                                x = chainPositions[c].x - chainPositions[0].x,
                                y = chainPositions[c].y - chainPositions[0].y
                            };

                        float fconnectionAngle =
                            Mathf.Atan2(fconnectionPos.y, fconnectionPos.x) * Mathf.Rad2Deg;
                        Quaternion fconnectionQAngle = Quaternion.Euler(
                            0,
                            0,
                            fconnectionAngle + 90
                        );

                        finalConnection.transform.SetPositionAndRotation(
                            chainsSpawned[0].transform.position,
                            fconnectionQAngle
                        );
                        linkAngle = Mathf.Atan2(linkPos.y, linkPos.x) * Mathf.Rad2Deg;
                        _ = Quaternion.Euler(0, 0, linkAngle + 90);
                    }
                    chainsSpawned[c].GetComponent<SpriteRenderer>().sprite = poolChainsSprites[0];
                }
            }
        }
        connectionAngle = Mathf.Atan2(connectionPos.y, connectionPos.x) * Mathf.Rad2Deg;
        connectionQAngle = Quaternion.Euler(0, 0, connectionAngle + 90);

        linkAngle = Mathf.Atan2(linkPos.y, linkPos.x) * Mathf.Rad2Deg;
        linkQAngle = Quaternion.Euler(0, 0, linkAngle + 90);

        if (chainsSpawned.Count > 1)
        {
            newConnection.transform.SetPositionAndRotation(pos, connectionQAngle);
        }

        newChain.transform.SetPositionAndRotation(pos, linkQAngle);
        // Debug.Log("Chain created at: " + pos.ToString());
    }

    private void StageEnds(bool winStatus)
    {
        enemyTimerToDefeat = -2f;

        for (int k = 0; k < runesList.Count; k++)
        {
            runesList[k].GetComponent<Button>().enabled = false;
        }
        for (int k = 0; k < chainsLocation.childCount; k++)
        {
            Destroy(chainsLocation.GetChild(k).gameObject);
        }

        if (!winStatus)
        {
            enemy.GetComponent<AudioSource>().clip = attack;
            enemy.GetComponent<AudioSource>().Play();

            enemyZone.GetComponent<AudioSource>().clip = lose;
            enemyZone.GetComponent<AudioSource>().Play();

            enemyZone.GetComponent<Animator>().Play("enemyAttack");
            player.GetComponent<Animator>().Play("hurtCharacters");

            // Debug.Log("Player gets 1 damage");
            player.playerHealth -= 1;
            switch (nextEnemyType)
            {
                case 0:
                    // Cyclop deals 1 dmg
                    timerText.text = "Ooff!";
                    player.playerHealth -= 1;
                    break;
                case 1:
                    // Mage deals 2 dmg
                    timerText.text = "Ouch!";
                    player.playerHealth -= 2;
                    break;
                case 2:
                    // Boss deals 3 dmg
                    timerText.text = "Aggh!";
                    player.playerHealth -= 3;
                    break;
            }
            
            if (player.playerHealth < 0)
                player.playerHealth = 0;

            timerText.text = "OUCH!";
        }
        else
        {
            player.GetComponent<AudioSource>().clip = attack;
            player.GetComponent<AudioSource>().Play();

            enemyZone.GetComponent<AudioSource>().Play();
            timerText.text = "---";
            player.transform.parent.GetComponent<Animator>().Play("playerAttack");
            enemy.GetComponent<Animator>().Play("deadCharacters");
            enemy.GetComponent<Enemy>().KillChildRunes();

            switch (nextEnemyType)
            {
                case 0:
                    enemyZone.GetComponent<AudioSource>().clip = win1;
                    // Cyclop gives 1 point
                    timerText.text = "Nice!";
                    score += 1;
                    break;
                case 1:
                    enemyZone.GetComponent<AudioSource>().clip = win2;
                    // Mage gives 2 points
                    timerText.text = "Woo!";
                    score += 2;
                    break;
                case 2:
                    enemyZone.GetComponent<AudioSource>().clip = win3;
                    // Boss gives 5 points
                    timerText.text = "Yeah!";
                    score += 5;
                    // Also heals 1!!
                    player.playerHealth += 1;
                    // TODO: Visually display an icon for health up
                    break;
            }

            scoreText.text = $"{score}";
        }
        Destroy(enemy.gameObject, 2f);
        //Enemy dies
        enemy = null;

        for (int k = 0; k < chainsLocation.childCount; k++)
        {
            Destroy(chainsLocation.GetChild(k).gameObject, 0.5f);
        }

        // Debug.Log("Cleaning runes lists");
        userRunesIndex = new();
        userRunesSprites = new();
        runesClickedByUser = new();
        killingRunes = new();
        runesList = new();
        chainsSpawned = new();
        chainPositions = new();

        if (player.playerHealth > 0)
        {
            // Wait 2 seconds then create the next enemy
            Invoke(nameof(CreateNewEnemy), 2.5f);
        }
        else
        {
            timerText.text = "GAME OVER";
            Destroy(player.gameObject, 3f);
            player.GetComponent<Animator>().Play("deadCharacters");
            enemyZone.GetComponent<AudioSource>().clip = gameOver;
            Invoke(nameof(GameOver), 3f);
        }
    }

    private void GameOver()
    {
        scoreText.transform.SetParent(null);
        DontDestroyOnLoad(scoreText.gameObject);

        SceneManager.LoadSceneAsync("GameOver");
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
