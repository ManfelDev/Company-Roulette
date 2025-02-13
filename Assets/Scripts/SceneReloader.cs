using UnityEngine;
using UnityEngine.SceneManagement; // Required for scene management

public class SceneReloader : MonoBehaviour
{
    public void ReloadScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void ExitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // Stop Play Mode in the Editor
#else
        Application.Quit(); // Quit the game in a built application
#endif
    }
}
