using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

public enum SKILL_TYPE
{ QSkill, WSkill, ESkill, RSkill}

public abstract class Hero : Character, IControllable
{
    private int level;
    private float curExp;
    private float aimExp;
    
    //Property �κ� ����� ����並 ���� ������Ʈ
    public int Level
    { get => level; set => level = value; }
    public float CurExp
    { get => curExp; set => curExp = value; }
    public float AimExp
    { get => aimExp; set => aimExp = value; }
    public float MoveSpeed
    { get => info.MoveSpeed; set => info.MoveSpeed = value; }
    public NavMeshAgent Agent
    { get => agent; set => agent = value; }

    private void Start()
    {
        InitStats();
    }

    public override void InitStats() 
    { 
        level = 1;
        curExp = 0;
    }

    /// <summary>
    /// Attack �޼���� ��� ���� �������� ����
    /// </summary>
    public override void Attack(IHitAble target)
    {
        // ���� �Լ� ȣ��Ǵ� ���
        // 1. ���� Ÿ�� ����
        // 2. ���� ��
        // 3. �ڵ� ����
        // 4. ���� ���� ���
        //  3-1. ���� Ÿ�� ������ ��� : �ڱ� �ڽ��� ������ ��� ����, �ǹ�, ���� ���� ����(�Ʊ� ����)
        //  3-2. ���� ���� ��� : �� �ǹ�, ����, ���� ���� ����
    }

    public abstract void UseSkill(SOSkill skill);
}
