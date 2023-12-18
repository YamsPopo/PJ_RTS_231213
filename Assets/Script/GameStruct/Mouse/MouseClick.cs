using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;

public class MouseClick : MonoBehaviourPunCallbacks
{
    private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;
    private LayerMask layerBuilding;
    private Camera mainCamera;
    private RTSController controller;

    private void Awake()
    {
        mainCamera = Camera.main;
        controller = GetComponent< RTSController>();
        if(PhotonNetwork.IsMasterClient)
        {
            layerUnit = 1 << 6;
            layerBuilding = 1 << 12;
        }
        else
        {
            layerUnit = 1 << 7;
            layerBuilding = 1 << 13;
        }
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
                        SlotManager.Instance.SlotType = SLOTTYPE.ProductBuilding;
                    }
                    //�ؼ��� ����������
                    if (hit.collider.gameObject.TryGetComponent(out NexusBuilding nexusBuilding))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.NexusBuilding;
                    }
                }
            }
            else
            {
                controller.DeselctBuliding();
                SlotManager.Instance.SlotType = SLOTTYPE.None;
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
                        SlotManager.Instance.SlotType = SLOTTYPE.SupplyUnit;
                        SlotManager.Instance.selectedSupplyUnit = supplyUnit;
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
