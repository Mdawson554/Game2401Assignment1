using System;
using Gameplay;
using States.StateTypes;

namespace States
{
    [Serializable]
    public class PlayerStateMachine : BaseStateMachine
    {
            public IStates CurrentState  { get; private set; }
            public WalkingState walkstate;
            public IdleState idlestate;
            public DialogueState dialoguestate;
            public PauseState PauseState;
            
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
                if (CurrentState != null)
                {
                    CurrentState.ExecuteState();
                }
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
