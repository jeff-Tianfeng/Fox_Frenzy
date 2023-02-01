using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameOverScreen : MonoBehaviour
{
    [SerializeField]
    private string GameSceneName = "game";
    [SerializeField]
    private string DataAnalysisScreen = "dataAnalysis";

    [SerializeField]
    private TextMeshProUGUI scoreText;

    void Awake()
    {
        scoreText.SetText("Your score is:" + ScoreController.GetScore().ToString());
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void Restart()
    {
        SceneManager.LoadSceneAsync(GameSceneName);
    }

    public void ExitGame()
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    public void DataAnalysis()
    {
        SceneManager.LoadSceneAsync(DataAnalysisScreen);
    }

}
