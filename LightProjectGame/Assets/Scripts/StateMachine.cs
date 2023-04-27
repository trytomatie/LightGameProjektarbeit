using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class StateMachine : MonoBehaviour
{
    public List<State> states;
    public State currentState;
    public bool isUpdating = true;

    // Start is called before the first frame update
    public void Start()
    {
        if(currentState == null)
        {
            currentState = states[0];
        }
    }

    public void Update()
    {
        if(isUpdating)
            Tick(CheckStates(currentState));
    }

    public void ManualUpdate()
    {
        Tick(CheckStates(currentState));
    }

    State CheckStates(State state)
    {
        State.StateName stateName = state.stateName;
        foreach (State s in states)
        {
            State.StateName name = s.AnyTransition(gameObject);
            if(name != State.StateName.Empty)
            {
                stateName = name;
            }
            //print(stateName);
        }
        if (state.stateName == stateName)
        {
            stateName = state.Transition(gameObject);
        }
        if(state.stateName != stateName)
        {
            return states.First(state => state.stateName == stateName);
        }
        return state;
    }

    /// <summary>
    /// Handle state functionality
    /// </summary>
    /// <param name="state"></param>
    public void Tick(State state)
    {
        currentState.UpdateState(gameObject);

        if (state == currentState)

        return;

        currentState?.ExitState(gameObject);
        currentState = state;
        currentState.EnterState(gameObject);
    }
}
