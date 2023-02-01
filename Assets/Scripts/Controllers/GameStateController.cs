using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controls the enabling and disabling of scripts, depending on the current game state
///
/// <para>GameObjects with scripts needed to be enabled/disabled need the "Stateful" component attached to them</para>
/// </summary>
public class GameStateController : MonoBehaviour
{
    private static GameStateController _instance;

    public static GameStateController Instance { get { return _instance; } }

    public enum State { Pre, Game, Post }

    private State currentState = State.Pre;

    public State CurrentState { get { return currentState; } }

    private List<Stateful> statefulScripts;

    private void Awake()
    {
        statefulScripts = new List<Stateful>(FindObjectsOfType<Stateful>());

        // ensures that only one exists in the scene
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }

        ChangeState(State.Pre);
    }

    /// <summary>
    /// Enables and disables scripts depending on the given state
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(State state)
    {
        currentState = state;

        // enable all the scripts that are valid in this state
        var shouldEnable = statefulScripts.FindAll(script => script.ValidStates.Contains(currentState));
        shouldEnable.ForEach(s => s.Script.enabled = true);

        // disable all the scripts that are not valid in this state
        var shouldDisable = statefulScripts.FindAll(script => !script.ValidStates.Contains(currentState));
        shouldDisable.ForEach(s => s.Script.enabled = false);

    }
}
