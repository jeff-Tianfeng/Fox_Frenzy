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
    private int collectCount = 0;
    private float avgDeviation = 0;

    // Update is called once per frame
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
        deviations.Add(gameController.GetPlayerAngleToFox());
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
        foxSoundController.setSoundDeviation(3);
        fakeFoxGenerator.setDifficult(false);
    }

    private void MediumMode()
    {
        foxSoundController.setSoundDeviation(5);
        fakeFoxGenerator.setDifficult(true);
    }
    
    private void HardMode()
    {
        foxSoundController.setSoundDeviation(7);
        fakeFoxGenerator.setDifficult(true);
    }
}
