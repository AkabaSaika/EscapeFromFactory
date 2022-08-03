using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAttack1Behaviour : StateMachineBehaviour
{
    
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        LeftPunch skill = animator.gameObject.GetComponent<LeftPunch>();
        skill.GetNextSkill(1004, 1003);
    }


    
    override public void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        Destroy(animator.gameObject.GetComponent<Skill>());
    }

}
