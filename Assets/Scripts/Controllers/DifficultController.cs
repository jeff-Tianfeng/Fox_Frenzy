using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultController : MonoBehaviour
{
    public FakeFoxGenerator fakeFoxGenerator;
    public FoxHandler foxHandler;
    public GameController gameController;
    public FoxSoundController foxSoundController;
    private List<float> deviations = new List<float>();
    private int countInterval = 50;
    private int collectCount = 0;
    private float avgDeviation = 0;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.frameCount % countInterval == 0)
        {
            collectCount++;
            collectDeviation();
            avgDeviation = calculateAvgDeviation();
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

    private void collectDeviation()
    {
        deviations.Add(gameController.GetPlayerAngleToFox());
    }

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
