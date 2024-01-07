using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
public interface IControllable
{
    public float MoveSpeed { get; set; }
    public NavMeshAgent Agent { get; set; }
}

public interface IHitAble
{
    int Hp
    { get; set; }
    int Priority
    { get; set; }
    void Hit(IAttackAble attacker);
    void Die();
}

public interface IAttackAble
{
    int Atk
    { get; set; }
    void Attack(IHitAble target);
}

public interface IProductAble
{
    IEnumerator UnitProductManagerCo();
    IEnumerator UnitCoolTimeCo(int popIndex, Transform selectBuildingTf, Building building);
    //���� ȣ�� �Լ�
    public void Production(int popIndex, Transform selectBuildingTf, Unit targetUnit);
    void ProductUIMatch();
}
