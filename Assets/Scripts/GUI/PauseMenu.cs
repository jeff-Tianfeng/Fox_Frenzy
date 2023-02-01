using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Handles the behaviour of the pause menu
/// </summary>
public class PauseMenu : MonoBehaviour
{
    [SerializeField]
    private string GameSceneName = "game";

    private Canvas canvas;

    // it's very important this is static
    // functions that are called by UI cannot change member variables, so this must be static
    private static bool isPaused = false;

    /// <summary>
    /// When pausing the game we expect the cursor to change to a confined state.
    /// And when unpausing we expect it to return to the previous state.
    /// </summary>
    private static CursorLockMode cachedLockState;

    private void Awake()
    {
        canvas = GetComponent<Canvas>();
        Unpause();
    }

    private void Update()
    {
        if (Input.GetKeyDown("escape"))
        {
            if (isPaused)
                Unpause();
            else
                Pause();
        }
    }

    private void Pause()
    {
        isPaused = true;

        Time.timeScale = 0;
        cachedLockState = Cursor.lockState;
        Cursor.lockState = CursorLockMode.Confined;
        canvas.enabled = true;
        AudioListener.pause = true;
    }

    private void Unpause()
    {
        isPaused = false;

        Time.timeScale = 1;
        Cursor.lockState = cachedLockState;
        canvas.enabled = false;
        AudioListener.pause = false;
    }

    public void Restart()
    {
        Unpause();
        SceneManager.LoadSceneAsync(GameSceneName);
    }

    public void Continue()
    {
        Unpause();
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}
