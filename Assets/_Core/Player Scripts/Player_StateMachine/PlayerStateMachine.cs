
using System;

public class PlayerStateMachine
{
    public PlayerState currentState { get; private set; }

    //Initilized First Sttate 
    public void Initilization(PlayerState _newState)
    {
        currentState = _newState;
        currentState.Enter();
    }
    //Update CurrentState
    public void ExecuteState()
    {
        currentState.Update();
    }
    // Exit Current State //Change  CurrentState // Enter  New State 
    public void ChangeState(PlayerState _newState)
    {
        currentState.Exit();
        currentState = _newState;
        currentState.Enter();
    }

    internal void ChangeState(object slideState)
    {
        throw new NotImplementedException();
    }
}
