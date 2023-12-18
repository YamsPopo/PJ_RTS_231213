using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class DetectiveComponent : MonoBehaviourPunCallbacks
{
    public LayerMask targetLayer;
    [SerializeField] bool isRangeDetection;
    PhotonView pv;

    public bool isCutomTargetLayer;
    public Collider[] cols;

    
    [SerializeField] private float detectiveRange; // ���� ����(�þߺ��� Ŭ �� ����)

    public Vector3 LastDetectivePos // ������ ������Ʈ ��ġ
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        pv = GetComponent<PhotonView>();
        if(isCutomTargetLayer)
        {
            targetLayer = (1 << 17) | (1 << 18);
        }
        else
        {
            pv.RPC("DetectLayer", RpcTarget.AllBuffered);
        }
    
    }


    private void Update()
    {
        cols = Physics.OverlapSphere(transform.position, detectiveRange, targetLayer);
        isRangeDetection = (bool)(cols.Length > 0);

        if(isRangeDetection)
        {
            RaycastHit hit;
            int index = 0;           
            while (cols[index] == null)
            {
                index++;
                continue;
            }    

            Vector3 dir = ((cols[index].transform.position) - transform.position).normalized;
            
            if(Physics.Raycast(transform.position,dir,out hit,detectiveRange))
            {
                LastDetectivePos = hit.transform.position;
            }
        }
    }

    public void AttackMethod()
    {
        if (cols[0].GetComponent<IHitAble>() != null)
        {
            Debug.Log(this.gameObject.name + this.gameObject.GetComponent<IAttackAble>().Atk +"���ȴ�");
            cols[0].GetComponent<IHitAble>().Hp -= this.gameObject.GetComponent<IAttackAble>().Atk;
            Debug.Log(cols[0].name + cols[0].GetComponent<IHitAble>().Hp + "�¾Ҵ�.");
        }
    }

    [PunRPC]
    public void DetectLayer()
    {
        if (this.gameObject.layer == 6)
        {
            targetLayer = (1 << 7) | (1 << 13) | (1 << 18);
        }
        else
            targetLayer = (1 << 6) | (1 << 12) | (1 << 17);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectiveRange);
    }

}
