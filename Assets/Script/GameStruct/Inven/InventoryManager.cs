using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

// inventory �� inventoryManager�� ���� �־�� �ұ�?
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
        Debug.Log("ADD ������ ȣ���");
        if(GameManager.Instance.Gold < addItem.itemData.price)
        {
            Debug.Log("�����");
            UIManager.Instance.shopMessage.SetActive(true);
            UIManager.Instance.shopMessageText.text = addItem.itemData.name + "��(��) ���� �� ��尡 �����մϴ�.";
            return;
        }
        if (inven.curItemCount == inven.maxSlotCount)   //������ ������ ���� ������
        {
            for (int k = 0; k < inven.slots.Length; k++)//������ ���� ������ �����Ҷ�
            {
                if (inven.slots[k].item != null)
                {
                    if (inven.slots[k].item.Stackable && inven.slots[k].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                    {   //���� �������� �������
                        inven.slots[k].item.Count++;
                        inven.slots[k].item.RefreshCount();
                        GameManager.Instance.Gold -= addItem.itemData.price;
                        return;
                    }
                }
            }
            UIManager.Instance.shopMessage.SetActive(true);
            UIManager.Instance.shopMessageText.text = "�κ��丮 ������ �����մϴ�.";
            return;
        }
        GameManager.Instance.Gold -= addItem.itemData.price;
        if (inven.curItemCount == 0)                    //������ ���� �������� ������
        {
            inven.slots[0].item = Instantiate(item, inven.slots[0].transform).GetComponent<InventoryItem>();
            inven.curItemCount++;
            ItemApplication(addItem);
            return;
        }
        else                                            //������ ���� �������� ������
        {
            for (int k = 0; k < inven.slots.Length; k++)//������ ���� ������ �����Ҷ�
            {
                if (inven.slots[k].item != null)
                {
                    if (inven.slots[k].item.Stackable && inven.slots[k].item.ID == item.GetComponent<InventoryItem>().itemData.ID)
                    {   //���� �������� �������
                        inven.slots[k].item.Count++;
                        inven.slots[k].item.RefreshCount();
                        return;
                    }
                }
            }
            for (int s = 0; s < inven.slots.Length; s++)
            {           //���� �������� �������
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