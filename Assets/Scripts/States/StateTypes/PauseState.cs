namespace States.StateTypes
{
    public class PauseState : IStates
    {
        private PlayerController player;
        public PauseState(PlayerController player)
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
