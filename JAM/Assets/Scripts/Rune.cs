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
            // ! Quick fix
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

        int randomIndex = Manager.GetRandomRuneIndex();
        this.GetComponent<Image>().sprite = Manager.userRunesSprites[randomIndex];

        uniqueRuneIndex = Manager.userRunesIndex[randomIndex];

        Manager.userRunesIndex.RemoveAt(randomIndex);
        Manager.userRunesSprites.RemoveAt(randomIndex);
    }

    public void OnClickOnRune()
    {
        // Get the current mouse click position when clicking inside a Rune
        Manager.SetChainLink(transform.position);

        Manager.runesClickedByUser.Add(uniqueRuneIndex);
    }
}
