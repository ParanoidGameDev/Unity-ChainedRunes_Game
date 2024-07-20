using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    public readonly List<string> RunesPool = new List<string>
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
    public List<Sprite> runeSprites;
    public List<string> currentRuneNames;
    public List<string> tempRuneList;
    public List<int> userRunes;
    public List<Sprite> currentRuneSprites;

    // ! Enemy
    public Transform enemyZone;
    public GameObject enemy;
    public GameObject enemyPrefab;
    public List<int> currentEnemyKillingRunes;

    // Start is called before the first frame update
    public int GetRandomRuneIndex()
    {
        return Random.Range(0, userRunes.Count);
    }

    // Update is called once per frame
    void Awake()
    {
        NewEnemy();
        // InvokeRepeating("NewEnemy", 0f, 5f);
    }

    public void NewEnemy()
    {
        if (enemy)
        {
            Destroy(enemy);
        }

        enemy = Instantiate(enemyPrefab, enemyZone.position, Quaternion.identity, enemyZone);

        currentRuneNames = new List<string>(RunesPool);
        // currentRuneSprites = new List<Sprite>(RuneSprites);

        List<int> killingRunes = enemy.GetComponent<Enemy>().killingRunes;
        // ?? Debug.Log("killingRunes.Count: " + killingRunes.Count);

        // Remove killingRunes from currentRuneNames
        for (int i = killingRunes.Count; i > 0; i--)
        {
            currentRuneNames.RemoveAt(i);
            Debug.Log("removed from pool = i: " + i);
            // currentRuneSprites.RemoveAt(i);
        }

        userRunes = new List<int>(killingRunes);
        // ?? Debug.Log(
        // ??     "killingRunes.Count: " + killingRunes.Count + " == userRunes.Count: " + userRunes.Count
        // ?? );

        // ?? Debug.Log("Despues de borrar las runes del enemigo: " + currentRuneNames.Count);
        // ?? Debug.Log("killingRunes.Count: " + killingRunes.Count);

        // Get remaining random runes from currentRuneNames
        for (int j = 0; j < 6 - killingRunes.Count; j++)
        {
            // ?? Debug.Log(j);
            int randomIndex = Random.Range(0, currentRuneNames.Count);
            userRunes.Add(randomIndex);
            currentRuneNames.RemoveAt(randomIndex);
            Debug.Log("removed from pool = randomIndex: " + randomIndex);
            // currentRuneSprites.RemoveAt(randomIndex);
        }

        // ?? Debug.Log("Despues de borrar el restante para 6: " + currentRuneNames.Count);
        // ?? Debug.Log("killingRunes.Count: " + killingRunes.Count);

        // ?? string result = "UserRunes: ";
        // ?? foreach (var rune in userRunes)
        // ?? {
        // ??     result += rune.ToString() + ", ";
        // ?? }
        // ?? Debug.Log(result[..^2] + ".");

        foreach (var x in userRunes)
        {
            Debug.Log(x);
            currentRuneSprites.Add(runeSprites[x]);
        }
    }
}
