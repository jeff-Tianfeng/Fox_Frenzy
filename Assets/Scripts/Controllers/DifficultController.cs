using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultController : MonoBehaviour
{
    [SerializeField]
    public FakeFoxGenerator fakeFoxGenerator;
    [SerializeField]
    public FoxHandler foxHandler;
    [SerializeField]
    public GameController gameController;
    [SerializeField]
    public FoxSoundController foxSoundController;
    // List to store deviations dynamically.
    private List<float> deviations = new List<float>();
    // take one count each 50 frames.
    private int countInterval = 50;
    // how many times that collect the fox.
    private int collectCount = 0;
    // Average deviation between player and fox.
    private float avgDeviation = 0;
    
    void Update()
    {
        if(Time.frameCount % countInterval == 0)
        {
            collectCount++;
            collectDeviation();
            avgDeviation = calculateAvgDeviation();
            // based on the performance, choose difficulty level.
            if(avgDeviation <= 30)
            {
                MediumMode();
            }else if(avgDeviation <= 15)
            {   
                HardMode();
            }else{
                EasyMode();
            }
        }
    }
    /// <summary>
    /// Append deviations value to the list.
    /// </summary>
    private void collectDeviation()
    {
        if(gameController != null){
            deviations.Add(gameController.GetPlayerAngleToFox());
        }
    }
    /// <summary>
    /// Calculate the average deviation.
    /// </summary>
    private float calculateAvgDeviation()
    {
        float temp = 0;
        for(int i = 0 ; i < deviations.Count ; i++) 
        {
            temp += deviations[i];
        }
        temp = temp / collectCount;
        return temp;
    }

    private void EasyMode()
    {
        if(fakeFoxGenerator != null)
        {
            foxSoundController.setSoundDeviation(3);
            fakeFoxGenerator.setDifficult(false);
        }
    }

    private void MediumMode()
    {
        if(fakeFoxGenerator != null)
        {
            foxSoundController.setSoundDeviation(5);
            fakeFoxGenerator.setDifficult(true);
        }
    }
    
    private void HardMode()
    {
        if(fakeFoxGenerator != null)
        {
            foxSoundController.setSoundDeviation(7);
            fakeFoxGenerator.setDifficult(true);
        }
    }
}
