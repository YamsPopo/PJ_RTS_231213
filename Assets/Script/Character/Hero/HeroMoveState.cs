using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroMoveState : HeroState
{
    public override void Enter()
    {
        hero.curState = HERO_STATE.MOVE;
        Debug.Log(hero.Agent.velocity);
    }

    public override void Update()
    {
        if(hero.agent.velocity == Vector3.zero)
        {
            sm.SetState((int)HERO_STATE.MOVE);
        }
    }
}
