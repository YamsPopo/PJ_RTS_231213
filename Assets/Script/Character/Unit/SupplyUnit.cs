using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SupplyUnit : Unit
{
    //�ൿƮ��
    SequenceNode btRootNode;
    ////////
    float miningCool = 5f;
    public Vector3 mineTf = Vector3.zero;//����, Ŭ���ɶ� �ڿ�Ŭ���Ǹ� rts��Ʈ�ѷ��ؼ� ���͸� �־���
    public Vector3 nexusTf = Vector3.zero;//��

    bool isMineEnd = false;
    public bool isRunMineCoroutine;
    public bool isMineClicked;
    public IEnumerator mineCo;
    public new void Awake()
    {
        base.Awake();
        MiningBTInit();
        mineCo = MiningCo();
    }
    private void Start()
    {
    }

    public override void Update()
    {
        base.Update();
        btRootNode.Evaluate();
    }

    //ä����ƾ �ൿƮ��
    public void MiningBTInit()
    {
        btRootNode = new SequenceNode();

        ActionNode miningReadyCheckAction = new ActionNode();
        ActionNode miningAction = new ActionNode();
        btRootNode.Add(miningReadyCheckAction);
        btRootNode.Add(miningAction);
        
        //�׼� ������
        miningReadyCheckAction.action = () =>
        {
            if (isMineClicked)
            {
                return BTNode.State.SUCCESS;
            }
            return BTNode.State.FAIL;
        };

        miningAction.action = () =>
        {
            isMineClicked = false;
            Collider[] cols = Physics.OverlapSphere(mineTf, 100f);
            //�ֺ� �ؼ��� ã��, ������ �а� �����
            foreach (var targetBuilding in cols)
            {
                if (targetBuilding.TryGetComponent(out NexusBuilding nexusBuilding))
                {
                    nexusTf = nexusBuilding.transform.position;
                }
            }
            isMineEnd = false;
            agent.SetDestination(mineTf);
            StartCoroutine(mineCo);
            return BTNode.State.RUN;
        };
    }

    public IEnumerator MiningCo()
    {
        float curCool = miningCool;
        float checkDis = 7f;
        int mindGold = 10;
        while (isMineClicked == false)
        {
            //���ҽ����� ����������
            if (Vector3.Distance(transform.position, mineTf) <= checkDis && isMineEnd == false)
            {
                agent.ResetPath();
                sm.SetState((int)UNIT_STATE.Work);
                curCool -= Time.deltaTime;
                if (curCool <= 0)
                {
                    agent.SetDestination(nexusTf);
                    isMineEnd = true;
                    curCool = miningCool;
                }
                yield return null;
            }

            //�ؼ����� ����������
            if (Vector3.Distance(transform.position, nexusTf) <= checkDis && isMineEnd == true)
            {
                agent.ResetPath();
                agent.SetDestination(mineTf);
                GameManager.Instance.Mine += mindGold;
                isMineEnd = false;
            }
            yield return null;
        }
    }

    public override void InitStats()
    {
        throw new NotImplementedException();
    }

    public override void Attack(IHitAble target)
    {
        throw new NotImplementedException();
    }

    public override void Hit(IAttackAble attacker)
    {
        throw new NotImplementedException();
    }

    public override void Die()
    {
        throw new NotImplementedException();
    }
}

