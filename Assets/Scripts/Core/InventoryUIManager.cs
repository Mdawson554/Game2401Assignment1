using System.Collections.Generic;
using Core;
using Interactions;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
   [SerializeField] private GameObject _inventoryCanvas;
   [SerializeField] private InventoryUIItem _inventoryUIPrefab;
   
   private Dictionary<InteractableObjects,InteractableObjectTypes> _interactableObjects = new Dictionary<InteractableObjects, InteractableObjectTypes>();
   
   public void CreateInventoryItem(ICollectible collectible)
   {
      var tempItem = Instantiate(_inventoryUIPrefab,this.transform.position,Quaternion.identity);
      tempItem.ItemImage.sprite = collectible.Icon;
      //_interactableObjects.Add(InteractableObjectTypes.Item,InteractableObjectTypes.Item);
   }
   
   private void UpdateInventoryUI(ICollectible collectible)
   {
      _inventoryCanvas.SetActive(true);
      
   }
}
