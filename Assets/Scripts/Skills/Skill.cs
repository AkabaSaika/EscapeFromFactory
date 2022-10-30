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
    private float m_backswingStartTime;//バックスイング開始のタイミング
    [SerializeField]
    private float m_attackPointNormalizedEndTime;//準備モーション終了のタイミング
    [SerializeField]
    private float m_attackAnimationNormalizedStartTime;//攻撃モーション開始のタイミング
    [SerializeField]
    private float m_attackAnimationNormalizedEndTime;//攻撃モーション終了のタイミング
    [SerializeField]
    private float m_motionSpeedBeforeAttack;//準備モーションの再生速度
    [SerializeField]
    private float m_motionSpeedDuringAttack;//攻撃モーションの再生速度
    [SerializeField]
    private float m_motionSpeedWhileHit;//命中時のモーション速度
    [SerializeField]
    private int m_power;//攻撃力
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
    public Vector3 hitOffset{get;set;}
    

    public float BackswingStartTime { get => m_backswingStartTime; set => m_backswingStartTime = value; }
    public string ClipName { get => m_ClipName; set => m_ClipName = value; }
    public GameObject Owner { get => m_Owner; set => m_Owner = value; }
    public int Power { get => m_power; set => m_power = value; }
    public string EffectPath { get => m_EffectPath; set => m_EffectPath = value; }
    public string VoicePath { get => m_voicePath; set => m_voicePath = value; }
    public GameObject[] Hps { get => hps; set => hps = value; }
    public float AttackAnimationNormalizedStartTime { get => m_attackAnimationNormalizedStartTime; set => m_attackAnimationNormalizedStartTime = value; }
    public float AttackAnimationNormalizedEndTime { get => m_attackAnimationNormalizedEndTime; set => m_attackAnimationNormalizedEndTime = value; }
    public float AttackPointNormalizedEndTime { get => m_attackPointNormalizedEndTime; set => m_attackPointNormalizedEndTime = value; }
    public float MotionSpeedBeforeAttack { get => m_motionSpeedBeforeAttack; set => m_motionSpeedBeforeAttack = value; }
    public float MotionSpeedDuringAttack { get => m_motionSpeedDuringAttack; set => m_motionSpeedDuringAttack = value; }
    public float MotionSpeedWhileHit { get => m_motionSpeedWhileHit; set => m_motionSpeedWhileHit = value; }
}

public class Skill : MonoBehaviour
{
    //スキルのデータを格納するオブジェクト
    public SkillParam sp = new SkillParam();
    //ヒットしたオブジェクトを格納するリスト
    [SerializeField]
    private List<GameObject> m_hitObject = new List<GameObject>();
    //キャンセルできるタイミングを示すBool値
    private int cancelID = Animator.StringToHash("CanCancel");
    //Animatorのリファレンス
    private Animator m_anim;
    //現在のState
    private AnimatorStateInfo stateInfo;
    //アニメーション終了時に呼び出されるaction
    private UnityAction action;
    //攻撃エフェクトを再生するaction
    private UnityAction<string> soundAction;
    //モーションのスピード倍率
    private int speedMultiplierId = Animator.StringToHash("SpeedMultiplier");

    public UnityAction Action { get => action; set => action = value; }
   

    private void Update()
    {
        //アニメーション終了時に登録された関数を呼び出す
        OnAnimationEnd(Action);
        //現在のAnimatorState
        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
        //攻撃判定を生成する
        if(stateInfo.normalizedTime>=sp.AttackAnimationNormalizedStartTime&&stateInfo.normalizedTime<=sp.AttackAnimationNormalizedEndTime)
        {
            foreach (var hp in sp.Hps)
            {
                StartCoroutine(DrawLineFixed(hp));
            }
        }
        if(stateInfo.normalizedTime>sp.AttackAnimationNormalizedEndTime)
        {
            StopAllCoroutines();
        }
    }

    
    private void OnAnimatorMove()
    {
        if (sp.Owner.tag == "Player")
        {
            stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
            //準備モーションと攻撃モーションの間に転向可能なタイミングを設定する
            if (stateInfo.normalizedTime >= sp.AttackPointNormalizedEndTime && stateInfo.normalizedTime <= sp.AttackAnimationNormalizedStartTime)
            {
                m_anim.SetFloat(speedMultiplierId, sp.MotionSpeedDuringAttack);
                sp.Owner.GetComponent<PlayerController>().Turn();
            }
            //それ以外はRootMotionのみ
            else
            {
                m_anim.ApplyBuiltinRootMotion();
            }
        }
    }
    /// <summary>
    /// 锟斤拷锟斤拷锟斤拷锟斤拷锟节伙拷锟斤拷锟诫。
    /// 锟斤拷锟节伙拷锟斤拷锟斤拷锟紿锟剿わ拷锟斤拷锟絭锟斤拷锟斤拷锟解部锟斤拷锟斤拷锟斤拷锟轿ワ拷锟介ス锟斤拷锟剿猴拷锟接筹拷锟斤拷锟斤拷锟�
    /// </summary>
    /// <param name="skillParam">锟窖ワ拷锟絗锟斤拷锟斤拷锟絳锟斤拷锟诫オ锟街ワ拷锟斤拷锟斤拷锟斤拷</param>
    /// <param name="hitPointPos">锟斤拷锟斤拷锟斤拷锟斤拷锟叫讹拷锟斤拷锟斤拷锟斤拷</param>
    public static SkillParam InitSkill(GameObject parent, cfg.test.SkillParam skillParam, GameObject[] hitPoints)
    {
        Skill skill = parent.AddComponent<Skill>() as Skill;
        skill.sp.Owner = parent;
        skill.m_anim = parent.GetComponent<Animator>();
        skill.sp.BackswingStartTime = skillParam.BackswingStartTime;
        skill.sp.AttackPointNormalizedEndTime = skillParam.AttackPointNormalizedEndTime;
        skill.sp.AttackAnimationNormalizedStartTime = skillParam.AttackAnimationNormalizedStartTime;
        skill.sp.AttackAnimationNormalizedEndTime = skillParam.AttackAnimationNormalizedEndTime;
        skill.sp.MotionSpeedBeforeAttack = skillParam.MotionSpeedBeforeAttack;
        skill.sp.MotionSpeedDuringAttack = skillParam.MotionSpeedDuringAttack;
        skill.sp.MotionSpeedWhileHit = skillParam.MotionSpeedWhileHit;
        skill.sp.ClipName = skillParam.SkillName;
        skill.sp.Power = skillParam.Power;
        skill.Action += skill.ClearHitObjectList;
        skill.soundAction += skill.PlayEffect;
        skill.sp.VoicePath = skillParam.Voice;
        skill.sp.Hps = hitPoints;
        skill.sp.hitOffset=skillParam.HitOffset;
        skill.AddAnimationEvent(hitPoints, skillParam.SkillName);

        skill.m_anim.SetFloat(skill.speedMultiplierId,skill.sp.MotionSpeedBeforeAttack);

        return skill.sp;
    }
    

    /// <summary>
    /// アニメーション再生速度を設定する
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="speed"></param>
    public static void SetAnimatorSpeed(Animator anim, float speed)
    {
        anim.speed = speed;
    }

    public void AddAnimationEvent(GameObject[] hitPoints,string clipName)
    {
        ResetHit(sp.BackswingStartTime);
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

    /// <summary>
    /// ヒットしたオブジェクトを格納するリストをクリアする
    /// </summary>
    public void ClearHitObjectList()
    {
        m_hitObject.Clear();
    }
    /// <summary>
    /// アニメーション再生終了時にcallbackに登録した関数を呼び出す
    /// </summary>
    /// <param name="callback">コールバック関数を格納するAction</param>
    private void OnAnimationEnd(UnityAction callback)
    {
        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime > 1.0f)
        {
            callback.Invoke();
        }
    }
    /// <summary>
    /// 攻撃エフェクトを再生
    /// </summary>
    /// <param name="path"></param>
    private void PlayEffect(string path)
    {
        AudioManager.Instance.EffectPlay(path, false);
    }
    /// <summary>
    /// アニメーションの再生速度を元に戻す
    /// </summary>
    private void ResetSpeedMultipiler()
    {
        m_anim.SetFloat(speedMultiplierId, 1);
    }

    /// <summary>
    /// 攻撃判定を生成する
    /// </summary>
    /// <param name="hitPoint">SphereCastの端点を格納するオブジェクト</param>
    /// <returns></returns>
    IEnumerator DrawLineFixed(GameObject hitPoint)
    {
        //Player锟斤拷Enemy锟届イ锟斤拷`锟斤拷锟斤拷锟斤拷锟斤拷锟侥わ拷锟斤拷锟�
        int layerMask = (1 << 11) | (1 << 12);

        //Debug.DrawLine(sp.Hps[0].transform.position, sp.Hps[1].transform.position, Color.red, 60);
        RaycastHit hit;
        Debug.Log(sp.Hps[0].transform.localPosition + "\n" + sp.Hps[1].transform.localPosition);
        Ray ray = new Ray(sp.Hps[0].transform.position, sp.Hps[1].transform.position);
//#if UNITY_EDITOR
        //if (RotaryHeart.Lib.PhysicsExtension.Physics.SphereCast(sp.Hps[0].transform.position, 0.1f, (sp.Hps[1].transform.position-sp.Hps[0].transform.position).normalized,  out hit, Vector3.Distance(sp.Hps[0].transform.position, sp.Hps[1].transform.position), layerMask, RotaryHeart.Lib.PhysicsExtension.PreviewCondition.Editor)/*|| Physics.SphereCast(sp.Hps[1].transform.position, 0.1f, (sp.Hps[0].transform.position-sp.Hps[1].transform.position).normalized,  out hit, Vector3.Distance(sp.Hps[1].transform.position, sp.Hps[0].transform.position), layerMask)*/)
//#else
        //
        if(Physics.SphereCast(sp.Hps[0].transform.position, 0.1f, (sp.Hps[1].transform.position - sp.Hps[0].transform.position).normalized, out hit, Vector3.Distance(sp.Hps[0].transform.position, sp.Hps[1].transform.position), layerMask))
//#endif
        {
            AutoLockOn(true);
            switch (hit.collider.tag)
            {
                case "Enemy":
                    if (!gameObject.CompareTag("Enemy"))
                    {
                            if (!m_hitObject.Exists(ho => ho == hit.collider.gameObject))
                            {
                                
                                HitEvent he = new HitEvent(sp.ClipName, sp.Power,sp.hitOffset);
                                m_hitObject.Add(hit.collider.gameObject);
                                foreach (var ho in m_hitObject)
                                {
                                    soundAction.Invoke(sp.EffectPath);
                                    ho.GetComponent<FSM>().Damaged(he);
                                }
                                Debug.Log(hit.collider.gameObject.name);
                                m_anim.SetFloat(speedMultiplierId, sp.MotionSpeedWhileHit);
                                Invoke("ResetSpeedMultipiler", 0.2f);

                            }
                    }
                    break;
                case "Player":
                    if(!gameObject.CompareTag("Player"))
                    {
                        if (!m_hitObject.Exists(ho => ho == hit.collider.gameObject))
                        {
                            
                            HitEvent he = new HitEvent(sp.ClipName, sp.Power,sp.hitOffset);
                            m_hitObject.Add(hit.collider.gameObject);
                            foreach (var ho in m_hitObject)
                            {
                                ho.GetComponent<PlayerController>().Damaged(he);
                            }
                            Debug.Log(hit.collider.gameObject.name);
                            m_anim.SetFloat(speedMultiplierId, sp.MotionSpeedWhileHit);
                            Invoke("ResetSpeedMultipiler", 0.2f);
                        }
                    }
                    break;
            }
        }
        yield return null;
    }

    private void AutoLockOn(bool isAutoLockOn)
    {
        if(isAutoLockOn)
        {
            Collider[] colliders = Physics.OverlapSphere(transform.position,5,1<<12);
            if(colliders.Length>0)
            {
                transform.LookAt(NearestObject(colliders).transform.position);
            }
            
        }
    }

    private void OnGUI() {
       if(m_hitObject.Count>0)
       {
           GUI.Box(new Rect(transform.position.x,transform.position.y,500,100),m_hitObject[0].name);
       }
    }

    private GameObject NearestObject(Collider[] colliders)
    {
        float tmpShortestDistance=float.MaxValue;
        GameObject nearestObject=colliders[0].gameObject;
        foreach(var i in colliders)
        {
            float distance = Vector3.Distance(transform.position,i.transform.position);
            if(distance<tmpShortestDistance)
            {
                tmpShortestDistance=distance;
                nearestObject=i.gameObject;
            }
        }
        Debug.Log("nearest object is:"+nearestObject.name);
        return nearestObject;
    }
}
