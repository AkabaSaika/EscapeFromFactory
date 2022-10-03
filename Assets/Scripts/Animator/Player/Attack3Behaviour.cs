﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack3Behaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        GreatSword sm = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<GreatSword>();
        SkillParam skillParam = sm.GetNextSkill(1003,1001);
        VoiceBehaviour voice = new PlayerVoiceBehaviour();
        voice.Play("Audio/Voice/" + skillParam.VoicePath);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int cancelID = Animator.StringToHash("CanCancel");
        //base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(cancelID, false);
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<Skill>());

    }
}