using UnityEngine;
using TMPro;
using System;

/// <summary>
/// GUIManager updates the games GUI.
/// </summary>
public class GUIManager : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timer;

    [SerializeField]
    private GameController gameController;

    private void Update()
    {
        UpdateTimer();
    }

    private void UpdateTimer()
    {
        TimeSpan time = TimeSpan.FromSeconds(gameController.GetTimer());
        timer.SetText(time.ToString(@"m\:ss"));
    }
}
