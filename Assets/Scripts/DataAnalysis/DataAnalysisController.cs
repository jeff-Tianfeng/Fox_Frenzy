using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;
using System.Diagnostics;

public class DataAnalysisController : MonoBehaviour
{

    [SerializeField]
    private TextMeshProUGUI bellRingTimeText;
    [SerializeField]
    private TextMeshProUGUI deviationTimeText;
    // name of python file
    string sArguments = @"main.py";

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
            RunPythonScript(sArguments, "-u");
        }
 
    public static void RunPythonScript(string sArgName, string args = "")
    {
        Process p = new Process();
		//python path.
		string path = @"C:\Users\27996\Desktop\Fox_Frenzy\DataAnalysisTool\" + sArgName;
        string sArguments = path;
 
	    p.StartInfo.FileName = @"python.exe";
	
		p.StartInfo.UseShellExecute = false;
        p.StartInfo.Arguments = sArguments; 
        p.StartInfo.RedirectStandardOutput = true;
        p.StartInfo.RedirectStandardInput = true;
        p.StartInfo.RedirectStandardError = true;
        p.StartInfo.CreateNoWindow = true;
        p.Start();
        p.BeginOutputReadLine();
		p.OutputDataReceived += new DataReceivedEventHandler (Out_RecvData);
        Console.ReadLine();
        p.WaitForExit();
    }
 
	static void Out_RecvData(object sender, DataReceivedEventArgs e)
	{
		if (!string.IsNullOrEmpty(e.Data)) {
			UnityEngine.Debug.Log (e.Data);
 
		}
	}

}
