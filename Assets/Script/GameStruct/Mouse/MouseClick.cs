using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MouseClick : MonoBehaviour
{
    [SerializeField] private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;
    [SerializeField] private LayerMask layerBuilding;
    private Camera mainCamera;
    private RTSController controller;
    public SlotManager behaviourSlotManager;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent< RTSController>();
    }

    private void Update()
    {
        if(!EventSystem.current.IsPointerOverGameObject())
        MouseControll();
    }

    void MouseControll()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);

            //���� Ŭ���Ҷ�
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBuilding))
            {
                if (hit.transform.TryGetComponent(out BuildingController buildingCon))
                {
                    controller.DeselctBuliding();
                    controller.SelectBuilding = buildingCon;
                    buildingCon.SelectBuliding();
                    //������� ����������
                    if (hit.collider.gameObject.TryGetComponent(out ProductBuilding productBuilding))
                    {
                        behaviourSlotManager.SlotType = SLOTTYPE.ProductBuilding;
                    }
                    //�ؼ��� ����������
                    if (hit.collider.gameObject.TryGetComponent(out NexusBuilding nexusBuilding))
                    {
                        behaviourSlotManager.SlotType = SLOTTYPE.NexusBuilding;
                    }
                }
            }
            else
            {
                controller.DeselctBuliding();
                behaviourSlotManager.SlotType = SLOTTYPE.None;
            }

            //���� Ŭ���ҋ�
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                if (hit.transform.GetComponent<UnitController>() == null) return;

                //����Ŭ��
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    controller.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
                }
                else
                {
                    controller.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
                    //�������� Ŭ�� ������
                    if(hit.collider.gameObject.TryGetComponent(out SupplyUnit supplyUnit))
                    {
                        behaviourSlotManager.SlotType = SLOTTYPE.SupplyUnit;
                    }
                }
            }
            else
            {
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    controller.DeselectAll();
                }
            }
        }
        //�̵�
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("Ŭ��");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, ~(1 << 8)))//FOV �������� �����ϱ�����
            {
                Debug.Log("���� ����");
                controller.MoveSelectedUnits(hit);
            }
        }

    }
}
