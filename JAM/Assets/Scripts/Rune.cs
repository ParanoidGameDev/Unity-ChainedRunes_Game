using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Rune : MonoBehaviour
{
    public string runeName;
    public _GameManager Manager;

    private void OnEnable() { }

    void Start()
    {
        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();

        int index = Manager.GetRandomRune();
        runeName = Manager.currentRuneNames[index];

        this.gameObject.name = runeName;
        this.GetComponent<SpriteRenderer>().sprite = Manager.currentRuneSprites[index];

        Manager.currentRuneNames.RemoveAt(index);
        Manager.currentRuneSprites.RemoveAt(index);
    }

    void FixedUpdate() { }
}
