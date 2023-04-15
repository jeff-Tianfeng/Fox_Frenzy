using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCollector : MonoBehaviour
{   
    [SerializeField]
    public DataCollector dataCollector;
    [SerializeField]
    public InputField name;
    [SerializeField]
    public InputField age;
    private string player = null;
    private string playerAge = null;

    private int lifeTime = 2;
    /// <summary>
    /// Give limitations on the player input.
    /// </summary>
    public void LoadGameScene(){
        player = name.text;
        playerAge = age.text;
        if(player.Length != 0 && playerAge.Length != 0)
        {
            SceneManager.LoadScene(3);
            dataCollector.record();
        }else
        {
            emptyInput();
        }
        
    }

    public void Exit()
    {
        Application.Quit();
    }

    public string returnName(){
        return player;
    }

    public string retrunAge(){
        return playerAge;
    }
    /// <summary>
    /// Prompt words
    /// </summary>
    private void emptyInput()
    {
        Title.Instance.Show(
            "Name or Age cannt be empty",
            "Please input again",
            50,
            lifeTime: lifeTime
        );
    }
}
