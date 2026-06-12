namespace States.StateTypes
{
    public class IdleState : IStates
    {
        private PlayerController player;
        public IdleState(PlayerController player)
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
