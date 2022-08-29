using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPunch : Weapon
{
    private void Awake()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach (var i in transforms)
        {
            if (i.name == "Left_Wrist_Joint_01")
            {
                leftHand = i;
            }
            if (i.name == "Right_Wrist_Joint_01")
            {
                rightHand = i;
            }
        }
        WeaponTrans = InitLeftHandWeapon(1003);
        weaponBehaviour = new GreatSwordBehaviour();
    }
}
