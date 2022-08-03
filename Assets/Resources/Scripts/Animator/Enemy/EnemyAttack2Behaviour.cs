using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack2Behaviour : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        RightPunch skill = animator.gameObject.GetComponent<RightPunch>();
        skill.GetNextSkill(1005, 1002);
    }



    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject.GetComponent<Skill>());
    }
}
