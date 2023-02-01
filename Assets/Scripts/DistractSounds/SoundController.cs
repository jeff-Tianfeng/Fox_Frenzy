using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Adding distracting Sounds to the background sounds, this sound will rotate in 360
/// and continiously playing.
/// </summary>
public class SoundController : MonoBehaviour
{

    [SerializeField]
    private AudioSource music;

    public AudioClip bellring;

    private float soundMoveSpeed = 0.001f;//initialize the sound moving speed.
    private float soundSpatial = -1.0f;// initialize the sound from left.
    private bool isReachSummit = false;// check if the sound reach most left or right.

    private void Awake()
    {
        music.clip = bellring;
        music.panStereo = soundSpatial;
        music.Play();
    }

    private void Update(){
        music.clip = bellring;
        if(isReachSummit){
            soundSpatial = soundSpatial - soundMoveSpeed;
            if(soundSpatial < -1.0f){
                isReachSummit = false;//start to go right.
            }
        }else{
            soundSpatial = soundSpatial + soundMoveSpeed;
            if(soundSpatial > 1.0f){
                isReachSummit = true;//start to go left.
            }
        }
        //Debug.Log(soundSpatial);
        music.panStereo = soundSpatial;
    }

}
