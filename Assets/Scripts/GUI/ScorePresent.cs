using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class ScorePresent : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI scoreText;

   void Update()
    {
        scoreText.SetText("                                                                                                                                         Score:" + ScoreController.GetScore().ToString());
    }
}
