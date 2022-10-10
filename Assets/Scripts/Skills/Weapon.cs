using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.IO;
using UnityEngine;
using SimpleJSON;

public enum Side
{
    Left,
    Right
}

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
    [SerializeField]
    private GameObject[] leftHitPoints;
    [SerializeField]
    private GameObject[] rightHitPoints;

    [SerializeField]
    private GameObject[] hitPoints;


    public Side side;

    public GameObject WeaponTrans { get => weaponTrans; set => weaponTrans = value; }
    public GameObject[] LeftHitPoints { get => leftHitPoints; set => leftHitPoints = value; }
    public GameObject[] RightHitPoints { get => rightHitPoints; set => rightHitPoints = value; }

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
        side = Side.Right;
        hitPoints = InitHitPoints(weapon.HitLinePos, weaponTrans.transform);
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
        side = Side.Left;
        hitPoints = InitHitPoints(weapon.HitLinePos, weaponTrans.transform);
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

    private GameObject[] InitHitPoints(Vector3[] hitPointPos, Transform parent)
    {
        int length = hitPointPos.Length;
        GameObject[] hitPoints = new GameObject[length];


        for (int i = 0; i < length; i++)
        {
            hitPoints[i] = new GameObject("Empty");
            hitPoints[i].name = gameObject.ToString() + parent.ToString() + "hitPoint" + (i + 1).ToString();
            hitPoints[i].transform.SetParent(parent);
            hitPoints[i].transform.localPosition = hitPointPos[i];
        }
        return hitPoints;
    }

    public void Attack()
    {
        weaponBehaviour.useWeapon();
    }

    public SkillParam GetNextSkill(int skillId, int weaponId)
    {
        SkillParam skillParam = Skill.InitSkill(gameObject, TablesSingLeton.Instance.Tables.TbSkillParam.Get(skillId),  hitPoints);
        return skillParam;
    }
}

