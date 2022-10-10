using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeavyFullMetalSword : Weapon
{
    private void Awake()
    {
        WeaponTrans = InitRightHandWeapon(1004);
        weaponBehaviour = new GreatSwordBehaviour();
    }
}
