using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AsheRSkill : ActiveSkill
{
    [SerializeField] private GameObject skillEffect;


    public override void Active()
    {
       
        Instantiate(skillEffect,transform.position,transform.rotation).GetComponent<AsheRSkillEffect>().owner = (Ashe)owner;

     //   base.Active();
        Debug.Log(owner.name + "Ashe R skill");
        
    }
}
