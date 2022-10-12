using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KatanaBehaviour : WeaponBehaviour
{
    private Animator anim = GameObject.FindWithTag("Player").GetComponent<Animator>();
    private int attack1ID = Animator.StringToHash("Attack1");
    private int attack2ID = Animator.StringToHash("Attack2");
    private int attack3ID = Animator.StringToHash("Attack3");

    public void useWeapon()
    {
        if (Input.GetMouseButtonDown(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("Katana"))
        {
            anim.SetTrigger(attack1ID);
        }
        if (Input.GetMouseButtonDown(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("KatanaAttack1")&&!anim.GetCurrentAnimatorStateInfo(0).IsName("KatanaAttack3"))
        {
            anim.SetTrigger(attack2ID);
        }
        if (Input.GetMouseButtonDown(0) && anim.GetCurrentAnimatorStateInfo(0).IsName("KatanaAttack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("KatanaAttack3"))
        {
            anim.SetTrigger(attack3ID);
        }
    }
}
