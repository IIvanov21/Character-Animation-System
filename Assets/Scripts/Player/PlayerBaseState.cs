using UnityEngine;

/*
 * Base class for all player states, inherits from the default State class...
 */
public abstract class PlayerBaseState : State
{
    /*
     * Reference to the Player State Machine
     */
    protected PlayerStateMachine stateMachine;

    public PlayerBaseState(PlayerStateMachine stateMachine)
    {
        this.stateMachine = stateMachine;
    }
}
