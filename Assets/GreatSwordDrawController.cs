﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordDrawController : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.gameObject.AddComponent<HeavyFullMetalSword>();
    }
}
