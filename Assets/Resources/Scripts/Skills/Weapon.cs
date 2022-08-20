using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using SimpleJSON;

public class Weapon : MonoBehaviour
{
    [SerializeField]
    public Transform rightHand;
    [SerializeField]
    public Transform leftHand;
    [SerializeField]
    protected WeaponBehaviour weaponBehaviour;
    //protected SkillBehaviour skillBehaviour;
    [SerializeField]
    private GameObject weaponTrans;

    private TargetSelector ts;

    public GameObject WeaponTrans { get => weaponTrans; set => weaponTrans = value; }

    protected GameObject InitRightHandWeapon(int id)
    {
        var tables = TablesSingLeton.Instance.Tables;
        var weapon = tables.TbWeapon.Get(id);
        if(weapon.Filename != "")
        {
            weaponTrans = Instantiate(Resources.Load<GameObject>("Models/Items/Weapons/" + weapon.Filename));
        }
        else
        {
            weaponTrans = new GameObject();
        }
        weaponTrans.name = weapon.Name;
        weaponTrans.transform.parent = rightHand;
        weaponTrans.transform.localPosition = weapon.PositionOffset;
        weaponTrans.transform.localRotation = Quaternion.Euler(weapon.RotateOffset);
        weaponTrans.transform.localScale = weapon.Scale;
        return weaponTrans;
    }

    protected GameObject InitLeftHandWeapon(int id)
    {
        var tables = TablesSingLeton.Instance.Tables;
        var weapon = tables.TbWeapon.Get(id);
        if (weapon.Filename != "")
        {
            weaponTrans = Instantiate(Resources.Load<GameObject>("Models/Items/Weapons/" + weapon.Filename));
        }
        else
        {
            weaponTrans = new GameObject();
        }
        weaponTrans.name = weapon.Name;
        weaponTrans.transform.parent = leftHand;
        weaponTrans.transform.localPosition = weapon.PositionOffset;
        weaponTrans.transform.localRotation = Quaternion.Euler(weapon.RotateOffset);
        weaponTrans.transform.localScale = weapon.Scale;
        return weaponTrans;
    }

    private void Start()
    {
        ts = gameObject.AddComponent<TargetSelector>();
    }

    private void Update() {
        Attack();
        //skillBehaviour.ManageSkill();
    }

    public void Attack()
    {
        weaponBehaviour.useWeapon();
    }

    public SkillParam GetNextSkill(int skillId, int weaponId)
    {
       SkillParam skillParam = Skill.InitSkill(TablesSingLeton.Instance.Tables.TbSkillParam.Get(skillId), WeaponTrans, TablesSingLeton.Instance.Tables.TbWeapon.Get(weaponId).HitPointPos);
       return skillParam;
    }
}

