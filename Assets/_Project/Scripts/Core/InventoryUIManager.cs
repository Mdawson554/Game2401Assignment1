using System.Collections;
using System.Collections.Generic;
using Interactions;
using UnityEngine;

namespace Core
{
   public class InventoryUIManager :  Singleton<InventoryUIManager>
   {
      [SerializeField] private GameObject inventoryCanvas;
      [SerializeField] private float waitTime = 0.5f;
      private Dictionary<CollectibleTypes, InventoryUIItem> _items = new Dictionary<CollectibleTypes, InventoryUIItem>();
   
      public void CreateInventoryItem(InventoryUIItem inventoryUIItem, CollectibleTypes collectibleType)
      {
         var tempItem = Instantiate(inventoryUIItem, inventoryCanvas.transform.position, Quaternion.identity);
         tempItem.transform.SetParent(inventoryCanvas.transform);
         tempItem.ItemImage.sprite = inventoryUIItem.Icon;
         tempItem.Itemname.text = inventoryUIItem.name;  
         tempItem.ItemCount = 0; 
         tempItem.ItemCountText.text = "0";
         _items.TryAdd(collectibleType, tempItem);
      }

      public void IncrementInventoryUIItem(InventoryUIItem inventoryUIItem, CollectibleTypes collectibleType, BaseItem baseItem)
      {
         var current = _items[collectibleType];
         current.ItemCount++;
         current.Itemname.text = baseItem.GetItemName();  
         current.ItemCountText.text = current.ItemCount.ToString();
         StartCoroutine(HideInventoryUiElement());
      }
   
      IEnumerator HideInventoryUiElement()
      {
         inventoryCanvas.SetActive(true);
         yield return new WaitForSeconds(waitTime);
         inventoryCanvas.SetActive(false);
         yield return null;
      }
   }
}
