using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuCollector : MonoBehaviour
{
    public DataCollector dataCollector;
    private string player = "";
    private string playerAge;
    public InputField name;
    public InputField age;

    public void LoadGameScene(){
        player = name.text;
        playerAge = age.text;
        SceneManager.LoadScene(1);
        dataCollector.record();
    }

    public string returnName(){
        return player;
    }

    public string retrunAge(){
        return playerAge;
    }
}
