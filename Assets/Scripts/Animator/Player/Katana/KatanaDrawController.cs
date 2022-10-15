using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaDrawController : StateMachineBehaviour
{
    override public void OnStateUpdate(Animator animator, AnimatorStateInfo animatorStateInfo, int layerIndex)
    {
        if(animatorStateInfo.normalizedTime>=0.451f&&!animator.gameObject.GetComponent<Katana>())
        {
            animator.gameObject.AddComponent<Katana>();
        }
    }
}
