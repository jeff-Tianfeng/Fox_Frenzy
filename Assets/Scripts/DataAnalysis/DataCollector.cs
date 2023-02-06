using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Text;
using System;

public class DataCollector : MonoBehaviour
{   
    [SerializeField]
    private FoxBehaviour foxBehaviour;
    [SerializeField]
    public GameController gameController;
    [SerializeField]
    private int countInterval = 50;

    private static float[] dataDistance = new float[1000];
    private static float[] dataAngle = new float[1000];
    private static int foxCollectCount = 0;
    private static int collectTime1 = 0;
    private static int collectTime2 = 0;
    
    private int blockSignal = 10;
    private static int deviateTime = 0;//total times of deviation.
    private bool blocker = true;//for block the update function, allow one deviation count one a time.

    void Update()
    {   
        //this part is for collecting data into two lists.
        if(Time.frameCount % countInterval == 0){
            if(gameController.GetSearchTime() == foxCollectCount){
                dataDistance[collectTime1] = foxBehaviour.getDistance();
                dataAngle[collectTime2] = foxBehaviour.getAngle();
            }else{
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

    /// <summary>
    /// collect total times of deviation over 30 degrees.
    /// </summary>
    private void collectAngleDeviation()
    {
        if(gameController.checkFoxDIviation() >= 30 && blocker == true){
            deviateTime++;
            blocker = false;
        }
        if(gameController.checkFoxDIviation() < 30){
            blocker = true;
        }
    }
    /// <summary>
    /// Save the diatance data into a txt file in folder "Assets"
    /// </summary>
    public void Save(float[] dataSet, int type)
    {
        string  sb = string.Empty;
            for(int j = 0; j<1000; j++)
                sb = sb + dataSet[j] + ',';
        Debug.Log(sb);
        FileStream fs;
        if(type == 1){
            fs = new FileStream(Application.dataPath + "/save.txt", FileMode.Create);//the file storing path can be changed in here.
        }else{
            fs = new FileStream(Application.dataPath + "/Angle.txt", FileMode.Create);//the file storing path can be changed in here.
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
        if(blockSignal != -1){
            blockSignal = gameController.GetTimer();
        }else{
            blockSignal = -1;
        }
    
        if(blockSignal == 2){
            Save(dataDistance,1);
            Save(dataAngle,2);
            blockSignal = -1;
        }
    }
    
    public static int getDeviationTime()
    {
        return deviateTime;
    }
}