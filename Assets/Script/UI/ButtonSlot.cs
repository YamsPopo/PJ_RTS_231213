using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Playables;
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

    public SlotManager slotManager;

    public bool isBuildClicked; 

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
        slotManager = GetComponentInParent<SlotManager>();
    }
    private void Update()
    {
        BuildCheck();
    }

    void BuildCheck()
    {
        if (isBuildClicked)
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

                    //targetObj.GetComponent<MeshRenderer>().material.SetColor("_InstallColor", Color.black);
                    targetObj.GetComponent<FieldOfView>().fov.GetComponent<MeshRenderer>().enabled = true;
                    isBuildClicked = false;
                    StartCoroutine(BuildProgressCo());
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

    //���� ���� ������� �ڷ�ƾ
    IEnumerator BuildProgressCo()
    {
        float coolTime = 6f;

        targetObj.gameObject.SetActive(false);

        GameObject temp = Instantiate(slotManager.buildingProgressprefab,targetObj.transform.position,targetObj.transform.rotation);
        TimelineController tc = temp.GetComponent<TimelineController>();
        TCPlay(tc, 1);
        yield return new WaitForSeconds(coolTime / 3);

        //while(dsad)
        //{
        //    yield return new WaitForEndOfFrame();
        //}

        //�ϲ��� �����ϸ� �����ܰ�
        TCPlay(tc, 2);
        yield return new WaitForSeconds(coolTime / 3);
        TCPlay(tc, 3);
        yield return new WaitForSeconds(coolTime / 3);

        Destroy(temp.gameObject);
        Instantiate(slotManager.buildingEFTprefab,targetObj.transform);
        //�׷���
        SetMPB("_InstallColor", Color.black);
        targetObj.gameObject.SetActive(true);
        targetObj = null;
    }

    void TCPlay(TimelineController tc, int boolIndex)
    {
        tc.track[0] = tc.timeline.GetRootTrack(0);
        tc.track[1] = tc.timeline.GetRootTrack(1);
        tc.track[2] = tc.timeline.GetRootTrack(2);
        switch (boolIndex)
        {
            case 1:
                tc.track[0].muted = false;
                tc.track[1].muted = true;
                tc.track[2].muted = true;
                tc.playable.Play();
                break;
            case 2:
                tc.track[0].muted = true;
                tc.track[1].muted = false;
                tc.track[2].muted = true;
                tc.playable.Play();
                break;
            case 3:
                tc.track[0].muted = true;
                tc.track[1].muted = true;
                tc.track[2].muted = false;
                tc.playable.Play();
                break;

            default:
                break;
        }
    }
}