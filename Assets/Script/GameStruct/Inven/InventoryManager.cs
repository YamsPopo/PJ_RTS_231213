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
        else
        {
            GameManager.Instance.Gold -= addItem.itemData.price;
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
                        return;
                    }
                }
            }
            UIManager.Instance.shopMessage.SetActive(true);
            UIManager.Instance.shopMessageText.text = "�κ��丮 ������ �����մϴ�.";
            return;
        }
        if (inven.curItemCount == 0)                    //������ ���� �������� ������
        {
            inven.slots[0].item = Instantiate(item, inven.slots[0].transform).GetComponent<InventoryItem>();
            inven.curItemCount++;
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
                    break;
                case EquipableItemType.TOP:
                    break;
                case EquipableItemType.SHOOSE:
                    break;
                case EquipableItemType.HAT:
                    break;
                case EquipableItemType.HAND:
                    break;
                case EquipableItemType.EARRING:
                    break;
                case EquipableItemType.RING:
                    break;
                case EquipableItemType.NECKLE:
                    break;
            }
        }
    }



}