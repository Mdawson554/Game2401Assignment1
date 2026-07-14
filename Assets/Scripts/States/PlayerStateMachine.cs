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
            private PlayerController playerController;
            public string current;
            
            public PlayerInteractor playerInteractor;
        
            public void IntializeState(IStates startingState)
            {
                changeState(startingState);
            }

            public override void changeState(IStates newState)
            {
                current = newState.ToString();
                base.changeState(newState);
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

            public void Start()
            {
                playerController = GetComponent<PlayerController>();
            }

            public PlayerStateMachine(PlayerController playerController)
            {
                this.walkstate = new WalkingState(playerController);
                this.idlestate = new IdleState(playerController);
                this.dialoguestate = new DialogueState(playerController);
                this.PauseState = new PauseState(playerController);
            }
    }
}
