using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

/// <summary>
/// Handles the behaviour of the foxes
/// </summary>
public class FoxBehaviour : MonoBehaviour
{
    public GameController gameController;
    // Fox States:
    // Buried - underground, not moving
    // PoppingUp - in the process of popping out of the ground, in motion
    // Resting - above ground, not moving
    private enum State { Buried, PoppingUp, Resting }

    private State currentState = State.Buried;
    // fox pop up radius between player, 10000 = ultimate, fox will pop up all the time.
    [SerializeField]
    private float popUpRadius = 10000;
    // Two state's fox y position (burried and popup).
    [SerializeField]
    private float burrowedYPos = -1f;
    [SerializeField]
    private float poppedUpYPos = 0.1f;
    // popup animation cause time.
    [SerializeField]
    private float timeToPopUp = 2f;
    // after popup, fox take time to run away
    [SerializeField]
    private float timeToRunAway = 100f;
    /// <summary>
    /// The maximum angle between the look direction and the fox which will cause the fox to pop up
    /// </summary>
    [SerializeField]
    private float maximumViewAngleToPopUp = 30;
    // if fox been collected.
    public bool collected = false;

    private float timer = 0;

    private float distanceToPlayer = 100;

    private bool isFoxPopUp = false;

    public bool isTrueFox = true;

    private void Awake()
    {
        //set initial fox y position, also distinguish true between fake.
        FoxDown();
    }

    private void Start()
    {
        //start logging
        gameController.StartNewSearch();
    }

    private void Update()
    {
        distanceToPlayer = gameController.GetPlayerDistanceToFox();
        //decide behaviour based on distance to player and current state
        StairingPrompt();
        FoxPopPrompt();

    }
    /// <summary>
    /// When player close to the fox, tell players stairing at the fox for a while.
    /// <summary>
    private void StairingPrompt(){
        //Debug.Log(distanceToPlayer);
        if(distanceToPlayer < popUpRadius && currentState == State.Buried){
            timer += Time.deltaTime; 
            Title.Instance.Show("Stairing at where the fox may appear!", "Stay still");
        }
    }
    /// <summary>
    /// When player stairing at fox for 3 sceonds then fox pop up.
    /// <summary>
    private void FoxPopPrompt(){
        if (distanceToPlayer < popUpRadius && currentState == State.Buried && timer >=3)
        //&& currentState == State.Buried && isPlayerLookingAtFox()
        {
            StartCoroutine(PopUp());
            
            TurnTowards(gameController.GetPlayerPosition());
            Title.Instance.Show("You found a fox!", "", 60, 30);
            gameController.FoxPoppedUp();
            timer = 0;
        }else if (distanceToPlayer > popUpRadius){
            FoxDown();
            gameController.EndSearch();
            currentState = State.Buried;//reset the state.
        }
    }
    /// <summary>
    /// Make fox y position down the ground.
    /// <summary>
    public void FoxDown(){
        transform.position = SetYComponent(transform.position, burrowedYPos);
    }
    /// <summary>
    /// Makes the fox face towards the post position.
    /// </summary>
    private void TurnTowards(Vector3 postPosition)
    {
        Vector3 foxToPlayer = postPosition - transform.position;
        foxToPlayer.y = 0;

        float angle = Vector3.Angle(foxToPlayer, transform.forward);
        transform.rotation = Quaternion.Euler(0, angle, 0);
    }

    private void OnTriggerEnter()
    {
        OnCollected();
    }

    /// <summary>
    /// Called when the collectable is collected
    /// </summary>
    private void OnCollected()
    {
        //collectable has been collected
        if(gameController!= null)
        {
            gameController.FoxCollected();
        }
    }

    /// <summary>
    /// Triggers the collectable to pop up out of the ground
    /// <para>This function is a coroutine function, so should only be called via StartCoroutine</para>
    /// </summary>
    private IEnumerator PopUp()
    {
        
        currentState = State.PoppingUp;
        yield return MoveToPositionOverTime(SetYComponent(transform.position, poppedUpYPos), timeToPopUp);
        TurnTowards(new Vector3(0,0,0));
        yield return MoveToPositionOverTime(SetYComponent(new Vector3(0,0,0), poppedUpYPos),timeToRunAway);
        currentState = State.Resting;
    }

    /// <summary>
    /// Linearly interpolates the transform from the current position to a new position over time.
    /// <para>This function is a coroutine function, so should only be called via StartCoroutine</para>
    /// <para>Solution taken from: http://answers.unity.com/answers/1146981/view.html </para>
    /// </summary>
    /// <param name="pos">The position to move to</param>
    /// <param name="time">How long to take to move to the new position</param>
    /// <returns></returns>
    private IEnumerator MoveToPositionOverTime(Vector3 pos, float time)
    {
        float elapsedTime = 0;
        Vector3 startingPos = this.transform.position;
        while (elapsedTime < time)
        {
            transform.position = Vector3.Lerp(startingPos, pos, (elapsedTime / time));
            elapsedTime += Time.deltaTime;
            //Debug.Log(elapsedTime);
            yield return new WaitForEndOfFrame();
        }

        transform.position = pos;
    }

    /// <summary>
    /// Sets the y component of the vector to the value given
    /// </summary>
    /// <param name="vec">The vector to update</param>
    /// <param name="y">The y component</param>
    /// <returns></returns>
    private Vector3 SetYComponent(Vector3 vec, float y)
    {
        vec.y = y;
        return vec;
    }

    /// <summary>
    /// Returns true if player is looking at the fox
    /// </summary>
    /// <returns></returns>
    private bool isPlayerLookingAtFox()
    {
        return gameController.GetPlayerAngleToFox() < maximumViewAngleToPopUp;
    }
    /// <summary>
    /// getters and setters.
    /// </summary>
    public float getDistance(){
        return distanceToPlayer;
    }

    public float getAngle()
    {
        if(gameController!= null)
        {
            return gameController.GetPlayerAngleToFox();
        }
            return 0;
    }

    public void getState()
    {
        Debug.Log(currentState);
    }

    public void setFoxFake()
    {
        isTrueFox = false;
    }

}
