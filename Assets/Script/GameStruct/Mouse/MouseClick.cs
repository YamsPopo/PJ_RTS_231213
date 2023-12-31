using Photon.Pun.Demo.PunBasics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Photon.Pun;
using Unity.VisualScripting;

public class MouseClick : MonoBehaviourPunCallbacks
{

    public GameObject clickEffect;

    GameObject effect;

    private LayerMask layerUnit;
    [SerializeField] private LayerMask layerGround;
    private LayerMask layerBuilding;
    private LayerMask layerHero;
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
            layerHero = 1 << 17;
        }
        else
        {
            layerUnit = 1 << 7;
            layerBuilding = 1 << 13;
            layerHero = 1 << 18;
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

            //빌딩 클릭할때
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerBuilding))
            {
                controller.DeselectAll();
                if (hit.transform.TryGetComponent(out BuildingController buildingCon))
                {
                    controller.DeselctBuliding();
                    controller.SelectBuilding = buildingCon;
                    buildingCon.SelectBuliding();
                    //생산빌딩 선택했으면
                    if (hit.collider.gameObject.TryGetComponent(out ProductBuilding productBuilding))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.ProductBuilding;
                    }
                    //넥서스 선택했으면
                    if (hit.collider.gameObject.TryGetComponent(out NexusBuilding nexusBuilding))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.NexusBuilding;
                    }
                }
                return;
            }
            else
            {
                controller.DeselctBuliding();
                SlotManager.Instance.SlotType = SLOTTYPE.None;
            }

            //유닛 클릭할떄
            if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerUnit))
            {
                if (hit.transform.GetComponent<UnitController>() == null) return;

                //다중클릭
                if (Input.GetKey(KeyCode.LeftShift))
                {
                    controller.ShiftClickSelectUnit(hit.transform.GetComponent<UnitController>());
                }
                else
                {
                    controller.ClickSelectUnit(hit.transform.GetComponent<UnitController>());
                    //생산유닛 클릭 했을때
                    if(hit.collider.gameObject.TryGetComponent(out SupplyUnit supplyUnit))
                    {
                        SlotManager.Instance.SlotType = SLOTTYPE.SupplyUnit;
                        SlotManager.Instance.selectedSupplyUnit = supplyUnit;
                    }
                }
                return;
            }
            else
            {
                controller.DeselctBuliding();
                SlotManager.Instance.SlotType = SLOTTYPE.None;
                if (!Input.GetKey(KeyCode.LeftShift))
                {
                    controller.DeselectAll();
                }
            }
        }
        //이동
        if (Input.GetMouseButtonDown(1))
        {
            RaycastHit hit;
            Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
            Debug.Log("클릭");
            if (Physics.Raycast(ray, out hit, Mathf.Infinity,  ~(1 << 8)))//FOV 안찍히게 전달하기위함
            {
                //effect = Instantiate(clickEffect, transform);
                //effect.transform.position = hit.point;
                //StartCoroutine(GameManager.Instance.WaitForEffectCo(effect));

                Debug.Log("땅을 찍음");
                controller.MoveSelectedUnits(hit);
            }
        }
    }
}
