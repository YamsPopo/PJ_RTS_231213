using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// inventory 와 inventoryManager가 따로 있어야 할까?
public class InventoryManager : SingleTon<InventoryManager>
{
    public Inventory inven;
    public InventoryItem waitForSelling = null;


    public void AddItem(GameObject item)
    {
        //for (int i = 0; i < inven.slots.Length; i++)
        //{
        //    if (inven.slots[i].item == null)
        //    {
        //        if (inven.curItemCount == inven.maxSlotCount) return;
        //        inven.slots[i].item = Instantiate(item, inven.slots[i].transform).GetComponent<InventoryItem>();
        //        inven.curItemCount++;
        //        break;
        //    }
        //    else
        //    {
        //        if (inven.slots[i].item.Stackable 
        //            && inven.slots[i].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
        //        {
        //                inven.slots[i].item.Count++;
        //                inven.slots[i].item.RefreshCount();

        //            break;
        //        }
        //    }
        //}
        Item addItem = item.GetComponent<Item>();
        Debug.Log("ADD 아이템 호출됨");
        if(GameManager.Instance.Gold < addItem.itemData.price)
        {
            Debug.Log("못산다");
            UIManager.Instance.shopMessage.SetActive(true);
            UIManager.Instance.shopMessageText.text = addItem.itemData.name + "을(를) 구매 할 골드가 부족합니다.";
            return;
        }
        if (inven.curItemCount == inven.maxSlotCount)   //아이템 슬룻이 꽉차 있을때
        {
            for (int k = 0; k < inven.slots.Length; k++)//아이템 슬룻에 스택이 가능할때
            {
                if (inven.slots[k].item != null)
                {
                    if (inven.slots[k].item.Stackable && inven.slots[k].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                    {   //같은 아이템이 있을경우
                        inven.slots[k].item.Count++;
                        inven.slots[k].item.RefreshCount();
                        GameManager.Instance.Gold -= addItem.itemData.price;
                        return;
                    }
                }
            }
            UIManager.Instance.shopMessage.SetActive(true);
            UIManager.Instance.shopMessageText.text = "인벤토리 공간이 부족합니다.";
            return;
        }
        GameManager.Instance.Gold -= addItem.itemData.price;
        if (inven.curItemCount == 0)                    //아이템 슬룻에 아이템이 없을때
        {
            inven.slots[0].item = Instantiate(item, inven.slots[0].transform).GetComponent<InventoryItem>();
            inven.curItemCount++;
            ItemApplication(addItem);
            return;
        }
        else                                            //아이템 슬룻에 아이템이 있을때
        {
            for (int k = 0; k < inven.slots.Length; k++)//아이템 슬룻에 스택이 가능할때
            {
                if (inven.slots[k].item != null)
                {
                    if (inven.slots[k].item.Stackable && inven.slots[k].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                    {   //같은 아이템이 있을경우
                        inven.slots[k].item.Count++;
                        inven.slots[k].item.RefreshCount();
                        return;
                    }
                }
            }
            for (int s = 0; s < inven.slots.Length; s++)
            {           //같은 아이템이 없을경우
                if (inven.slots[s].item == null)
                {
                    inven.slots[s].item = Instantiate(item, inven.slots[s].transform).GetComponent<InventoryItem>();
                    inven.curItemCount++;
                    ItemApplication(addItem);
                    return;
                }
            }        
        }
    }
    public void ItemApplication(Item item)
    {
        if(item.itemData.type == EItemType.EQUIPABLE)
        {
            switch (item.itemData.equipableItemType)
            {
                case EquipableItemType.BOTTOM:
                    GameManager.Instance.playerHero.info.Def += (int)item.itemData.value;
                    break;
                case EquipableItemType.TOP:
                    GameManager.Instance.playerHero.info.MaxHp += (int)item.itemData.value;
                    GameManager.Instance.playerHero.info.CurentHp += (int)item.itemData.value;
                    break;
                case EquipableItemType.SHOOSE:
                    GameManager.Instance.playerHero.MoveSpeed += (int)item.itemData.value;
                    break;
                case EquipableItemType.HAT:
                    GameManager.Instance.playerHero.info.SightRange += (int)item.itemData.value;
                    break;
                case EquipableItemType.HAND:
                    GameManager.Instance.playerHero.info.Atk += (int)item.itemData.value;
                    break;
                case EquipableItemType.EARRING:
                    GameManager.Instance.playerHero.MaxMp += (int)item.itemData.value;
                    GameManager.Instance.playerHero.info.CurentHp += (int)item.itemData.value;
                    break;
                case EquipableItemType.RING:
                    GameManager.Instance.playerHero.info.AtkSpeed += (int)item.itemData.value;
                    break;
                case EquipableItemType.NECKLE:
                    GameManager.Instance.playerHero.info.AtkRange += (int)item.itemData.value;
                    break;
            }
        }
    }



}