using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using SimpleJSON;


public class Punch : Weapon
{
    private void Awake()
    {
        WeaponTrans = InitRightHandWeapon(1002);
        weaponBehaviour = new GreatSwordBehaviour();
    }
}
