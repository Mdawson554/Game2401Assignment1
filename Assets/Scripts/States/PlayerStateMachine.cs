using System;
using Core;
using Gameplay;
using States.StateTypes;
using UnityEngine;

namespace States
{
    [Serializable]
    public class PlayerStateMachine : BaseStateMachine
    {
            public WalkingState walkstate;
            public IdleState idlestate;
            public DialogueState dialoguestate;
            public PauseState PauseState;
            public  PlayerController playerController;
            public string current;
            
            public PlayerInteractor playerInteractor;
        
            public void IntializeState(IStates startingState)
            {
                changeState(startingState);
            }
            public void TransitionState(IStates nextState)
            {
                changeState(nextState);
            }
            public void ExecuteState()
            {
                if (currentState != null)
                {
                    currentState.ExecuteState();
                }
            }

            public void Pause()
            {
                changeState(PauseState);
            }

            public void Start()
            {
                playerController = GetComponent<PlayerController>();
            }

            public PlayerStateMachine(PlayerStateMachine playerStateMachine)
            {
                walkstate = new WalkingState(this);
                idlestate = new IdleState(this);
                dialoguestate = new DialogueState(this);
                PauseState = new PauseState(this);
            }
    }
}
