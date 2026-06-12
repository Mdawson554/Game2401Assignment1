namespace States.StateTypes
{
    public class DialogueState : IStates
    {
        private PlayerController player;
        public DialogueState(PlayerController player)
        {
            this.player = player;
        }
        
        public void EnterState()
        {
            player.ToggleMovement(false);
        }
        public void ExecuteState()
        {
            
        }
        public void ExitState()
        {
           
        }
    }
}
