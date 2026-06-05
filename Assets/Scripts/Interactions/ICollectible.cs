namespace Interactions
{
    public interface ICollectible
    {
        void OnCollected();       // trigger collect animation / tween / despawn item / add item to inventory
        void OnCollectEffect();   // trigger particle effect
    }
}
