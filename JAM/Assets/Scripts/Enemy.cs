using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int health; // This preloads from te Unity inspector
    public List<int> killingRunes; // This holds the required runes to kill the enemy
    public _GameManager Manager;
    public List<Sprite> poolRunesSprites; // This preloads from te Unity inspector;
    public GameObject enemyRunePrefab; // This preloads from te Unity inspector
    public Transform runeOrigin; // This preloads from te Unity hierarchy
    public List<GameObject> runesEnemylist; // This temporary saves the enemy shown runes

    void Awake()
    {
        GetKillingRunes();
    }

    public void GetKillingRunes()
    {
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();
        runeOrigin = GameObject.Find("enemyRunes").transform;

        // Loop through the health value to get the runes to be used for killing
        for (int i = 0; i < health; i++)
        {
            killingRunes.Add(Manager.GetRunesPoolIndex()[i]);
        }

        OrderRunesVisual();

        // ^ Debug ========================
        // __CustomGlobalFunctions.DebugList(killingRunes, "Total killingRunes: ", "No killingRunes.");
    }

    public void OrderRunesVisual()
    {
        List<Vector2> three =
            new() { new Vector2(-0.7f, 0.8f), new Vector2(0, 0.8f), new Vector2(0.7f, 0.8f) };
        List<Vector2> four =
            new()
            {
                new Vector2(-0.35f, 0.7f + 0.8f),
                new Vector2(0.35f, 0.7f + 0.8f),
                new Vector2(-0.35f, 0.0f + 0.8f),
                new Vector2(0.35f, 0.0f + 0.8f),
            };
        List<Vector2> five =
            new()
            {
                new Vector2(-0.35f, 0.7f + 0.8f),
                new Vector2(0.35f, 0.7f + 0.8f),
                new Vector2(-0.7f, 0 + 0.8f),
                new Vector2(0, 0 + 0.8f),
                new Vector2(0.7f, 0 + 0.8f)
            };

        List<Vector2> vectors = health switch
        {
            3 => three,
            4 => four,
            5 => five,
            _ => throw new System.NotImplementedException(),
        };

        int j = 0;
        foreach (int rune in killingRunes)
        {
            GameObject runeEnemy = Instantiate(enemyRunePrefab, runeOrigin);
            runeEnemy.transform.localPosition = vectors[j];
            runeEnemy.GetComponent<SpriteRenderer>().sprite = poolRunesSprites[rune];
            j++;
            runesEnemylist.Add(runeEnemy);
        }
    }

    private void OnDestroy()
    {
        foreach (var r in runesEnemylist)
        {
            Destroy(r);
        }
    }
}
