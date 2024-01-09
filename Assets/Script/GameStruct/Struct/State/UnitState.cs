using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using Photon.Pun;

public class UnitIdleState : UnitState //������ �⺻ ����
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity != Vector3.zero) //������ velocity�� 0�̾ƴ� �� �����δٸ�
            sm.SetState((int)UNIT_STATE.Move); //Move ���·� ����
        if (owner.DetectiveComponent.IsRangeDetection) //������ ���� Ž���ߴٸ� (������ �Ʊ�)
        {
            if(owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                //������ ��� ���� Hp������ �� ������ Ž���� ��� ����(��)���·� ����
                for(int i = 0; i< owner.DetectiveComponent.targetCols.Length; i++)
                {
                    if (owner.DetectiveComponent.targetCols[i].gameObject.GetComponent<Character>().Hp < 50)
                        sm.SetState((int)UNIT_STATE.Attack);
                }
            }
            else
                sm.SetState((int)UNIT_STATE.Attack); // ������ ���ֵ��� ���ݻ���
        }
        if (owner.Hp <= 0) //������ Hp�� 0�� �ȴٸ� ���� ����
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitMoveState : UnitState //������ �����̴� ����
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity == Vector3.zero) //������ �����̰� ���� �ʴٸ�
            sm.SetState((int)UNIT_STATE.Idle); // �⺻���·� ��ȯ
        if (owner.DetectiveComponent.IsRangeDetection) //������ ���� Ž���ߴٸ� (������ �Ʊ�)
        {
            //���� ������ ��� �⺻ ���¿����� ���� �����ϰ� �� ����
            if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer)
            {
                if (owner.agent.velocity == Vector3.zero) //�������� ���ٸ�
                    sm.SetState((int)UNIT_STATE.Idle); //�⺻ ���·� ��ȯ
                else
                    sm.SetState((int)UNIT_STATE.Move); // �ƴ϶�� ��� �����̴� ����
            }
            else //���� ������ �ƴ� �ٸ������� ���
                sm.SetState((int)UNIT_STATE.Attack); // ���� ���·� ��ȯ
        }
        if (owner.Hp <= 0) // ������ Hp�� 0�� �ȴٸ� �״� ���·� ��ȯ
            sm.SetState((int)UNIT_STATE.Die);
    }
}
public class UnitAttackState : UnitState // ������ ���� ����
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.gameObject.GetComponent<BattleUnit>()?.unitType == BATTLE_UNIT.Healer) //���� ������ ���
        {
            if (owner.DetectiveComponent.IsRangeDetection == false) //Ž���� ������ ���ٸ�
                sm.SetState((int)UNIT_STATE.Idle); // �⺻ ���� ��ȯ
            else
            {
                int unitCount = 0; //���� ��
                for (int i = 0; i < owner.DetectiveComponent.targetCols.Length; i++)
                {   //Ž���� ���ֵ��� Hp�� 80�� �Ѵ��� �˻��Ѵ�.
                    if (owner.DetectiveComponent.targetCols[i].gameObject.GetComponent<Character>().Hp > 80)
                    { 
                        unitCount++; //Ž���� ������ ü���� 80�� ���� ������ ī��Ʈ ����
                        if (unitCount == owner.DetectiveComponent.targetCols.Length) //��� ������ Hp�� 80�ʰ��� ���
                            sm.SetState((int)UNIT_STATE.Idle); //�⺻ ���� ��ȯ
                    }
                }
            }
        }
        if (owner.DetectiveComponent.IsRangeDetection == false)
        {   
            sm.SetState((int)UNIT_STATE.Idle); //Ž���� ĳ���Ͱ� ���� ��� �⺻ ���� ��ȯ
        }
        if (owner.Hp <= 0) //������ Hp�� 0�� ��� �״� ���� ��ȯ
            sm.SetState((int)UNIT_STATE.Die);
    }
}

public class UnitDieState : UnitState // ������ �״� ����
{
    public override void Enter() //���¿� �� ����
    {
        owner.GetComponent<Collider>().enabled = false; //������ �ݶ��̴��� �� �ǰ��� �Ұ����ϰ� ����
    }
    public override void Exit()
    {

    }
    public override void Update()
    {
        //������ 'Die'�ִϸ��̼��� ����ǰ� Ư�� ������ ���� ����
        if (owner.animator.GetCurrentAnimatorStateInfo(0).normalizedTime > 0.9f &&
            owner.animator.GetCurrentAnimatorStateInfo(0).IsName("Die"))
        {
            GameManager.Instance.Population--; //���� �α��� 1 ����
            GameManager.Instance.unitObjectPool.ReturnPool(owner.gameObject, owner.obpId); // ������Ʈ Ǯ�� ��Ȱ��ȭ ���·� �ٽ� ���ư�
        }
    }

}
public class UnitWorkState : UnitState
{
    public override void Enter()
    {
    }

    public override void Exit()
    {
    }

    public override void Update()
    {
        if (owner.agent.velocity != Vector3.zero)
            sm.SetState((int)UNIT_STATE.Move);
    }
}


public abstract class UnitState : State 
{
    protected Unit owner;
    public override void Init(IStateMachine sm)
    {
        this.sm = sm;
        owner = (Unit)sm.GetOwner();
    }

}
