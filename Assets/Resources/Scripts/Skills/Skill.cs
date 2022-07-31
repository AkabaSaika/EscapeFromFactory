using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class SkillParam
{
    //private float m_attackPointStartTime;
    [SerializeField]
    private float m_attackPointEndTime;
    [SerializeField]
    private float m_backswingStartTime;
    //private float m_backswingEndTime;
    [SerializeField]
    private float m_attackAnimationStartTime;
    [SerializeField]
    private float m_attackAnimationEndTime;
    [SerializeField]
    private int m_power;
    //private Transform m_Parent;
    [SerializeField]
    private GameObject m_Owner;
    //private Vector3[] m_hitPointPos;
    
    
    private string m_ClipName;


    public float AttackPointEndTime { get => m_attackPointEndTime; set => m_attackPointEndTime = value; }
    public float BackswingStartTime { get => m_backswingStartTime; set => m_backswingStartTime = value; }
    //public float BackswingEndTime { get => m_backswingEndTime; set => m_backswingEndTime = value; }
    //public Vector3[] HitPointPos { get => m_hitPointPos; set => m_hitPointPos = value; }
    public float AttackAnimationStartTime { get => m_attackAnimationStartTime; set => m_attackAnimationStartTime = value; }
    public float AttackAnimationEndTime { get => m_attackAnimationEndTime; set => m_attackAnimationEndTime = value; }
    //public Transform Parent { get => m_Parent; set => m_Parent = value; }
    //public Animator Anim { get => m_Anim; set => m_Anim = value; }
    public string ClipName { get => m_ClipName; set => m_ClipName = value; }
    public GameObject Owner { get => m_Owner; set => m_Owner = value; }
    public int Power { get => m_power; set => m_power = value; }
}

public class Skill : MonoBehaviour
{
    public SkillParam sp = new SkillParam();
    private Vector3[] m_attacklinePos = new Vector3[2];
    private List<GameObject> m_hitObject = new List<GameObject>();
    public bool isAttacking;
    private int cancelID = Animator.StringToHash("CanCancel");
    private GameObject weapon;
    private AnimationClip m_clip;
    private Animator m_anim;
    private GameObject[] hps;


    /// <summary>
    /// スキルを初期化する。
    /// 初期化する際にこの関数が外部（武器のクラス）に呼び出される
    /// </summary>
    /// <param name="skillParam">パラメータを格納するオブジェクト</param>
    /// <param name="weapon">スキルが所属する武器</param>
    /// <param name="hitPointPos">武器攻撃判定の座標</param>
    public static void InitSkill(cfg.test.SkillParam skillParam, GameObject weapon, Vector3[] hitPointPos)
    {   
        Skill skill = GameObject.FindGameObjectWithTag(skillParam.Owner).AddComponent<Skill>() as Skill;
       
        skill.sp.Owner = GameObject.FindGameObjectWithTag(skillParam.Owner);
        skill.m_anim = skill.sp.Owner.GetComponent<Animator>();
        skill.sp.AttackAnimationEndTime = skillParam.AttackAnimationEndTime;
        skill.sp.AttackAnimationStartTime = skillParam.AttackAnimationStartTime;
        skill.sp.AttackPointEndTime = skillParam.AttackPointEndTime;
        skill.sp.BackswingStartTime = skillParam.BackswingStartTime;
        skill.sp.ClipName = skillParam.SkillName;
        skill.sp.Power = skillParam.Power;
        skill.hps = skill.InitHitPoints(hitPointPos, weapon.transform);
        skill.AddAnimationEvent(skill.hps, skillParam.SkillName);
    }


    public GameObject[] InitHitPoints(Vector3[] hitPointPos,Transform parent)
    {

        int length = hitPointPos.Length;
        GameObject[] hitPoints = new GameObject[length];
        for (int i = 0;i<length;i++)
        {
            hitPoints[i] = new GameObject("Empty");
            hitPoints[i].name = "hitPoint"+(i+1).ToString();
            hitPoints[i].transform.SetParent(parent);
            hitPoints[i].transform.localPosition = hitPointPos[i];
        }
        return hitPoints;
    }


    /// <summary>
    /// 攻撃判定を生成する 
    /// </summary>
    /// <param name="clipName"></param>
    /// <param name="startTime"></param>
    /// <param name="endTime"></param>
    /// <param name="hitPoint"></param>
    private void DrawHit(string clipName,float startTime,float endTime,GameObject hitPoint,Animator anim)
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (string.Equals(clip.name, clipName))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName = "ResetHit";
                events.time = 0.01f;
                clip.AddEvent(events);
            }
            for (float i = startTime; i <= endTime; i += 0.01f)
            {
                if (string.Equals(clip.name, clipName))
                {
                    AnimationEvent events = new AnimationEvent();
                    events.objectReferenceParameter=hitPoint;
                    events.functionName = "CastHitLine";
                    events.time = i;
                    clip.AddEvent(events);
                }
            }
        }
    }
    /// <summary>
    /// 
    /// </summary>
    private void ResetHit()
    {
        m_attacklinePos[0] = m_attacklinePos[1] = Vector3.zero;
        m_hitObject.Clear();
    }

    /// <summary>
    /// 攻撃判定の線分を生成する
    /// </summary>
    /// <param name="hitPoint"></param>
    private void CastHitLine(GameObject hitPoint)
    {
        int layA = LayerMask.NameToLayer("Default");
        m_attacklinePos[1] = hitPoint.transform.position;
        
        if (m_attacklinePos[0]==Vector3.zero)
        {
            m_attacklinePos[0] = m_attacklinePos[1];
        }
        Debug.DrawLine(m_attacklinePos[0], m_attacklinePos[1], Color.red, 60);
        RaycastHit hit;
        if (Physics.Linecast(m_attacklinePos[0], m_attacklinePos[1], out hit, 1 << layA)||Physics.Linecast(m_attacklinePos[1], m_attacklinePos[0], out hit, 1 << layA))
        {
            Debug.Log("hit");
            switch (hit.collider.tag)
            {
                case "Enemy":
                    if(hit.collider.gameObject.GetComponent<FSM>().canHit)
                    {    
                        HitEvent he = new HitEvent(sp.ClipName, sp.Power);
                        //NotifyObservers();
                        m_hitObject.Add(hit.collider.gameObject);          
                        foreach(var ho in m_hitObject)
                        {
                            ho.GetComponent<FSM>().Damaged(he);
                        }
                        Debug.Log(hit.collider.gameObject.name);
                        //hit.collider.SendMessage("Damaged");
                        Skill.SetAnimatorSpeed(m_anim, 0.3f);
                        Invoke("AnimPlay", 0.1f);
                    }
                    break;
            }

        }
        m_attacklinePos[0] = m_attacklinePos[1];
    }

    /// <summary>
    /// 動画の再生速度を調整する
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="speed"></param>
    public static void SetAnimatorSpeed(Animator anim, float speed)
    {
        anim.speed = speed;
    }

    private void AnimPlay()
    {
        m_anim.speed = 1;
    }

    /// <summary>
    /// ヒット可能な状態に戻る関数を呼び出す
    /// </summary>
    private void CallResetCanHit()
    {
        foreach(var i in m_hitObject)
        {
            i.SendMessage("SetCanHit");
        }
    }

    public void AddAnimationEvent(GameObject[] hitPoints,string clipName)
    {
        foreach (var hp in hitPoints)
        {
            DrawHit(clipName,sp.AttackAnimationStartTime, sp.AttackAnimationEndTime, hp, m_anim);
         }
        SetCancel(sp.BackswingStartTime);
        ResetHit(sp.BackswingStartTime);
    }

    /// <summary>
    /// キャンセル可能なタイミングを設定する
    /// </summary>
    /// <param name="backswingStartTime"></param>
    private void SetCancel(float backswingStartTime)
    {
        AnimationClip[] clips = m_anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (string.Equals(clip.name, sp.ClipName))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName = "CallResetCanHit";
                events.time = backswingStartTime;
                clip.AddEvent(events);
            }
        }
    }

    private void ResetHit(float backswingStartTime)
    {
        AnimationClip[] clips = m_anim.runtimeAnimatorController.animationClips;
        foreach (var clip in clips)
        {
            if (string.Equals(clip.name, sp.ClipName))
            {
                AnimationEvent events = new AnimationEvent();
                events.objectReferenceParameter = m_anim;
                events.functionName = "CancelState";
                events.time = backswingStartTime;
                clip.AddEvent(events);
            }
        }
    }

    private void NotifyObservers()
    {
        
    }

    private void CancelState(Animator anim)
    {
        anim.SetBool(cancelID, true);
    }


    private void DisableRootMotion()
    {
        m_anim.applyRootMotion = false;
    }

    private Animator SetAnimationController(GameObject owner)
    {
        Animator anim;
        anim = owner.GetComponent<Animator>();
        return anim;
    }
}
