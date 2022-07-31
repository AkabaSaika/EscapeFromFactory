using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GreatSwordBehaviour : WeaponBehaviour
{
    private Animator anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    private int attack1ID = Animator.StringToHash("Attack1");
    private int attack2ID = Animator.StringToHash("Attack2");
    private int attack3ID = Animator.StringToHash("Attack3");

    public void useWeapon()
    {
        if(Input.GetMouseButtonDown(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
        {
            anim.SetTrigger(attack1ID);
        }
        if(Input.GetMouseButtonDown(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            anim.SetTrigger(attack2ID);
        }
        if(Input.GetMouseButtonDown(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetTrigger(attack3ID);
        }
    }
}
