using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Katana : Weapon
{
    private void Awake()
    {
        rightHand = GameObject.Find("Character1_RightHand").transform;
        leftHand = GameObject.Find("Character1_LeftHand").transform;
        WeaponTrans = InitRightHandWeapon(1005);
        weaponBehaviour = new KatanaBehaviour();
    }
}
