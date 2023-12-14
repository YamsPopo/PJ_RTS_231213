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
    float miningCool = 3f;
    public Vector3 mineTf = Vector3.zero;//����, Ŭ���ɶ� �ڿ�Ŭ���Ǹ� rts��Ʈ�ѷ��ؼ� ���͸� �־���
    public Vector3 nexusTf = Vector3.zero;//��


    private bool isMineClicked;
    public bool IsMineClicked
    {
        get { return isMineClicked; }
        set { isMineClicked = value; }
    }
    public new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(miningCo());
    }

    public new void Update()
    {
        base.Update();
        //btRootNode.Evaluate();
    }

    //ä����ƾ �ൿƮ��
    public void MiningBTInit()
    {
        btRootNode = new SequenceNode();

        ActionNode miningReadyCheck = new ActionNode();
        ActionNode miningAction = new ActionNode();
        ActionNode goNexus = new ActionNode();
        btRootNode.Add(miningReadyCheck);
        btRootNode.Add(miningAction);
        
        //�׼� ������
        miningReadyCheck.action = () =>
        {
            if (IsMineClicked)
            {
                return BTNode.State.SUCCESS;
            }
            return BTNode.State.FAIL;
        };

        miningAction.action = () =>
        {
            float curCool = miningCool;

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

            while (curCool > 0)
            {
                curCool -= Time.fixedDeltaTime;
            }


            return BTNode.State.SUCCESS;
        };

        goNexus.action = () =>
        {
            agent.SetDestination(nexusTf);

            while (agent.remainingDistance < 5f)
            {

            }

            return BTNode.State.SUCCESS;
        };
    }

    public class WaitForClickedTarget : CustomYieldInstruction
    {
        public SupplyUnit supplyUnit;
        public WaitForClickedTarget(SupplyUnit supplyUnit)
        {
            this.supplyUnit = supplyUnit;
        }
        public override bool keepWaiting
        {
            get
            {
                return !supplyUnit.IsMineClicked;
            }
        }
    }

    public IEnumerator miningCo()
    {
        float curCool = miningCool;

        while (true)
        {
            yield return null;
            //�ڿ��ű�鼭 ��ȯ�ϴ� �ڵ�
            
            //Ŭ���ɶ� �Һ����� �ٲ�� ���� ���ִ� ����
            yield return new WaitForClickedTarget(this);

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

            while (true)
            {
                Debug.LogWarning("�ڷ�ƾ ù while ����");
                //���ҽ����� ����������
                if (Vector3.Distance(transform.position, mineTf) <= 5f)
                {
                    Debug.LogWarning(nexusTf);
                    curCool -= Time.fixedDeltaTime;
                    if (curCool <= 0)
                    {
                        agent.SetDestination(nexusTf);
                        curCool = miningCool;
                    }
                }

                //�ؼ����� ����������
                if (Vector3.Distance(transform.position, nexusTf) <= 8f)
                {
                    agent.SetDestination(mineTf);
                    GameManager.Instance.Tree += 10;
                    //������
                }
                yield return null;
            }

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

