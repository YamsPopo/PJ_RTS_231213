using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public class SupplyUnit : Unit
{
    float resourseCool = 3f;
    public Vector3 resourseTf = Vector3.zero;//����, Ŭ���ɶ� �ڿ�Ŭ���Ǹ� rts��Ʈ�ѷ��ؼ� ���͸� �־���
    public Vector3 nexusTf = Vector3.zero;//��


    private bool isResourseClicked;
    public bool IsResourseClicked
    {
        get { return isResourseClicked; }
        set { isResourseClicked = value; }
    }
    public new void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        StartCoroutine(ResourseCo());
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
                return !supplyUnit.IsResourseClicked;
            }
        }
    }

    public IEnumerator ResourseCo()
    {
        float curCool = resourseCool;

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
                        curCool = resourseCool;
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

