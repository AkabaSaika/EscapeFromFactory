using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack2Behaviour : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        GreatSword sm = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<GreatSword>();
        sm.GetNextSkill(1002,1001);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int cancelID = Animator.StringToHash("CanCancel");
        animator.SetBool(cancelID, false);
        Destroy(GameObject.FindGameObjectWithTag("Player").GetComponent<Skill>());

    }
}
