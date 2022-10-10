using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEditor;


[Serializable]
public class SkillParam
{
    [SerializeField]
    private float m_attackPointEndTime;//アタックモ�`ションの���筌皎`ションの�K阻�r�g
    [SerializeField]
    private float m_backswingStartTime;//アタックモ�`ションのバックスイングの�_兵�r�g
    [SerializeField]
    private float m_attackAnimationStartTime;//アタックモ�`ションの�_兵�r�g
    [SerializeField]
    private float m_attackAnimationEndTime;//アタックモ�`ションの�K阻�r�g
    [SerializeField]
    private float m_attackPointNormalizedEndTime;//アタックモ�`ションの���筌皎`ションの屎�ｻ��K阻�r�g
    [SerializeField]
    private float m_attackAnimationNormalizedStartTime;//アタックモ�`ションの屎�ｻ��_兵�r�g
    [SerializeField]
    private float m_attackAnimationNormalizedEndTime;//アタックモ�`ションの屎�ｻ��K阻�r�g
    [SerializeField]
    private int m_power;//好�珍�
    [SerializeField]
    private GameObject m_Owner;
    [SerializeField]
    private string m_ClipName;
    [SerializeField]
    private string m_EffectPath="Audio/HammerImpact4";
    [SerializeField]
    private string m_voicePath;
    [SerializeField]
    private GameObject[] hps;
    

    public float AttackPointEndTime { get => m_attackPointEndTime; set => m_attackPointEndTime = value; }
    public float BackswingStartTime { get => m_backswingStartTime; set => m_backswingStartTime = value; }
    public float AttackAnimationStartTime { get => m_attackAnimationStartTime; set => m_attackAnimationStartTime = value; }
    public float AttackAnimationEndTime { get => m_attackAnimationEndTime; set => m_attackAnimationEndTime = value; }
    public string ClipName { get => m_ClipName; set => m_ClipName = value; }
    public GameObject Owner { get => m_Owner; set => m_Owner = value; }
    public int Power { get => m_power; set => m_power = value; }
    public string EffectPath { get => m_EffectPath; set => m_EffectPath = value; }
    public string VoicePath { get => m_voicePath; set => m_voicePath = value; }
    public GameObject[] Hps { get => hps; set => hps = value; }
    public float AttackAnimationNormalizedStartTime { get => m_attackAnimationNormalizedStartTime; set => m_attackAnimationNormalizedStartTime = value; }
    public float AttackAnimationNormalizedEndTime { get => m_attackAnimationNormalizedEndTime; set => m_attackAnimationNormalizedEndTime = value; }
    public float AttackPointNormalizedEndTime { get => m_attackPointNormalizedEndTime; set => m_attackPointNormalizedEndTime = value; }
    
}

public class Skill : MonoBehaviour
{
    public SkillParam sp = new SkillParam();
    [SerializeField]
    private List<GameObject> m_hitObject = new List<GameObject>();
    public bool isAttacking;
    private int cancelID = Animator.StringToHash("CanCancel");
    private GameObject weapon;
    private AnimationClip m_clip;
    private Animator m_anim;
    private Animation animation;
    private AnimatorStateInfo stateInfo;
    [SerializeField]
    private UnityAction action;
    private UnityAction<string> soundAction;


    public UnityAction Action { get => action; set => action = value; }
   

    private void Update()
    {
        OnAnimationEnd(Action);

        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
        if(stateInfo.normalizedTime>=sp.AttackAnimationNormalizedStartTime&&stateInfo.normalizedTime<=sp.AttackAnimationNormalizedEndTime)
        {
            foreach (var hp in sp.Hps)
            {
                StartCoroutine(DrawLineFixed(hp));
            }
        }
        if(stateInfo.normalizedTime>sp.AttackAnimationNormalizedEndTime)
        {
                //StopCoroutine(DrawLineFixed(hp));
                StopAllCoroutines();
        }
    }


    private void OnAnimatorMove()
    {
        if (sp.Owner.tag == "Player")
        {
            stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
            if (stateInfo.normalizedTime >= sp.AttackPointNormalizedEndTime && stateInfo.normalizedTime <= sp.AttackAnimationNormalizedStartTime)
            {
                sp.Owner.GetComponent<PlayerController>().Turn();
            }
            else
            {
                m_anim.ApplyBuiltinRootMotion();
            }
        }
    }
    /// <summary>
    /// スキルを兜豚晒する。
    /// 兜豚晒する�Hにこの�v方が翌何��冷匂のクラス��に柵び竃される
    /// </summary>
    /// <param name="skillParam">パラメ�`タを鯉�{するオブジェクト</param>
    /// <param name="hitPointPos">冷匂好�津亢┐臨���</param>
    public static SkillParam InitSkill(GameObject parent, cfg.test.SkillParam skillParam, GameObject[] hitPoints)
    {
        Skill skill = parent.AddComponent<Skill>() as Skill;
        skill.sp.Owner = parent;
        skill.m_anim = parent.GetComponent<Animator>();
        skill.animation = parent.GetComponent<Animation>();
        skill.sp.AttackAnimationEndTime = skillParam.AttackAnimationEndTime;
        skill.sp.AttackAnimationStartTime = skillParam.AttackAnimationStartTime;
        skill.sp.AttackPointEndTime = skillParam.AttackPointEndTime;
        skill.sp.BackswingStartTime = skillParam.BackswingStartTime;
        skill.sp.AttackPointNormalizedEndTime = skillParam.AttackPointNormalizedEndTime;
        skill.sp.AttackAnimationNormalizedStartTime = skillParam.AttackAnimationNormalizedStartTime;
        skill.sp.AttackAnimationNormalizedEndTime = skillParam.AttackAnimationNormalizedEndTime;
        skill.sp.ClipName = skillParam.SkillName;
        skill.sp.Power = skillParam.Power;
        skill.Action += skill.ClearHitObjectList;
        //skill.Action += skill.ClearHitPoint;
        skill.soundAction += skill.PlayEffect;
        skill.sp.VoicePath = skillParam.Voice;
        skill.sp.Hps = hitPoints;
        skill.AddAnimationEvent(hitPoints, skillParam.SkillName);
        
        return skill.sp;
    }
    
    /// <summary>
    /// 好�津亢┐鯢�撹する 
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
        }
    }
    /// <summary>
    /// 好�張皎`ション俐阻瘁に好�弔靴織�ブジェクトをクリアする
    /// </summary>
    private void ResetHit()
    {
        m_hitObject.Clear();
    }

    /// <summary>
    /// �啝�の壅伏堀業を�{屁する
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
    /// ヒット辛嬬な彜�Bに��る�v方を柵び竃す
    /// </summary>
    private void CallResetCanHit()
    {
        //foreach(var i in m_hitObject)
        //{
        //    i.SendMessage("SetCanHit");
        //}
    }

    public void AddAnimationEvent(GameObject[] hitPoints,string clipName)
    {
        foreach (var hp in hitPoints)
        {
            DrawHit(clipName, sp.AttackAnimationStartTime, sp.AttackAnimationEndTime, hp, m_anim);
        }


        SetCancel(sp.BackswingStartTime);
        ResetHit(sp.BackswingStartTime);
    }

    /// <summary>
    /// キャンセル辛嬬なタイミングを�O協する
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

    public void ClearHitObjectList()
    {
        m_hitObject.Clear();
    }

    private void OnAnimationEnd(UnityAction callback)
    {
        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime > 1.0f)
        {
            callback.Invoke();
        }
    }

    private void PlayEffect(string path)
    {
        AudioManager.Instance.EffectPlay(path, false);
    }

    IEnumerator DrawLineFixed(GameObject hitPoint)
    {
        int layerMask = (1 << 11) | (1 << 12);

        //Debug.DrawLine(sp.Hps[0].transform.position, sp.Hps[1].transform.position, Color.red, 60);
        RaycastHit hit;
        Debug.Log(sp.Hps[0].transform.localPosition + "\n" + sp.Hps[1].transform.localPosition);
        Ray ray = new Ray(sp.Hps[0].transform.position, sp.Hps[1].transform.position);
//#if UNITY_EDITOR
        if (RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(sp.Hps[0].transform.position, 0.1f, (sp.Hps[1].transform.position-sp.Hps[0].transform.position).normalized,  out hit, Vector3.Distance(sp.Hps[0].transform.position, sp.Hps[1].transform.position), layerMask, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Both)/* || Physics.SphereCast(sp.Hps[1].transform.position, 0.1f, sp.Hps[0].transform.position,  out hit, Vector3.Distance(sp.Hps[1].transform.position, sp.Hps[0].transform.position), layerMask)*/)
//#else
//        if(Physics.SphereCast(ray,0.1f,out hit,Vector3.Distance(sp.Hps[0].transform.position,sp.Hps[1].transform.position),layerMask)|| Physics.SphereCast(ray, 0.1f, out hit, Vector3.Distance(sp.Hps[1].transform.position, sp.Hps[0].transform.position), layerMask))
//#endif
        {
            switch (hit.collider.tag)
            {
                case "Enemy":
                    if (!gameObject.CompareTag("Enemy"))
                    {
                        if (hit.collider.gameObject.GetComponent<FSM>().canHit)
                        {
                            if (!m_hitObject.Exists(ho => ho == hit.collider.gameObject))
                            {
                                HitEvent he = new HitEvent(sp.ClipName, sp.Power);
                                m_hitObject.Add(hit.collider.gameObject);
                                foreach (var ho in m_hitObject)
                                {
                                    soundAction.Invoke(sp.EffectPath);
                                    ho.GetComponent<FSM>().Damaged(he);
                                }
                                Debug.Log(hit.collider.gameObject.name);
                                Skill.SetAnimatorSpeed(m_anim, 0.3f);
                                Invoke("AnimPlay", 0.1f);

                            }
                        }
                    }
                    break;
                case "Player":
                    if(!gameObject.CompareTag("Player"))
                    {
                        if (!m_hitObject.Exists(ho => ho == hit.collider.gameObject))
                        {
                            HitEvent he = new HitEvent(sp.ClipName, sp.Power);
                            m_hitObject.Add(hit.collider.gameObject);
                            foreach (var ho in m_hitObject)
                            {
                                ho.GetComponent<PlayerController>().Damaged(he);
                            }
                            Debug.Log(hit.collider.gameObject.name);
                            Skill.SetAnimatorSpeed(m_anim, 0.3f);
                            Invoke("AnimPlay", 0.1f);
                        }
                    }
                    break;
            }
        }
        yield return null;
    }
}
