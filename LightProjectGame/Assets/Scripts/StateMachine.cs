using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using System;

public class StateMachine : MonoBehaviour
{
    public List<State> states;
    public State currentState;
    public bool isUpdating = true;

    private State.StateName forceState = State.StateName.Empty;

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

    public void ForceState(State state)
    {
        forceState = state.stateName;
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
        }
        if (state.stateName == stateName)
        {
            stateName = state.Transition(gameObject);
        }
        if(state.stateName != stateName)
        {
            State sn = currentState;
            try
            {
               sn = states.First(state => state.stateName == stateName);
            }
            catch(Exception e)
            {
                Debug.Log(string.Format("{0} not found, Skipping Transition", stateName));
            }
            return sn;
        }
        if (forceState != State.StateName.Empty)
        {
            state = states.First(state => state.stateName == forceState);
            forceState = State.StateName.Empty;
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
