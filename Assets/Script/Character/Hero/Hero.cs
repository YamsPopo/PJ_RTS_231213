using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.AI;

public enum SKILL_TYPE
{ QSkill, WSkill, ESkill, RSkill}

public enum HERO_STATE
{ IDLE, MOVE, ATTACK, STUN, DIE}

public abstract class Hero : Character, IControllable
{
    [SerializeField] private int level;
    [SerializeField] private float curExp;
    [SerializeField] private float aimExp;
    public HERO_STATE curState;
    
    

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
        sm = new StateMachine<Character>(this);
        InitStats();
    }

    public override void InitStats() 
    { 
        level = 1;
        curExp = 0;
        curState = HERO_STATE.IDLE;
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

    public abstract void UseSkill(Skill skill);

    private void FixedUpdate()
    {
        // velocity ���� ���ϸ� run, �ƴϸ� idle - ����
        if (agent.velocity != Vector3.zero)
            animator.SetBool("IsMove", true);
        else
            animator.SetBool("IsMove", false);
    }
}
