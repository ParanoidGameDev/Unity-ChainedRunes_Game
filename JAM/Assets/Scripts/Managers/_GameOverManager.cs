using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class _GameOverManager : MonoBehaviour
{

    public Transform anchorScore;

    // Start is called before the first frame update
    void Start()
    {

        GameObject score = GameObject.Find("score");

        score.transform.parent = anchorScore;
        score.GetComponent<TextMeshProUGUI>().alignment = TextAlignmentOptions.Center;
        score.transform.localPosition = Vector2.zero;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
