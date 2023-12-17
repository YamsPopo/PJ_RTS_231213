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

    public bool isRunMineCoroutine;
    public bool isMineClicked;
    public new void Awake()
    {
        base.Awake();
        MiningBTInit();
    }
    private void Start()
    {
    }

    public new void Update()
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
            if (isMineClicked && isRunMineCoroutine == false)
            {
                return BTNode.State.SUCCESS;
            }
            return BTNode.State.FAIL;
        };

        //�ѹ��� ������ ��ƾ�� ���������� �ݺ����������
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

            agent.SetDestination(mineTf);
            StartCoroutine(MiningCo());
            return BTNode.State.RUN;
        };

    }

    //public class WaitForClickedTarget : CustomYieldInstruction
    //{
    //    public SupplyUnit supplyUnit;
    //    public WaitForClickedTarget(SupplyUnit supplyUnit)
    //    {
    //        this.supplyUnit = supplyUnit;
    //    }
    //    public override bool keepWaiting
    //    {
    //        get
    //        {
    //            return !supplyUnit.isMineClicked;
    //        }
    //    }
    //}

    public IEnumerator MiningCo()
    {
        isRunMineCoroutine = true;

        float curCool = miningCool;

        //���� ��ƾ �����ؾ߉�
        while (true)
        {

            //���ҽ����� ����������
            if (Vector3.Distance(transform.position, mineTf) <= 3f)
            {
                agent.ResetPath();
                sm.SetState((int)UNIT_STATE.Work);
                curCool -= Time.deltaTime;
                if (curCool <= 0)
                {
                    agent.SetDestination(nexusTf);
                    sm.SetState((int)UNIT_STATE.Move);
                    curCool = miningCool;
                }
            }

            //�ؼ����� ����������
            if (Vector3.Distance(transform.position, nexusTf) <= 8f)
            {
                agent.ResetPath();
                agent.SetDestination(mineTf);
                GameManager.Instance.Tree += 10;
                //������
            }

            //���ϴ»��� Ż��~�� �̰�
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                sm.SetState((int)UNIT_STATE.Work);
                isRunMineCoroutine = false;
                break;
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

