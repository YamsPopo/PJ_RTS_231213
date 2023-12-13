using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public enum BuildType
{
    None,
    Product,
    Defends,
    Population,
    Nexus
}

public class ButtonSlot : MonoBehaviour
{
    //������ƮǮ ������
    //�����Ǹ��߱�
    public GameObject targetObj;
    public Image imageCompo;//���� �̹��� 
    public Action slotAction;//���� ���
    public BuildType buildType;

    //�׷���
    public MeshRenderer meshRenderer;
    MaterialPropertyBlock mpb;
    public void BTN()//�������� ���� �׽�Ʈ, ������
    {
        if (slotAction != null)
            slotAction();
    }

    //�׷���
    private void SetMPB(string propertyName, Color color)
    {
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(propertyName, color);
        meshRenderer.SetPropertyBlock(mpb);
    }
    //
    private void Start()
    {
        //�׷���
        mpb = new MaterialPropertyBlock();
        //
    }
    private void Update()
    {
        BuildCheck();
    }

    void BuildCheck()
    {
        if (targetObj != null)
        {
            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out RaycastHit hitInfo, 150f, ~(1 << 13 | 1 << 8)))//������ ������ ���̾�
            {
                targetObj.transform.position = hitInfo.point;
                targetObj.GetComponent<NavMeshObstacle>().enabled = false;

                //�׷���
                SetMPB("_InstallColor", Color.green);
                //targetObj.GetComponent<MeshRenderer>().material.SetColor("_InstallColor", Color.green);

                targetObj.GetComponent<FieldOfView>().fov.GetComponent<MeshRenderer>().enabled = false;
                //������ ���������� Ŭ���ȵǰ�
                if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
                {
                    targetObj.GetComponent<NavMeshObstacle>().enabled = true;

                    //�׷���
                    SetMPB("_InstallColor", Color.black);

                    //targetObj.GetComponent<MeshRenderer>().material.SetColor("_InstallColor", Color.black);
                    targetObj.GetComponent<FieldOfView>().fov.GetComponent<MeshRenderer>().enabled = true;
                    targetObj = null;
                }

                //���� ��� ��Ŭ�� �̿�
                if(Input.GetMouseButtonDown(1))
                {
                    //��� ������Ʈ�� ���������
                    GameManager.Instance.buildingObjectPool.ReturnPool(targetObj);
                    targetObj = null;
                }
            }
        }
    }
}