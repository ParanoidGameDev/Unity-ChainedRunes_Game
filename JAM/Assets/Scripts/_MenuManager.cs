using UnityEngine;
using UnityEngine.SceneManagement;

public class _MenuManager : MonoBehaviour
{
    [SerializeField] private _InputManager input;

    public static bool settings = false;
    
    private void LateUpdate() {
        if (this.input.escape) {
            if (settings) this.CloseSettings();
            else if(!settings) this.OpenSettings(); 
        }
    }

    public void ChangeScene(string sceneName) {
        Time.timeScale = 1.0f;
        if(settings) settings = !settings;
        SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
    }

    public void ShowCredits() {
        SceneManager.LoadSceneAsync("Credits", LoadSceneMode.Additive);
    }

    public void HideCredits() {
        SceneManager.UnloadSceneAsync("Credits");
    }

    public void CloseSettings() {
        Time.timeScale = 1.0f;
        SceneManager.UnloadSceneAsync("Pause");
        settings = !settings;
    }
    public void OpenSettings() {
        for (int i = 0; i < SceneManager.sceneCount; i++) {
            Scene scene = SceneManager.GetSceneAt(i);
            if (scene.name == "Pause") {
                return;
            }
        }
        SceneManager.LoadSceneAsync("Pause", LoadSceneMode.Additive);
        Time.timeScale = 0.0f;
        settings = !settings;
    }

    public void QuitGame() {
        #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
