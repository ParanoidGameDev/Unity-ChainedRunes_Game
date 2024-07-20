using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public _GameManager Manager;
    public TextMeshPro RuneText;

    void Start()
    {
        GetRune();
    }

    public void GetRune()
    {
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();

        int randomIndex = Manager.GetRandomRuneIndex();
        RuneText.text = Manager.UserRunesIndex[randomIndex].ToString();

        this.gameObject.name = RuneText.text;
        this.GetComponent<SpriteRenderer>().sprite = Manager.UserRunesSprites[randomIndex];

        // ^ Debug ========================
        Debug.Log("Rune removed from pool and assigned: " + Manager.UserRunesIndex[randomIndex]);

        Manager.UserRunesIndex.RemoveAt(randomIndex);
        Manager.UserRunesSprites.RemoveAt(randomIndex);

        // ^ Debug ========================
        // Debug.Log("Remainign runes in pool: " + Manager.UserRunesIndex.Count);

        // ^ Debug ========================
        __CustomGlobalFunctions.DebugList(
            Manager.UserRunesIndex,
            "Remaining Runes in Pool: ",
            "No runes left in pool."
        );

        // }
    }
}
