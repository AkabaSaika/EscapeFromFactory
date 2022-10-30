using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using SimpleJSON;


public class RightPunch : Weapon
{
    private void Awake()
    {
        Transform[] transforms = gameObject.GetComponentsInChildren<Transform>();
        foreach(var i in transforms)
        {
            if(i.name=="Left_Wrist_Joint_01")
            {
                leftHand = i;
            }
            if(i.name=="Right_Wrist_Joint_01")
            {
                rightHand = i;
            }
        }
        WeaponTrans = InitRightHandWeapon(1002);
        weaponBehaviour = null;
    }
}
