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
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();

        int index = Manager.GetRandomRuneIndex();
        RuneText.text = Manager.userRunes[index].ToString();

        this.gameObject.name = RuneText.text;
        this.GetComponent<SpriteRenderer>().sprite = Manager.currentRuneSprites[index];

        Manager.userRunes.RemoveAt(index);
        Manager.currentRuneSprites.RemoveAt(index);
    }
}
