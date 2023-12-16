using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering.Universal;
using static UnityEngine.UI.CanvasScaler;

public enum PLAY_MODE
{ RTS_MODE, AOS_MODE}


public class GameManager : SingleTon<GameManager>
{
    public PLAY_MODE playMode = PLAY_MODE.RTS_MODE;
    public Transform[] points= new Transform[2];
    public Transform buildSpawnPoint;
    public int index;

    public PhotonView pv;

    //������
    public event Action onRoundStart;
    public event Action onRoundEnd;

    //�ڷᱸ��
    public ObjectPool unitObjectPool;
    public ObjectPool buildingObjectPool;
    PriorityQueue<string, int> adapterPriorityQueue;
    IPrioxyQueue<string, int> priorityQueue;//�켱���� ť�� ��ɸ� ��밡���� ��¥ ť
    StateMachine<GameManager> stateMachine;

    //�׷���
    //public UniversalRendererData urData; //��� ����

    public RTSController rtsController;
    private int tree;
    public int Tree
    {
        get { return tree; }
        set { tree = value; }
    }
    private int gold;
    public int Gold
    {
        get { return gold; }
        set { gold = value; }
    }
    private int population;

    public int Population
    {
        get { return population; }
        set { population = value; }
    }

    //Player ����
    Hero playerHero;
    public Hero PlayerHero
    {
        get { return playerHero; } set { playerHero = value; }
    }
    protected override void Awake()
    {
        base.Awake();

        pv = GetComponent<PhotonView>();
        //Player ã��
        //--- ��� ������ �־� ���δ�. ���̶� ���� �±׿� ���� ��ũ��Ʈ���ٵ� �̷��� ã�°� ������?
        playerHero = GameObject.FindGameObjectWithTag("PlayerHero").GetComponent<Hero>();
        playMode = PLAY_MODE.RTS_MODE;
        //ĳ���� ���� ������ �迭�� ����
        index = UnityEngine.Random.Range(0, points.Length);
        //ĳ���� ����
        PhotonNetwork.Instantiate(DropDownManager.selectHeroName, points[index].position, points[index].rotation, 0);
     //   if(pv.ViewID == 1)
        PhotonNetwork.Instantiate("Nexus", buildSpawnPoint.position, buildSpawnPoint.rotation, 0);
     //   else
     //       PhotonNetwork.Instantiate("Nexus", buildSpawnPoint.position, buildSpawnPoint.rotation, 0);
    }
    void Start()
    {
        StartCoroutine(PlayerInitCo());

        //�켱ť
        adapterPriorityQueue = new PriorityQueue<string, int>();
        priorityQueue = adapterPriorityQueue;

        priorityQueue.Enqueue("�ú�����", 1);
        priorityQueue.Enqueue("��������", 2);
        priorityQueue.Enqueue("����", 3);
        priorityQueue.Enqueue("�ǹ�", 4);

        for (int i = 0; i < 4; i++)
        {
            //Debug.Log(priorityQueue.Dequeue());
        }

        //FSM �����
        stateMachine = new StateMachine<GameManager>(this);
    }

    // Update is called once per frame
    void Update()
    {
        //���ӸŴ��� ���¸ӽ� �Ⱦ��Ű���
        //stateMachine.UpdateState();
    }

    //���ӽ����ϰ� �÷��̾� ���� �ʱ�ȭ
    IEnumerator PlayerInitCo()
    {
        //�÷��̾� ó�� ���� ���ְ� �ؼ��� ��ȯ
        GameObject nexus =  buildingObjectPool.Pop(0);
        nexus.transform.position = new Vector3(20, 0, 480);//���� �� ���� �ؼ��� ������ġ
        for (int i = 0; i < 5; i++)
        {
            GameObject supply = unitObjectPool.Pop(6);
            rtsController.fieldUnitList.Add(supply.GetComponent<UnitController>());
            supply.transform.position = new Vector3(20, 0, 470);
            supply.GetComponent<NavMeshAgent>().enabled = true;

            //������ ��������ϰ� �ٷ������ؾ� ��� �������� �Ȱ�ħ
            supply.GetComponent<NavMeshAgent>().SetDestination(supply.transform.position + new Vector3(0,0,-2));
            yield return null;
            supply.GetComponent<NavMeshAgent>().ResetPath();
        }
    }
}
