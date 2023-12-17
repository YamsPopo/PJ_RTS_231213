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
        if (inven.curItemCount == inven.maxSlotCount)   //������ ������ ���� ������
            return;
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



}