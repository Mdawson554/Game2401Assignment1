using UnityEngine;

namespace Interactions
{
    public interface ICollectible
    {
        public Sprite Icon { get; set; }
        void OnCollectEffect();   // trigger particle effect
    }
}
