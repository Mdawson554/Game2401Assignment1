namespace States.StateTypes
{
    public class WalkingState : IStates
    {
        private PlayerController player;
        public WalkingState(PlayerController player)
        {
            this.player = player;
        }
        
        public void EnterState()
        {
            player.ToggleMovement(true);
        }
        public void ExecuteState()
        {
            
        }
        public void ExitState()
        {
            
        }
    }
}
