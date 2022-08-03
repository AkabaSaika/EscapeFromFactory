using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LeftPunch : Weapon
{
    private void Awake()
    {
        WeaponTrans = InitLeftHandWeapon(1003);
        weaponBehaviour = new GreatSwordBehaviour();
    }
}
