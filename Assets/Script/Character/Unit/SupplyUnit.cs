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
    SelectorNode btRootNode;
    ////////
    float miningCool = 3f;
    public Vector3 resourseTf = Vector3.zero;//����, Ŭ���ɶ� �ڿ�Ŭ���Ǹ� rts��Ʈ�ѷ��ؼ� ���͸� �־���
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
        btRootNode.Evaluate();
    }

    //ä����ƾ �ൿƮ��
    public void MiningBTInit()
    {
        btRootNode = new SelectorNode();

        SequenceNode miningNode = new SequenceNode();
        ActionNode clickedCheck = new ActionNode();
        ActionNode miningAction = new ActionNode();
        miningNode.Add(clickedCheck);

        clickedCheck.action = () =>
        {
            if (IsMineClicked)
            {
                return BTNode.State.SUCCESS;
            }
            return BTNode.State.FAIL;
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

            Collider[] cols = Physics.OverlapSphere(resourseTf, 100f);
            //�ֺ� �ؼ��� ã��, ������ �а� �����
            foreach (var targetBuilding in cols)
            {
                if (targetBuilding.TryGetComponent(out NexusBuilding nexusBuilding))
                {
                    nexusTf = nexusBuilding.transform.position;
                }
            }

            agent.SetDestination(resourseTf);

            while (true)
            {
                Debug.LogWarning("�ڷ�ƾ ù while ����");
                //���ҽ����� ����������
                if (Vector3.Distance(transform.position, resourseTf) <= 5f)
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
                    agent.SetDestination(resourseTf);
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

