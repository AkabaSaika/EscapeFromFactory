using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerSkillManager : SkillBehaviour
{
    AnimatorStateInfo currAnimInfo;
    private Animator anim;
    private GreatSword greatSword;
    private Skill skill;
    private GameObject player;

    public void ManageSkill()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        anim = player.GetComponent<Animator>();
        greatSword = player.GetComponent<GreatSword>();
        currAnimInfo = anim.GetCurrentAnimatorStateInfo(0);
        //OnAnimationStart("Attack1", 1001,greatSword.GreatSwordSlash,skill);
        //OnAnimationStart("Attack2", 1001, greatSword.GreatSwordSlash, skill);
        //OnAnimationStart("Attack3", 1001, greatSword.GreatSwordSlash, skill);
    }

    private void OnAnimationStart(string stateName,int skillId,GameObject skillObj,Skill skill)
    {
        
        var tables = TablesSingLeton.Instance.Tables;

        if (currAnimInfo.IsName(stateName) && currAnimInfo.normalizedTime == 0)
        {
            Skill.InitSkill(tables.TbSkillParam.Get(skillId),greatSword.WeaponTrans,tables.TbWeapon.Get(1001).HitPointPos);
        }
    }
}
