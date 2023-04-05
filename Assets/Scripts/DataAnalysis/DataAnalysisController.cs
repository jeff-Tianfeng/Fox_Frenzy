 using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;

public class DataAnalysisController : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI bellRingTimeText;
    [SerializeField]
    private TextMeshProUGUI deviationTimeText;

    void Awake()
    {
        bellRingTimeText.SetText("Total bell ring times: " + FoxSoundController.returnSoundPlayTime().ToString());
        deviationTimeText.SetText("Total deviate times: " + DataCollector.getDeviationTime().ToString());
        Cursor.lockState = CursorLockMode.Confined;
    }
    /// <summary>
    /// Start the python scripy to show the player's performance.
    /// </summary>
    public void Button_Click()
        {
            //Process.Start("/Users/zhaotianfeng/Desktop/fox/KL2CSoundNew/DataAnalysisTool/dist/main");
        }

}
