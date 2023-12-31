using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morgana : Hero
{
   
    [SerializeField] List<Skill> skillList;
    public List<Transform> skillTransform;

    public override void Awake()
    {
        base.Awake();
        sm = new StateMachine<Character>(this);

        skillDic = new Dictionary<int, Skill>();
        skillDic.Add((int)SKILL_TYPE.QSkill, skillList[(int)SKILL_TYPE.QSkill]);
        skillDic.Add((int)SKILL_TYPE.WSkill, skillList[(int)SKILL_TYPE.WSkill]);
        skillDic.Add((int)SKILL_TYPE.ESkill, skillList[(int)SKILL_TYPE.ESkill]);
        skillDic.Add((int)SKILL_TYPE.RSkill, skillList[(int)SKILL_TYPE.RSkill]);

        skillTransform = new List<Transform>();

        foreach (var keyValue in skillDic)
        {
            if(GetComponent<PhotonView>().IsMine)
            {
                keyValue.Value.SetOwner(this);
            }
        }

        //for (int i = 0; i < UIManager.Instance.skillSlots.Length; i++)
        //{
        //    UIManager.Instance.skillSlots[i].Init(this);
        //}
        InitStats();
    }

    public override void Update()
    {
        base.Update();
        if (Input.GetKeyDown(KeyCode.Q))
        {
            if (GetComponent<PhotonView>().IsMine)
                GetComponent<PhotonView>().RPC("QSkill", RpcTarget.AllBuffered);
            //UseSkill(SKILL_TYPE.QSkill);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            UseSkill(SKILL_TYPE.WSkill);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            UseSkill(SKILL_TYPE.ESkill);
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            UseSkill(SKILL_TYPE.RSkill);
        }

    }
    public override void InitStats()
    {
        base.InitStats();
        info.MaxHp = 200;
        info.CurentHp = info.MaxHp;
        info.Atk = 10;
        info.Def = 10;
        MoveSpeed = 10f;
        info.AtkSpeed = 1f;
        info.AtkRange = 10f;
        Agent.speed = MoveSpeed;
        Agent.angularSpeed = 1200f;
    }
    public override void Attack(IHitAble target)
    {
        base.Attack(target);
    }

    public override void UseSkill(SKILL_TYPE skillType)
    {
        UIManager.Instance.skillSlots[(int)skillType].TrySkillActive();
        Debug.Log("����");
    }


    public override void Die()
    {
        curState = HERO_STATE.DIE;
        animator.SetTrigger("DieTrigger");
    }

    public override void Hit(IAttackAble attacker)
    {
        if (curState == HERO_STATE.DIE) { return; }

        info.CurentHp -= attacker.Atk;
        if (info.CurentHp <= 0)
        {
            Die();
        }
    }

    public override void Attack(IHitAble target, Transform targetTrans)
    {
        //throw new System.NotImplementedException();
    }

    [PunRPC]
    public void QSkill()
    {
            UseSkill(SKILL_TYPE.QSkill);
    }
}
