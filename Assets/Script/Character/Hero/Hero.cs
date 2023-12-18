using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI;

public enum SKILL_TYPE
{  QSkill, WSkill, ESkill, RSkill}

public enum HERO_STATE
{ IDLE, MOVE, ATTACK, STUN, DIE}


public abstract class Hero : Character, IControllable
{
    [Header("Hero's Info")]
    [SerializeField] private int level;
    [SerializeField] private float curExp;
    [SerializeField] private float aimExp;
    [SerializeField] private float curMp;
    [SerializeField] private float maxMp;
    [SerializeField] private Sprite heroImage;

    public Dictionary<int, Skill> skillDic;
    public HERO_STATE curState;
    public IHitAble target;
    public Character clickTarget; // �������̽� target �� ��� ���ŵǴ� transform�� ������ ������ ��� ��


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

    public float CurMp
    { get => curMp; set => curMp = value; }
    public float MaxMp
    { get => maxMp; set => maxMp = value; }

    public Sprite HeroImage
    { get=> heroImage; set => heroImage = value; }

    private void Start()
    {
        sm.AddState((int)HERO_STATE.IDLE, new HeroIdleState());
        sm.AddState((int)HERO_STATE.MOVE, new HeroMoveState());
        sm.SetState((int)HERO_STATE.IDLE);
    }

    public virtual void Update()
    {
        sm.UpdateState();
    }

    public override void InitStats() 
    { 
        level = 1;
        curExp = 0;
    }

    /// <summary>
    /// Attack �޼���� ��� ���� �������� ����
    /// </summary>
    /// // ���� �Լ� ȣ��Ǵ� ���
    // 1. ���� Ÿ�� ����
    // 2. ���� ��
    // 3. �ڵ� ����
    // 4. ���� ���� ���
    //  3-1. ���� Ÿ�� ������ ��� : �ڱ� �ڽ��� ������ ��� ����, �ǹ�, ���� ���� ����(�Ʊ� ����)
    //  3-2. ���� ���� ��� : �� �ǹ�, ����, ���� ���� ����
    public override void Attack(IHitAble target) 
    {
        this.target = target;
    }

    public abstract void Attack(IHitAble target, Transform targetTrans);

    public abstract void UseSkill(SKILL_TYPE skillType);

    private void FixedUpdate()
    {
        // velocity ���� ���ϸ� run, �ƴϸ� idle - ����
        if (agent.velocity != Vector3.zero)
        {
            curState = HERO_STATE.MOVE;
            animator.SetBool("IsMove", true);
        }   
        else if(!agent.isStopped)
        {
            curState = HERO_STATE.IDLE;
            animator.SetBool("IsMove", false);
        }
            
    }
}
