using System.Collections.Generic;
using Core;
using Interactions;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
   public static InventoryUIManager Instance;
   
   [SerializeField] private GameObject _inventoryCanvas;
   [SerializeField] private InventoryUIItem _inventoryUIPrefab;
   
   private void Awake()
   {
      if (Instance != null && Instance != this) Destroy(this);
      Instance = this;

   }
   
   public void CreateInventoryItem(InventoryUIItem inventoryUIItem)
   {
      var tempItem = Instantiate(_inventoryUIPrefab,this.transform.position,Quaternion.identity);
      tempItem.ItemImage.sprite = inventoryUIItem.Icon;
   }
   
   private void UpdateInventoryUI(ICollectible collectible)
   {
      _inventoryCanvas.SetActive(true);
      
   }
}
