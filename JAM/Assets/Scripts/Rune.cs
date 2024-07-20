using TMPro;
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
        RuneText.text = Manager.userRunesIndex[randomIndex].ToString();

        this.gameObject.name = RuneText.text;
        this.GetComponent<SpriteRenderer>().sprite = Manager.userRunesSprites[randomIndex];

        // ^ Debug ========================
        // Debug.Log("Rune removed from pool and assigned: " + Manager.UserRunesIndex[randomIndex]);

        Manager.userRunesIndex.RemoveAt(randomIndex);
        Manager.userRunesSprites.RemoveAt(randomIndex);

        // ^ Debug ========================
        // Debug.Log("Remainign runes in pool: " + Manager.UserRunesIndex.Count);

        // ^ Debug ========================
        // __CustomGlobalFunctions.DebugList(
        //     Manager.UserRunesIndex,
        //     "Remaining Runes in Pool: ",
        //     "No runes left in pool."
        // );

        // }
    }
}
