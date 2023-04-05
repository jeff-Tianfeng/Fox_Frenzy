using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FakeFoxBehaviour : MonoBehaviour
{
    [SerializeField]
    private GameController gameController;
    [SerializeField]
    private int lifeTime = 2;

    private int FakeFoxCollectTime = 0;

    private void Start()
    {
        //start logging
    }

    private void OnTriggerEnter()
    {
        collectFakeFox();
    }
    /// <summary>
    /// Fake fox been collected.
    /// </summary>
    private void collectFakeFox()
    {
        FakeFoxCollectTime++;
        gameController.FakeFoxCollected();
        Title.Instance.Show(
            "You found a fake fox",
            "It's bell is not ringing, try again",
            50,
            lifeTime: lifeTime
        );
    }

    public int getFakeFoxCollectTime()
    {
        return FakeFoxCollectTime;
    }
}
