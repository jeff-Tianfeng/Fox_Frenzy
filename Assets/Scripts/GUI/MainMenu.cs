using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField]
    private string GameSceneName = "SoundTest";

    public void StartGame() => SceneManager.LoadSceneAsync(GameSceneName);
    public void Exit() => Application.Quit();
}
