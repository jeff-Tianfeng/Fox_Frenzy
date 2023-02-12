using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the flow of the game and handles inter-gameobject communication
/// </summary>
public class GameController : MonoBehaviour
{

    private float minAngle = 120f;

    [SerializeField]
    private Logger logger;

    [SerializeField]
    private ScoreController scoreController;

    [SerializeField]
    private FoxHandler foxHandler;

    [SerializeField]
    private GameObject player;
    private PlayerMovement playerMovement;

    public static int searchTime = 0;

    /// <summary>
    /// The duration of the countdown timer, in seconds.
    /// </summary>
    [SerializeField]
    private int countdownDuration = 180;

    /// <summary>
    /// If this angle to fox is exceeded, the player is reset to the start
    /// </summary>
    [SerializeField]
    private int foxAngleToReset = 60;

    /// <summary>
    /// The amount of time remaining on the countdown.
    /// </summary>
    private int timeRemaining;

    private bool isFoxHidden = true;

    private AudioSource audioSource;

    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        playerMovement = player.GetComponent<PlayerMovement>();

        StartCoroutine(Countdown());
    }

    private void Update()
    {
        MonitorPlayer();
        IsMissFox();
    }

    /// <summary>
    /// Called when a fox has been collected
    /// </summary>
    public void FoxCollected()
    {
        searchTime++;

        ResetPlayer();

        foxHandler.CollectableCollected();

        scoreController.AddScore();

        audioSource.Play();

        isFoxHidden = true;
    }

    /// <summary>
    /// Begin a new search
    /// </summary>
    public void StartNewSearch()
    {
        logger.StartNewSearch();
    }

    /// <summary>
    /// End the current search
    /// </summary>
    public void EndSearch()
    {
        logger.EndSearch();
    }

    /// <summary>
    /// Monitors the player's current properties to decide whether the player needs to be reset
    /// </summary>
    private void MonitorPlayer()
    {
        if (!isFoxHidden || !checkFoxOutOfViewRange()) return;

        ResetPlayer();
        Title.Instance.Show("You wandered too far!", "Try again!");
        EndSearch();
        StartNewSearch();
        scoreController.Punishment();
    }
    /// <summary>
    /// After the fox popup, this is to check if the fox is in player's view angle.
    /// </summary>
    private void IsMissFox(){
        if(!isFoxHidden && !IsFoxInSight()){
            ResetPlayer();
            Title.Instance.Show("Oh no! You missed the fox", "Try again!");
            EndSearch();
            StartNewSearch();
            scoreController.Punishment();
        }
    }
    /// <summary>
    /// Check if the fox is in player's view angle, if yes return true else return false.
    /// </summary>
    private bool IsFoxInSight(){
        GameObject fox = foxHandler.GetFox();

        Vector3 avatarPos = player.transform.position;
        Vector3 enemyPos = fox.transform.position;
        //The vector of the player relative to the target.
        Vector3 srcLocalVect = enemyPos - avatarPos;
        srcLocalVect.y = 0;
        //Gets a point directly in front of the player.
        Vector3 forwardLocalPos = player.transform.forward * 1 + avatarPos;
        // Get positive direction vector.
        Vector3 forwardLocalVect = forwardLocalPos - avatarPos;
        forwardLocalVect.y = 0;
        //caculate the angle.
        float angle = Vector3.Angle(srcLocalVect, forwardLocalVect);
        if(angle < minAngle/2)
        {
            return true;
        }else{
            return false;
        }
    }

    /// <summary>
    /// Called when a fox has popped up
    /// </summary>
    public void FoxPoppedUp()
    {
        EndSearch();
        playerMovement.UnlockCamera();
        isFoxHidden = false;
    }

    /// <summary>
    /// Resets the player
    /// </summary>
    public void ResetPlayer()
    {
        playerMovement.ResetPlayer();
    }

    /// <summary>
    /// Returns the player's position in world space
    /// </summary>
    /// <returns></returns>
    public Vector3 GetPlayerPosition()
    {
        return player.transform.position;
    }

    /// <summary>
    /// Returns the angle between the player's forward looking direction and the vector from the player to the fox
    /// </summary>
    /// <returns></returns>
    public float GetPlayerAngleToFox()
    {
        //get vector from player to fox 
        Vector3 vecToFox = foxHandler.GetFox().transform.position - player.transform.position;

        //get look direction of the player
        Vector3 lookDir = player.transform.forward;

        //find angle between vectors
        return GetAngleBetween(vecToFox, lookDir);
    }

    /// <summary>
    /// Returns true if the fox has left the view range of the player
    /// </summary>
    /// <returns></returns>
    private bool checkFoxOutOfViewRange()
    {
        //get vector from player to fox 
        Vector3 vecToFox = foxHandler.GetFox().transform.position - player.transform.position;

        return GetAngleBetween(playerMovement.GetBaseForwardVector(), vecToFox) > foxAngleToReset;
    }

    public float checkFoxDIviation()
    {
        Vector3 vecToFox = new Vector3(0,0,0);
        if(foxHandler != null)
        {
            vecToFox = foxHandler.GetFox().transform.position - player.transform.position;
        }
        return GetAngleBetween(playerMovement.GetBaseForwardVector(), vecToFox);
    }

    /// <summary>
    /// Returns the angle between two vectors, ignoring their y values 
    /// </summary>
    private float GetAngleBetween(Vector3 a, Vector3 b)
    {
        a.y = 0;
        b.y = 0;
        return Vector3.Angle(a, b);
    }

    /// <summary>
    /// Get distance from player to fox ignoring y
    /// </summary>
    public float GetPlayerDistanceToFox()
    {
        Vector3 playerPos = player.transform.position;
        playerPos.y = 0;

        Vector3 fox = foxHandler.GetFox().transform.position;
        fox.y = 0;

        return Vector3.Distance(playerPos, fox);
    }

    private IEnumerator Countdown()
    {
        int counter = 0;
        timeRemaining = countdownDuration;

        while (counter < countdownDuration)
        {
            yield return new WaitForSeconds(1);
            counter++;
            timeRemaining = countdownDuration - counter;
        }

        UnityEngine.SceneManagement.SceneManager.LoadScene("GameOver");
    }

    public int GetTimer()
    {
        return timeRemaining;
    }

    public int GetSearchTime(){
        return searchTime;
    }

}
