using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Introduction : MonoBehaviour
{
    [SerializeField]
    private int lifeTime = 8;

    void Start()
    {
        Title.Instance.Show(
            "Get Ready...",
            "",
            50,
            lifeTime: lifeTime
        );
    }

    void Update()
    {
        if (GameStateController.Instance.CurrentState != GameStateController.State.Game && !Title.Instance.Running)
        {
            GameStateController.Instance.ChangeState(GameStateController.State.Game);
        }
    }
}
