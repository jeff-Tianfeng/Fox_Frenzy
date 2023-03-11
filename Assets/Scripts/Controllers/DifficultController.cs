using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DifficultController : MonoBehaviour
{
    public FakeFoxGenerator fakeFoxGenerator;
    public FoxHandler foxHandler;
    public GameController gameController;
    //Game difficult level.
    private enum State { Easy, Medium, Hard };
    private State currentState = State.Easy;
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
                currentState = State.Medium;
            }else if(avgDeviation <= 15)
            {
                currentState = State.Hard;
            }else{
                currentState = State.Easy;
            }
            Debug.Log(currentState);
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
}
