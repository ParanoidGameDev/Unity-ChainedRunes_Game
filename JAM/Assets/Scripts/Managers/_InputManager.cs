using UnityEngine;

public class _InputManager : MonoBehaviour
{
    public bool escape;

    private void Update()
    {
        //Pause
        if (Input.GetKeyDown(KeyCode.Escape)) this.escape = true;
        else this.escape = false;
    }
}
