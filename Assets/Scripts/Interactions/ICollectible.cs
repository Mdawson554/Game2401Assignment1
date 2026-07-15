using UnityEngine;

namespace Interactions
{
    public interface ICollectible
    {
        public Sprite Icon { get; set; }
        void OnCollectEffect();   
    }
    
    public enum CollectibleTypes
    {
        Clues,
        Keys,
    }
}
