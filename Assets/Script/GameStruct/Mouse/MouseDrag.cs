using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectangle; // ���콺 �巡���� ������ ����ȭ

    private Rect dragRect; // �巡�� ����
    private Vector2 dragStartPos = Vector2.zero;
    private Vector2 dragEndPos = Vector2.zero;

    private Camera mainCamera;
    private RTSController controller;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent<RTSController>();
        // start, end (0,0) �� ���·� �̹����� ũ�⸦ (0,0)���� �����Ͽ� ȭ�鿡 ������ �ʵ��� ��
        DrawDragRectangle();
    }

    private void Update()
    {
        Drag();    
    }

    public void Drag()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            dragRect = new Rect();
        }
        if (Input.GetMouseButton(0))
        {
            dragEndPos = Input.mousePosition;
            DrawDragRectangle();
        }
        if (Input.GetMouseButtonUp(0))
        {
            // ���콺 �巡�׸� ������ �� �巡�� ���� ���� �ִ� ���� ����
            CalculateDragRect();
            SelectUnits();

            UIManager.Instance.SelectUnitUIChange(controller);

            dragStartPos = dragEndPos = Vector2.zero;
            DrawDragRectangle();
        }
    }
    //public void SelectUnitUIChange()
    //{
    //    for (int i = 0; i < controller.selectedUnitList.Count; i++)
    //    { 
    //        UIManager.Instance.characterSlot.faceSlot[i].SetActive(true);
    //        UIManager.Instance.characterSlot.hpImage[i].fillAmount = (float)controller.selectedUnitList[i].unit.Hp / (float)100; // �ӽ÷� 100���� ������ ü�� ǥ��
    //        UIManager.Instance.characterSlot.faceImage[i].sprite = controller.selectedUnitList[i].unit.faceSprite;
    //    }
    //}

    private void DrawDragRectangle()
    {
        dragRectangle.position = (dragStartPos + dragEndPos) * 0.5f; // �巡�� ������ ��Ÿ���� �̹���UI ��ġ
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(dragStartPos.x - dragEndPos.x), Mathf.Abs(dragStartPos.y - dragEndPos.y)); // �巡�� �̹����� ũ��
    }

    private void CalculateDragRect()
    {
        if(Input.mousePosition.x < dragStartPos.x)
        {
            dragRect.xMin = Input.mousePosition.x;
            dragRect.xMax = dragStartPos.x;
        }
        else
        {
            dragRect.xMin = dragStartPos.x;
            dragRect.xMax = Input.mousePosition.x;
        }
        if (Input.mousePosition.y < dragStartPos.y)
        {
            dragRect.yMin = Input.mousePosition.y;
            dragRect.yMax = dragStartPos.y;
        }
        else
        {
            dragRect.yMin = dragStartPos.y;
            dragRect.yMax = Input.mousePosition.y;
        }
    }

    private void SelectUnits()
    {
        if(GameManager.Instance.rtsController.fieldUnitList == null)
            return;
        foreach(UnitController unit in GameManager.Instance.rtsController.fieldUnitList)
        {
            // ������ ���� ��ǥ�� ȭ�� ��ǥ�� ��ȯ�Ͽ� �巡�� ���� ���� �ִ��� �˻�
            if(dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                controller.DragSelectUnit(unit);
                //�������� Ŭ�� ������
                if (unit.TryGetComponent(out SupplyUnit supplyUnit))
                {
                    SlotManager.Instance.SlotType = SLOTTYPE.SupplyUnit;
                    SlotManager.Instance.selectedSupplyUnit = supplyUnit;
                }
                if (GameManager.Instance.rtsController.selectedUnitList.Count > 1)
                {
                    SlotManager.Instance.SlotType = SLOTTYPE.ArmySelect;
                }
            }
        }

        
    }


}
