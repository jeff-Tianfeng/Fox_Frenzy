using System.Collections.Generic;
using UnityEngine;


/// <summary>
/// Controls which scripts should be enabled and disabled for each game state
/// </summary>
public class Stateful : MonoBehaviour
{
    [SerializeField]
    private List<GameStateController.State> validStates;

    [SerializeField]
    private MonoBehaviour script;

    public List<GameStateController.State> ValidStates { get { return validStates; } }

    public MonoBehaviour Script { get { return script; } }
}
