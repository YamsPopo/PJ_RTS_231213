using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.PlayerLoop;

public class ShopController: MonoBehaviour
{
    //������ ����Ž��� ����Ͽ� �̵���ų �����Դϴ�.
    private NavMeshAgent shopAgent;
    public Transform[] shopMovePoint;
    [SerializeField]
    private const float leavingShopTime = 60f;
    private float curCoolTime;
    public float CurCoolTime
    {
        get { return curCoolTime; }
        set 
        {
            curCoolTime = value;
            UIManager.Instance.shopClosingTime.text = "�����ð� : " + curCoolTime;
        }
    }

    private int shopStopIndex; // ������ ������ ��ġ
    private int addValue; //��ȭ��
    [SerializeField]
    private int index; //ShopMovePoint�� �迭 ���Դϴ�.
    public int Index
    {
        get => index;
        set
        {
            index = value;
            if (index > shopMovePoint.Length - 2) //�迭�� ũ��� �������� -1�� ���Ͽ� index���� �����մϴ�.
                addValue = -1;
            if (index <= 0) //index�� 0�� �Ǹ� 1�� ���Ͽ� index���� �����մϴ�.
                addValue = 1;
        }
    }
    private bool shopStop; //������ Ư�� ������ ���缭 ������ �̿��Ҽ��ְ� �ϴ� �����Դϴ�.
    public bool ShopStop
    {
        get => shopStop;
        set
        {
            shopStop = value;
        }
    }
    private void Start()
    {
        Index = 0;
        addValue = 1;
        //������ �ʱ�ȭ �մϴ�.
        shopAgent = GetComponent<NavMeshAgent>();
        shopAgent.SetDestination(shopMovePoint[Index].position);
    }
    private void Update()
    {
        if (Vector3.Distance(transform.position, shopMovePoint[Index].position) <= 0.5f)
            ShopDirection();
    }

    public void ShopDirection()
    {
        if (Index == 0)
            shopStopIndex = Random.Range(0, shopMovePoint.Length);
        StartCoroutine(ShopStopCo(shopStopIndex));
    }
    IEnumerator ShopStopCo(int stopIndex)
    {
        Index += addValue;
        shopAgent.SetDestination(shopMovePoint[Index].position);
        Debug.Log(shopStopIndex);
        if (Index == stopIndex)
        {
            ShopStop = true;
            shopAgent.enabled = false;
            Debug.Log("�� ���Ⲩ��");
            CurCoolTime = leavingShopTime;
            while (CurCoolTime >= 0)
            {
                CurCoolTime -= Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }
            Debug.Log("�� �����ϲ���");
            ShopStop = false;
            shopAgent.enabled = true;
            shopAgent.SetDestination(shopMovePoint[Index].position);
        }
    }
    //private void OnTriggerEnter(Collider other)
    //{
    //    if(Index == 0)
    //        shopStopIndex = Random.Range(0, shopMovePoint.Length); // �ٽ� ���� �ö� ���� �������� ���� ��ġ�� ������
    //    if (other.gameObject.tag == "MovePoint")
    //        StartCoroutine(ShopStopCo(shopStopIndex));
    //}

}
