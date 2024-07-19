using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class _GameManager : MonoBehaviour
{
    public List<string> RuneNames = new List<string>
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
    public List<Sprite> RuneSprites;

    public List<string> currentRuneNames;
    public List<Sprite> currentRuneSprites;

    // Start is called before the first frame update
    public int GetRandomRune()
    {
        return Random.Range(0, currentRuneNames.Count - 1);
    }

    // Update is called once per frame
    void Awake()
    {
        NewEnemy();
    }

    public void NewEnemy()
    {
        currentRuneNames = RuneNames;
        currentRuneSprites = RuneSprites;
    }
}
