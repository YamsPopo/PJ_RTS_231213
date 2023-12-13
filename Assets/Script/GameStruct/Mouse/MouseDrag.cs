using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseDrag : MonoBehaviour
{
    [SerializeField] private RectTransform dragRectangle; // 마우스 드래그한 범위를 가시화

    private Rect dragRect; // 드래그 범위
    private Vector2 dragStartPos = Vector2.zero;
    private Vector2 dragEndPos = Vector2.zero;

    private Camera mainCamera;
    private RTSController controller;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent<RTSController>();
        // start, end (0,0) 인 상태로 이미지의 크기를 (0,0)으로 설정하여 화면에 보이지 않도록 함
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
            // 마우스 드래그를 종료할 때 드래그 범위 내에 있는 유닛 선택
            CalculateDragRect();
            SelectUnits();

            dragStartPos = dragEndPos = Vector2.zero;
            DrawDragRectangle();
        }
    }
    private void DrawDragRectangle()
    {
        dragRectangle.position = (dragStartPos + dragEndPos) * 0.5f; // 드래그 범위를 나타내는 이미지UI 위치
        dragRectangle.sizeDelta = new Vector2(Mathf.Abs(dragStartPos.x - dragEndPos.x), Mathf.Abs(dragStartPos.y - dragEndPos.y)); // 드래그 이미지의 크기
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
            // 유닛의 월드 좌표를 화면 좌표로 변환하여 드래그 범위 내에 있는지 검사
            if(dragRect.Contains(mainCamera.WorldToScreenPoint(unit.transform.position)))
            {
                controller.DragSelectUnit(unit);
                SlotManager.Instance.SlotType = SLOTTYPE.ArmySelect;
            }
        }

        
    }


}
