using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCollector : MonoBehaviour
{   [SerializeField]
    public DataCollector dataCollector;
    [SerializeField]
    public InputField name;
    [SerializeField]
    public InputField age;
    private string player = "";
    private string playerAge;

    public void LoadGameScene(){
        player = name.text;
        playerAge = age.text;
        SceneManager.LoadScene(1);
        dataCollector.record();
    }

    public void Exit()
    {
        UnityEditor.EditorApplication.isPlaying = false;
        Application.Quit();
    }

    public string returnName(){
        return player;
    }

    public string retrunAge(){
        return playerAge;
    }
}
