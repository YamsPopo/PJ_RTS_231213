using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvelynnPassiveSkill : PassiveSkill
{
    private float curTime;
    private int healAmount;
    private int passiveInvokeHP; // �нú� �ߵ� ü�� ����
    IEnumerator passiveCor;

    public override void SkillInit()
    {
        base.SkillInit();
        healAmount = (int)(owner.MaxMp * 0.03f); // �ʴ� ��ü ü���� 3% �� ȸ��
        passiveCor = EvelynnPassiveCor();
    }

    public override void Active()
    {
        base.Active();
        if(owner.curState != HERO_STATE.IDLE || owner.info.CurentHp > passiveInvokeHP) { return; }

        if(owner.curState == HERO_STATE.IDLE) 
        {
            StartCoroutine(passiveCor);
        }
        else
        {
            StopCoroutine(passiveCor);
            curTime = 0;
        }
    }

    IEnumerator EvelynnPassiveCor()
    {
        curTime = 0f;

        while(true)
        {
            curTime += Time.deltaTime;
            if (curTime > 4f)
            {
                owner.info.CurentHp += healAmount;
                yield return new WaitForSeconds(1f); // �ʴ�
            }
        }
    }


}
