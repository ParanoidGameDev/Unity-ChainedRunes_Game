using TMPro;
using UnityEngine;

public class Player : MonoBehaviour
{
    public int playerHealth = 6;
    public TextMeshPro healthText;

    // Start is called before the first frame update
    void Start() { }

    // Update is called once per frame
    void Update()
    {
        healthText.text = playerHealth.ToString();
    }
}
