using System;
using Core;
using EventSystem;
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
            public PlayerController playerController;
            public InputManager inputManager;
            public string current;
            
            public PlayerInteractor playerInteractor;
        
                
            public PlayerStateMachine(PlayerStateMachine playerStateMachine)
            {
                
            }

            public void Pause()
            {
                changeState(PauseState);
                Debug.Log("game paused");
            }

            public void Resume()
            {
                changeState(idlestate);
                Debug.Log("game resumed");
            }

            public void Interact()
            {
                changeState(dialoguestate);
                Debug.Log("dialogue interacted");
            }

            public void InteractComplete()
            {
                changeState(idlestate);
                Debug.Log("dialogue complete");
            }


            public void HandleStateChange(StateChangeEvent stateChangeEvent)
            {
                changeState(stateChangeEvent.assignedState);
            }
            
            
            public void Start()
            {
                playerController = GetComponent<PlayerController>();
                inputManager = GetComponent<InputManager>();
                walkstate = new WalkingState(this);
                idlestate = new IdleState(this);
                dialoguestate = new DialogueState(this);
                PauseState = new PauseState(this);
                changeState(idlestate);
                EventManager.instance.Subscribe<StateChangeEvent>(HandleStateChange);
            }
        
    }
}
