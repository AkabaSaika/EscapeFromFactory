using System.Collections;
using System.Collections.Generic;
using System.IO;
using Bright.Serialization;
using UnityEngine;
using SimpleJSON;

public class GreatSword : Weapon
{
    private void Awake()
    {
        WeaponTrans = InitRightHandWeapon(1001);
        weaponBehaviour = new GreatSwordBehaviour();
        //skillBehaviour = new PlayerSkillManager();
    }

    /// <summary>
    /// スキルを初期化する
    /// </summary>
    /// <param name="id"></param>

}
