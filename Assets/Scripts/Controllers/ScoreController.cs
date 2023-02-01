using UnityEngine;

public class ScoreController : MonoBehaviour
{
    [SerializeField]
    private GameObject cookie;

    /// <summary>
    /// The score currently obtained by the player
    /// </summary>
    private static int score = 0;
    private int defaultScore = 10;

    private void Start()
    {
        // ensures that scores are reset between runs
        score = 0;
    }

    /// <summary>
    /// Adds 1 the players score and restarts the countdown.
    /// </summary>
    public void AddScore()
    {   
        if(defaultScore >= 4){
            score += defaultScore;
            defaultScore = 10;
        }else{
            score += 4;
            defaultScore = 10;
        }
        GameObject go = Instantiate(cookie);
        go.transform.SetParent(GameObject.Find("ScoreArea").transform);
    }

    public static int GetScore()
    {
        return score;
    }

    public void Punishment(){
        defaultScore -= 2;
    }

}
