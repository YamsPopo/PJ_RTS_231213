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

    
    [SerializeField] private float detectiveRange; // 감지 범위(시야보다 클 수 없음)


    public PriorityQueue<string, int> AdaptpriorityQueue;
    public IPrioxyQueue<string, int> priorityQueue;
    public Vector3 LastDetectivePos // 감지된 오브젝트 위치
    {  get; private set; }

    public float DetectiveRange { get => detectiveRange; set=> detectiveRange = value; }

    public bool IsRangeDetection { get { return isRangeDetection; } }

    private void Awake()
    {
        AdaptpriorityQueue = new PriorityQueue<string, int>();
        priorityQueue = AdaptpriorityQueue;
        PriorityQueueInit();
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
            transform.forward = dir;
            if(Physics.Raycast(transform.position,dir,out hit,detectiveRange))
            {
                LastDetectivePos = hit.transform.position;
            }
        }
    }
    public void PriorityQueueInit()
    {
        priorityQueue.Enqueue("영웅", 1);
        priorityQueue.Enqueue("원거리유닛", 2);
        priorityQueue.Enqueue("근거리유닛", 3);
        priorityQueue.Enqueue("건물", 4);
        priorityQueue.Dequeue();
    }


    public void AttackMethod()
    {
        if (cols.Length <= 0)
            return;
        if (cols[0].GetComponent<IHitAble>() != null)
        {
            for (int i = 0; i < cols.Length; i++)
            {
                //priorityQueue.Enqueue();
            }

            Debug.Log(this.gameObject.name + this.gameObject.GetComponent<IAttackAble>().Atk +"때렸다");
            cols[0].GetComponent<IHitAble>().Hp -= this.gameObject.GetComponent<IAttackAble>().Atk;
            Debug.Log(cols[0].name + cols[0].GetComponent<IHitAble>().Hp + "맞았다.");
        }
    }
    
    public void HealMethod()
    {
        if (cols[0] == null)
            return;

        if (cols[0].GetComponent<IHitAble>() != null)
        {
            cols[0].GetComponent<IHitAble>().Hp += this.gameObject.GetComponent<IAttackAble>().Atk;
            Debug.Log(cols[0].name + "회복");
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
