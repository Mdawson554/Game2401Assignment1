namespace Interactions
{
    public interface IConditional
    {
        bool CanInteract();
        void OnKeyNotPickedUp();
    }
}