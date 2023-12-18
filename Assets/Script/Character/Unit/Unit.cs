using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.AI;
using Photon.Pun;

public enum UNIT_STATE
{
    None,
    Idle,
    Move,
    Attack,
    Work,
    Die
}

public abstract class Unit : Character
{
    public int obpId;

    public Sprite faceSprite;
/*    public MeshRenderer meshRenderer;
    MaterialPropertyBlock mpb;*/

    public float coolTime;
    /*private void SetMPB(string propertyName, Color color)
    {
        meshRenderer.GetPropertyBlock(mpb);
        mpb.SetColor(propertyName, color);
        meshRenderer.SetPropertyBlock(mpb);
    }*/
    public int curHp;
    public override void Awake()
    {
        Hp = 100;
        curHp = Hp;
        base.Awake();
        pv.RPC("UnitLayer", RpcTarget.AllBuffered);
        //mpb = new MaterialPropertyBlock();
        InitSm();
    }

    /*private void Start()
    {
        if (pv.IsMine)
            SetMPB("_PlayerColor", Color.green);
        else
            SetMPB("_PlayerColor", Color.red);
    }*/
    public virtual void Update()
    {
        sm.UpdateState();
        animator.SetInteger("State", sm.stateEnumInt);
    }

    private void InitSm()
    {
        sm.AddState((int)UNIT_STATE.Idle, new UnitIdleState());
        sm.AddState((int)UNIT_STATE.Move, new UnitMoveState());
        sm.AddState((int)UNIT_STATE.Attack, new UnitAttackState());
        sm.AddState((int)UNIT_STATE.Work, new UnitWorkState());
        sm.AddState((int)UNIT_STATE.Die, new UnitDieState());
        //�⺻���·� ������
        sm.SetState((int)UNIT_STATE.Idle);
    }
    [PunRPC]
    public void UnitLayer()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (pv.IsMine)
                this.gameObject.layer = 6;
            else
                this.gameObject.layer = 7;
        }
        else
        {
            if(pv.IsMine)
                this.gameObject.layer = 7;
            else
                this.gameObject.layer = 6;
        }
    }



    [PunRPC]
    public void RPCSetActive(bool active)
    {
        gameObject.SetActive(active);
        if(active)
        {
            Debug.Log("DebugID : 11,,,,"+ photonView.ViewID + ":" + photonView.Owner.NickName);
        }
    }
}
