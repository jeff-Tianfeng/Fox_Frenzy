using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

/// <summary>
/// Json File class.
/// </summary>
public class PlayerPerformanceData
{
    public string NickName;
    public string Age;
    public float[] dataDistanceJS;
    public float[] dataAngleJS;
    public int foxCollectCountJS;
    public int fakeFoxCollectCountJS;
    public int Score;
    public string Coords;
}
/// <summary>
/// Class for extracting data from the game.
/// </summary>
public class DataCollector : MonoBehaviour
{   
    [SerializeField]
    private MainMenuCollector mainMenuController;
    [SerializeField]
    private FoxBehaviour foxBehaviour;
    [SerializeField]
    public GameController gameController;
    [SerializeField]
    public ScoreController scoreController;
    [SerializeField]
    private int countInterval = 50;

    PlayerPerformanceData playerPerformance;

    private string JsonPath;

    private static float[] dataDistance = new float[3000];
    private static float[] dataAngle = new float[3000];
    private static int foxCollectCount = 0;
    private static int collectTime1 = 0;
    private static int collectTime2 = 0;
    
    private int blockSignal = 10;
    private static int deviateTime = 0;//total times of deviation.
    private bool blocker = true;//for block the update function, allow one deviation count one a time.

    private string coordTemp;

    void Start()
    {
        if(playerPerformance == null)
        {
            playerPerformance = new PlayerPerformanceData();
        }
    }
    void Update()
    {   
        //this part is for collecting data into two lists, the if statement is for when multi object using one datacollector to avoid congestion.
        if(gameController != null && foxBehaviour != null)
        {   
            if(Time.frameCount % countInterval == 0)
            {
                if(gameController.GetSearchTime() == foxCollectCount)
                {
                    // collect data each 50 frames.
                    dataDistance[collectTime1] = foxBehaviour.getDistance();
                    dataAngle[collectTime2] = foxBehaviour.getAngle();
                }else
                {
                    dataDistance[collectTime1] = -1;//using -1 to divide every run search.
                    dataAngle[collectTime2] = -1;
                    foxCollectCount = gameController.GetSearchTime();//reset the judgement signal.
                }
                collectTime1 = collectTime1 + 1;
                collectTime2 = collectTime2 + 1;
            }
            threadBlocker();
            collectAngleDeviation();
        }
    }
    /// <summary>
    /// collect total times of deviation over 30 degrees.
    /// </summary>
    private void collectAngleDeviation()
    {
        if(gameController != null)
        {
            if(gameController.checkFoxDIviation() >= 30 && blocker == true){
            deviateTime++;
            blocker = false;
        }
        if(gameController.checkFoxDIviation() < 30){
            blocker = true;
        }
        }
    }
    /// <summary>
    /// Save the diatance data into a txt file in folder "Assets"
    /// </summary>
    public void Save(float[] dataSet, int type)
    {
        string  sb = string.Empty;
            for(int j = 0; j<3000; j++)
                sb = sb + dataSet[j] + ',';
        FileStream fs;

        if(type == 1){
            fs = new FileStream(Application.dataPath + "/" + PlayerPrefs.GetString("NickName") + "Distance.txt", FileMode.Create);//the file storing path can be changed in here.
        }else{
            fs = new FileStream(Application.dataPath + "/" + PlayerPrefs.GetString("NickName") + "Deviation.txt", FileMode.Create);//the file storing path can be changed in here.
        }
        byte[] bytes = new UTF8Encoding().GetBytes(sb.ToString());
        fs.Write(bytes, 0, bytes.Length);
        fs.Close();
    }
    
    /// <summary>
    /// Block the over refresh, only game is over then call functiion Save
    /// </summary>
    private void threadBlocker()
    {
        //Debug.Log(gameController.GetTimer());
        if(gameController!= null){

            if(blockSignal != -1){
                blockSignal = gameController.GetTimer();
            }else{
                blockSignal = -1;
            }
    
        if(blockSignal == 5){
            Save(dataDistance,1);
            Save(dataAngle,2);
            // assign values to the JSON class.
            playerPerformance.NickName = PlayerPrefs.GetString("NickName");
            playerPerformance.Age = PlayerPrefs.GetString("Age");
            playerPerformance.dataDistanceJS = dataDistance;
            playerPerformance.dataAngleJS = dataAngle;
            playerPerformance.foxCollectCountJS = foxCollectCount;
            playerPerformance.Score = PlayerPrefs.GetInt("Score");
            playerPerformance.Coords = PlayerPrefs.GetString("Coords");

            JsonPath = Application.streamingAssetsPath + "/" + playerPerformance.NickName + "JsonTest.json";

            string json = JsonUtility.ToJson(playerPerformance, true);
            using (StreamWriter sw = new StreamWriter(JsonPath)){
                sw.WriteLine(json);
                sw.Close();
                sw.Dispose();
        }
            blockSignal = -1;
        }

        }
    }
    
    public static int getDeviationTime()
    {
        return deviateTime;
    }
    /// <summary>
    /// Using PlayerPrefs to temporarily.
    /// </summary>
    public void record(){
        if(mainMenuController != null)
        {
            PlayerPrefs.SetString("NickName", mainMenuController.returnName());
            PlayerPrefs.SetString("Age", mainMenuController.retrunAge());
        }
    }
    /// <summary>
    /// Insert the fox points arrangement information into the list.
    /// </summary>
    public void insertPointInfo(string list)
    {
        PlayerPrefs.SetString("Coords",list);
        PlayerPrefs.SetInt("Score",scoreController.returnScore());
        Debug.Log(scoreController.returnScore() + "dAhu");
    }
}
