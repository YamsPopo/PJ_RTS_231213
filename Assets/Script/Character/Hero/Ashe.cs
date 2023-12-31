using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ashe : Hero
{
    [SerializeField] int concentraction; // Q 스킬 스택
    [SerializeField] List<Skill> skillList;

    public Transform defaultTrans;
    public Transform[] qSkillTrans;

    Coroutine attackdelayCo;
    public Coroutine qSkilldelayCo;
    public Coroutine wSkilldelayCo;
    public Coroutine eSkilldelayCo;
    public int attackCount;
    float time = 0;
    public int Concentraction { get => concentraction; set { concentraction = value; } }

    public override void Awake()
    {
        base.Awake();
        sm = new StateMachine<Character>(this);

        skillDic = new Dictionary<int, Skill>();
        skillDic.Add((int)SKILL_TYPE.QSkill, skillList[(int)SKILL_TYPE.QSkill]);
        skillDic.Add((int)SKILL_TYPE.WSkill, skillList[(int)SKILL_TYPE.WSkill]);
        skillDic.Add((int)SKILL_TYPE.ESkill, skillList[(int)SKILL_TYPE.ESkill]);
        skillDic.Add((int)SKILL_TYPE.RSkill, skillList[(int)SKILL_TYPE.RSkill]);

        foreach (var keyValue in skillDic)
        {
            keyValue.Value.SetOwner(this);
        }

        //for(int i = 0; i < UIManager.Instance.skillSlots.Length; i++)
        //{
        //    UIManager.Instance.skillSlots[i].Init(GameManager.Instance.PlayerHero);
        //}
        InitStats();

        attackCount = 0;
    }

    private void FixedUpdate()
    {
        if (GameManager.Instance.PlayerHero != this) return;

        if (time > skillDic[0].CoolTime)
        {
            time = 0;
            attackCount = 0;
            skillDic[0].isActive = false;
        }

        if(skillDic[0].isActive)
        {
            time += Time.deltaTime;
            Debug.Log("Q스킬 시작");

            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

                if (Physics.Raycast(ray, out RaycastHit hitInfo))
                {
                    if (Vector3.Distance(hitInfo.transform.position, transform.position) > skillDic[0].range)
                        return;

                    if (hitInfo.transform.gameObject.TryGetComponent<Character>(out Character target))
                    {
                        clickTarget = target ;
                        curState = HERO_STATE.ATTACK;
                        agent.isStopped = true;
                        UseSkill(SKILL_TYPE.QSkill);
                        Debug.Log("Q 스킬 사용됌");
                    }
                    if (hitInfo.transform.gameObject.TryGetComponent<Building>(out Building buildTarget))
                    {
                        clickBuilding = buildTarget;
                        curState = HERO_STATE.ATTACK;
                        agent.isStopped = true;
                        UseSkill(SKILL_TYPE.QSkill);
                    }
                }
            }
        }

        if (attackCount >= 3)
        {
            if (Input.GetKeyDown(KeyCode.Q))
            {
                skillDic[0].isActive = true;
            }
        }

        if (!skillDic[0].isActive && Input.GetMouseButtonDown(1))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                if (Vector3.Distance(hitInfo.transform.position, transform.position) > skillDic[0].range)
                    return;

                if (hitInfo.transform.gameObject.TryGetComponent<Character>(out Character target))
                {
                    if (target == this) return;
                    clickTarget = target;
                    curState = HERO_STATE.ATTACK;
                    agent.isStopped = true;
                    Attack(target, target.transform);
                }
                if (hitInfo.transform.gameObject.TryGetComponent<Building>(out Building buildTarget))
                {
                    if (buildTarget == this) return;
                    clickBuilding = buildTarget;
                    curState = HERO_STATE.ATTACK;
                    agent.isStopped = true;
                    Attack(buildTarget, buildTarget.transform);
                }
            }
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            clickTarget = null;
            curState = HERO_STATE.ATTACK;
            agent.isStopped = true;
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("애쉬 E 사용");
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            if (Physics.Raycast(ray, out RaycastHit hitInfo))
            {
                clickPos = hitInfo.point;
                curState = HERO_STATE.ATTACK;
                agent.isStopped = true;
            }
        }
    }
    public override void Update()
    {
        base.Update();
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
        concentraction = 0;
    }
    public override void Attack(IHitAble target)
    {
        base.Attack(target);
    }

    public override void Attack(IHitAble target, Transform targetTrans)
    {
        base.Attack(target);
        animator.SetBool("AttackBasic", true);
        transform.forward = (targetTrans.position - this.transform.position).normalized;
        GameObject temp = InstantiateVFX("fx_small_arrow", defaultTrans, true);
        temp.transform.forward = (targetTrans.position - defaultTrans.position).normalized;
        attackCount++;
        attackdelayCo = StartCoroutine(AttackDelayCo());
    }

    public GameObject InstantiateVFX(string prefabName, Transform transform, bool setPanrentNull = false)
    {
        GameObject temp = (GameObject)Instantiate(Resources.Load("VFX/" + prefabName), transform, false); //옵젝풀로 바꿔주는게 좋을까나
        if(setPanrentNull)
            temp.transform.SetParent(null);

        return temp;
    }

    public override void UseSkill(SKILL_TYPE skillType)
    {
        UIManager.Instance.skillSlots[(int)skillType].TrySkillActive();
    }


    public override void Die()
    {
        curState = HERO_STATE.DIE;
        animator.SetTrigger("DieTrigger");

    }

    public override void Hit(IAttackAble attacker)
    {
        if (curState == HERO_STATE.DIE) { return; }

        if(attacker == null) { return; }    

        info.CurentHp -= attacker.Atk;
        if(info.CurentHp <= 0 )
        {
            Die();
        }

    }

    IEnumerator AttackDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("AttackBasic", false);
        agent.isStopped = false;
        StopCoroutine(attackdelayCo);
        yield return null;
    }

    public IEnumerator QSkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("QSkill", false);
        agent.isStopped = false;
        StopCoroutine(qSkilldelayCo);
        yield return null;
    }

    public IEnumerator WSkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("AttackBasic", false);
        agent.isStopped = false;
        StopCoroutine(wSkilldelayCo);
        yield return null;
    }
    public IEnumerator ESkillDelayCo()
    {
        yield return new WaitForSeconds(0.5f);
        curState = HERO_STATE.IDLE;
        animator.SetBool("QSkill", false);
        agent.isStopped = false;
        StopCoroutine(eSkilldelayCo);
        yield return null;
    }

}
