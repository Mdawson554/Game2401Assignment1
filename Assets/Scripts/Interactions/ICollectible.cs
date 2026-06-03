namespace Interactions
{
    public interface ICollectible
    {
        void OnCollected();       // trigger collect animation / tween
        void OnCollectEffect();   // trigger particle effect
    }
}
