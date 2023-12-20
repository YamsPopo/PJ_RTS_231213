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

public enum Tribe
{ HUMAN, ORC}


public class GameManager : SingleTon<GameManager>
{
    public PLAY_MODE playMode = PLAY_MODE.RTS_MODE;
    public Transform[] heroPoints= new Transform[2];
    public Transform[] buildPoints = new Transform[2];
    public Tribe tribe;

    public PhotonView pv;

    //������
    public event Action onRoundStart;
    public event Action onRoundEnd;

    //�ڷᱸ��
    public ObjectPool unitObjectPool;
    public ObjectPool buildingObjectPool;
    public ObjectPool monsterObjectPool;

    public GameManager(ObjectPool buildingOjbectPool)
    {
        this.buildingObjectPool = buildingOjbectPool;
    }
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
        if (DropDownManager.selectTribe == "Human")
            tribe = Tribe.HUMAN;
        else if (DropDownManager.selectTribe == "Orc")
            tribe = Tribe.ORC;

        pv = GetComponent<PhotonView>();
        //Player ã��
        //--- ��� ������ �־� ���δ�. ���̶� ���� �±׿� ���� ��ũ��Ʈ���ٵ� �̷��� ã�°� ������?
        playerHero = GameObject.FindGameObjectWithTag("PlayerHero").GetComponent<Hero>();
        playMode = PLAY_MODE.RTS_MODE;
        //ĳ���� ���� ������ �迭�� ����
        //ĳ����, �ؼ��� ���� ����Ʈ�� ����

        
    }
    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.Instantiate(DropDownManager.selectHeroName, heroPoints[MatchManager.masterIndexPoint].position, heroPoints[MatchManager.masterIndexPoint].rotation, 0);
            GameObject firstNexus = this.buildingObjectPool.Pop();
            firstNexus.transform.position = buildPoints[MatchManager.masterIndexPoint].position;

            //PhotonNetwork.Instantiate("Nexus", buildPoints[MatchManager.masterIndexPoint].position, buildPoints[MatchManager.masterIndexPoint].rotation, 0);
        }
        else
        {
            PhotonNetwork.Instantiate(DropDownManager.selectHeroName, heroPoints[MatchManager.userIndexPoint].position, heroPoints[MatchManager.userIndexPoint].rotation, 0);
            GameObject firstNexus = this.buildingObjectPool.Pop();
            firstNexus.transform.position = buildPoints[MatchManager.userIndexPoint].position;
            //PhotonNetwork.Instantiate("Nexus", buildPoints[MatchManager.userIndexPoint].position, buildPoints[MatchManager.userIndexPoint].rotation, 0);
        }
        //StartCoroutine(PlayerInitCo());


        for (int i = 0; i < 4; i++)
        {
            //Debug.Log(priorityQueue.Dequeue());
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PhotonNetwork.CurrentRoom.IsOpen = true;
    }

    // Update is called once per frame
    void Update()
    {
        //���ӸŴ��� ���¸ӽ� �Ⱦ��Ű���
        //stateMachine.UpdateState();
    }

    //���ӽ����ϰ� �÷��̾� ���� �ʱ�ȭ
    /*IEnumerator PlayerInitCo()
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
    }*/
}
