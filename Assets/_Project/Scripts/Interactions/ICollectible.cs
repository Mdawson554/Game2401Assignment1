using UnityEngine;

namespace _Project.Scripts.Interactions
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
