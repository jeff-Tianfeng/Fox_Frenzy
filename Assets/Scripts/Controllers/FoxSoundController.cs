using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoxSoundController : MonoBehaviour
{
    [SerializeField]
    public AudioClip audioClip1;
    [SerializeField]
    public AudioClip audioClip2;
    [SerializeField]
    public AudioClip audioClip3;
    [SerializeField]
    public AudioClip audioClip4;
    [SerializeField]
    private FoxHandler foxHandler;
    [SerializeField]
    private ScoreController scoreController;

    private bool isActivate = false;

    private float timer = 0;
    private float delayTime = 3;//the interval between two bell ring.
    private static int soundPlayTimes = 0;//param for cacukating score get intotal.

    private void Start(){
        Vector3 foxPosition = foxHandler.GetFox().transform.position;//get the fox position
        PlayCorrespondSound(1, foxPosition);//play the corresponding sounds
    }

    // Update is called once per frame
    private void Update()
    {   
        if(isActivate)
        {
            RingAtFoxPosition();
            checkIfOverTime();
        }
    }

    /// <summary>
    /// Play random fox bell sounds.
    /// </summary>
    private void PlayCorrespondSound(int i, Vector3 position){
        if(i == 1){
            AudioSource.PlayClipAtPoint(audioClip1, position);
        }if(i == 2){
            AudioSource.PlayClipAtPoint(audioClip2, position);
        }if(i == 3){
            AudioSource.PlayClipAtPoint(audioClip3, position);
        }if(i == 4){
            AudioSource.PlayClipAtPoint(audioClip4, position);
        }

        soundPlayTimes++;
    }
    /// <summary>
    /// Function with Timer that after 4 seconds the bell ring one time.
    /// </summary>
    private void RingAtFoxPosition(){
        int random = Random.Range(1,5); //genreate a random number [1,5]
        timer += Time.deltaTime;//using timer to count time.
        if(timer >= delayTime){
            Vector3 foxPosition = foxHandler.GetFox().transform.position;//get the fox position
            PlayCorrespondSound(random, foxPosition);//play the corresponding sounds
            timer = 0;
        }
    }

    private void checkIfOverTime(){
        if((soundPlayTimes % 4) == 0){
            scoreController.Punishment();
        }
    }
    /// <summary>
    /// return param soundOlayTime.
    /// </summary>
    public static int returnSoundPlayTime(){
        return soundPlayTimes;
    }

    public void setIsActivate(bool active)
    {
        isActivate = active;
    }

}
