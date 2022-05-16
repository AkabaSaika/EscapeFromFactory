using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack1Behaviour : StateMachineBehaviour
{

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        int cancelID = Animator.StringToHash("CanCancel");
        base.OnStateExit(animator, stateInfo, layerIndex);
        animator.SetBool(cancelID,false);
    }
}
