using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Rune : MonoBehaviour
{
    public _GameManager Manager;

    public int uniqueRuneIndex;

    private void Update()
    {
        if (Manager.runesClickedByUser.Count == Manager.killingRunes.Count)
        {
            this.GetComponent<Button>().interactable = false;
            Destroy(this.gameObject, 0.5f);
        }
    }

    void Start()
    {
        GetRune();
    }

    public void GetRune()
    {


        Manager = GameObject.Find("Manager").GetComponent<_GameManager>();

        // Get the current mouse click position when clicking inside a Rune
        this.GetComponent<Button>().onClick.AddListener(delegate { Manager.GetClickCoordinates(transform.position); });
        Debug.Log("Listener created!");

        int randomIndex = Manager.GetRandomRuneIndex();
        this.GetComponent<Image>().sprite = Manager.userRunesSprites[randomIndex];

        uniqueRuneIndex = Manager.userRunesIndex[randomIndex];

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
    }

    public void OnClickOnRune()
    {
        Manager.runesClickedByUser.Add(uniqueRuneIndex);
        // ^ Debug ========================
        // __CustomGlobalFunctions.DebugList(
        //     Manager.runesClickedByUser,
        //     "runesClickedByUser: ",
        //     "No runes clicked by user."
        // );
    }
}
