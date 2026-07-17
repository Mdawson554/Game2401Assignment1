using System.Collections;
using System.Collections.Generic;
using Core;
using Interactions;
using UnityEngine;

public class InventoryUIManager : MonoBehaviour
{
   public static InventoryUIManager Instance;
   [SerializeField] private GameObject _inventoryCanvas;
   [SerializeField] private float waitTime = 0.5f;
   private Dictionary<CollectibleTypes, InventoryUIItem> _items = new Dictionary<CollectibleTypes, InventoryUIItem>();
   
   private void Awake()
   {
      if (Instance != null && Instance != this) Destroy(this);
      Instance = this;
   }
   
   public void CreateInventoryItem(InventoryUIItem inventoryUIItem, CollectibleTypes collectibleType)
   {
      var tempItem = Instantiate(inventoryUIItem, _inventoryCanvas.transform.position, Quaternion.identity);
      tempItem.transform.SetParent(_inventoryCanvas.transform);
      tempItem.ItemImage.sprite = inventoryUIItem.Icon;
      tempItem.Itemname.text = inventoryUIItem.name;
      tempItem.ItemCount = 0; 
      tempItem.ItemCountText.text = "0";
      _items.TryAdd(collectibleType, tempItem);
   }
   
   public void IncrementInventoryUIItem(InventoryUIItem inventoryUIItem, CollectibleTypes collectibleType)
   {
      var current = _items[collectibleType];
      current.ItemCount++;
      current.ItemCountText.text = current.ItemCount.ToString();
      StartCoroutine(HideInventoryUiElemement());
   }
   
   IEnumerator HideInventoryUiElemement()
   {
      _inventoryCanvas.SetActive(true);
      yield return new WaitForSeconds(waitTime);
      _inventoryCanvas.SetActive(false);
      yield return null;
   }
}
