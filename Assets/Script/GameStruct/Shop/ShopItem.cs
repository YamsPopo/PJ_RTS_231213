using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShopItem : Item, IPointerClickHandler
{
    public GameObject dragableItem = null;
    private void Start()
    {
        initItemData();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.clickCount != 2) return;

        Debug.Log("����Ŭ��");
        InventoryManager.Instance.AddItem(dragableItem);
    }
}
