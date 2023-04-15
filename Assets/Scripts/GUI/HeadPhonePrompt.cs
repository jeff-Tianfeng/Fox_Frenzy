using UnityEngine;
using UnityEngine.SceneManagement;

public class HeadPhonePrompt : MonoBehaviour
{
    [SerializeField]
    private AudioSource music;

    [SerializeField]
    private string NextSceneName = "instructions";

    public AudioClip bellring;

    private void Awake()
    {
        music.clip = bellring;
        music.panStereo = -1.0f; // plays the sound from the left speaker
        music.Play();
    }

    public void GoToNextScene()
    {
        SceneManager.LoadSceneAsync(2);
    }

    public void PlaySoundAgain()
    {
        music.Play();
    }
}
