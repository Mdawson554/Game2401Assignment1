using System;
using _Project.Scripts.Core;
using _Project.Scripts.EventSystem;
using _Project.Scripts.Gameplay;
using _Project.Scripts.States.StateTypes;
using _Project.Scripts.Tools;
using UnityEngine;

namespace _Project.Scripts.States
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
                DevLogger.Log("game paused");
            }

            public void Resume()
            {
                changeState(idlestate);
                DevLogger.Log("game resumed");
            }

            public void Interact()
            {
                changeState(dialoguestate);
                DevLogger.Log("dialogue interacted");
            }

            public void InteractComplete()
            {
                changeState(idlestate);
                DevLogger.Log("dialogue complete");
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
            
            [ContextMenu("Print Player State")]
            private void PrintPlayerState()
            {
                DevLogger.Log(
                    $"[PlayerState]\n" +
                    $"Current State: {currentState?.GetType().Name}\n" +
                    $"Idle: {idlestate?.GetType().Name}\n" +
                    $"Walk: {walkstate?.GetType().Name}\n" +
                    $"Dialogue: {dialoguestate?.GetType().Name}\n" +
                    $"Pause: {PauseState?.GetType().Name}\n" +
                    $"Controller: {playerController}\n" +
                    $"Input: {inputManager}\n" +
                    $"Interactor: {playerInteractor}"
                );
            }
    }
}
