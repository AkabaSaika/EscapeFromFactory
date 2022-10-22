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
    private float m_attackPointEndTime;//�����å���`�����Μʂ��`�����νK�˕r�g
    [SerializeField]
    private float m_backswingStartTime;//�����å���`�����ΥХå������󥰤��_ʼ�r�g
    [SerializeField]
    private float m_attackAnimationStartTime;//�����å���`�������_ʼ�r�g
    [SerializeField]
    private float m_attackAnimationEndTime;//�����å���`�����νK�˕r�g
    [SerializeField]
    private float m_attackPointNormalizedEndTime;//�����å���`�����Μʂ��`��������Ҏ���K�˕r�g
    [SerializeField]
    private float m_attackAnimationNormalizedStartTime;//�����å���`��������Ҏ���_ʼ�r�g
    [SerializeField]
    private float m_attackAnimationNormalizedEndTime;//�����å���`��������Ҏ���K�˕r�g
    [SerializeField]
    private float m_motionSpeedBeforeAttack;
    [SerializeField]
    private float m_motionSpeedDuringAttack;
    [SerializeField]
    private float m_motionSpeedWhileHit;
    [SerializeField]
    private int m_power;//������
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
    public float MotionSpeedBeforeAttack { get => m_motionSpeedBeforeAttack; set => m_motionSpeedBeforeAttack = value; }
    public float MotionSpeedDuringAttack { get => m_motionSpeedDuringAttack; set => m_motionSpeedDuringAttack = value; }
    public float MotionSpeedWhileHit { get => m_motionSpeedWhileHit; set => m_motionSpeedWhileHit = value; }
}

public class Skill : MonoBehaviour
{
    //������Υѥ��`���������{���륪�֥�������
    public SkillParam sp = new SkillParam();
    //�ҥåȤ��줿���֥������Ȥ��{����ꥹ��
    [SerializeField]
    private List<GameObject> m_hitObject = new List<GameObject>();
    //��`�����򥭥�󥻥뤹�륿���ߥ󥰤�Q���ե饰
    private int cancelID = Animator.StringToHash("CanCancel");
    //���˥�`���`�Υ���ݩ`�ͥ��
    private Animator m_anim;
    //�F�ڤ�AnimatorState�Υ��֥�������
    private AnimatorStateInfo stateInfo;
    //���˥�`����������K�˕r�˺��ӳ�����륢�������
    private UnityAction action;
    //�������Еr�Υ��������
    private UnityAction<string> soundAction;
    //���˥�`����������ٶȤα���
    private int speedMultiplierId = Animator.StringToHash("SpeedMultiplier");

    public UnityAction Action { get => action; set => action = value; }
   

    private void Update()
    {
        //���˥�`�����K�˕r�Υ��`��Хå��v������ӳ���
        OnAnimationEnd(Action);
        //�F�ڤ�AnimatorState
        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);
        //���ĥ�`������Ф˹����ж������ɤ���
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
            //�ʂ��`�����ȹ��ĥ�`�������g��ܞ����ܤʥ����ߥ󥰤�����
            if (stateInfo.normalizedTime >= sp.AttackPointNormalizedEndTime && stateInfo.normalizedTime <= sp.AttackAnimationNormalizedStartTime)
            {
                m_anim.SetFloat(speedMultiplierId, sp.MotionSpeedDuringAttack);
                sp.Owner.GetComponent<PlayerController>().Turn();
            }
            //���Έ��Ϥ�RootMotion���m�ä����
            else
            {
                m_anim.ApplyBuiltinRootMotion();
            }
        }
    }
    /// <summary>
    /// ���������ڻ����롣
    /// ���ڻ������H�ˤ����v�����ⲿ�������Υ��饹���˺��ӳ������
    /// </summary>
    /// <param name="skillParam">�ѥ��`�����{���륪�֥�������</param>
    /// <param name="hitPointPos">���������ж�������</param>
    public static SkillParam InitSkill(GameObject parent, cfg.test.SkillParam skillParam, GameObject[] hitPoints)
    {
        Skill skill = parent.AddComponent<Skill>() as Skill;
        skill.sp.Owner = parent;
        skill.m_anim = parent.GetComponent<Animator>();
        skill.sp.AttackAnimationEndTime = skillParam.AttackAnimationEndTime;
        skill.sp.AttackAnimationStartTime = skillParam.AttackAnimationStartTime;
        skill.sp.AttackPointEndTime = skillParam.AttackPointEndTime;
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
        skill.AddAnimationEvent(hitPoints, skillParam.SkillName);

        skill.m_anim.SetFloat(skill.speedMultiplierId,skill.sp.MotionSpeedBeforeAttack);

        return skill.sp;
    }
    
    /// <summary>
    /// ���ĥ�`�����������˹��Ĥ������֥������Ȥ򥯥ꥢ����
    /// </summary>
    private void ResetHit()
    {
        m_hitObject.Clear();
    }

    /// <summary>
    /// ���˥�`�������ٶȤ��{������
    /// </summary>
    /// <param name="anim"></param>
    /// <param name="speed"></param>
    public static void SetAnimatorSpeed(Animator anim, float speed)
    {
        anim.speed = speed;
    }

    /// <summary>
    /// ���˥�`�������ٶȤ�Ԫ�ˑ���
    /// </summary>
    private void AnimPlay()
    {
        m_anim.speed = 1;
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
    /// �ҥåȤ������`�४�֥������Ȥ��{����ꥹ�Ȥ򥯥ꥢ����
    /// </summary>
    public void ClearHitObjectList()
    {
        m_hitObject.Clear();
    }
    /// <summary>
    /// ���˥�`�����K�˕r��ǥ��`��Хå��v������ӳ���
    /// </summary>
    /// <param name="callback">���`��Хå��v�����{����Action</param>
    private void OnAnimationEnd(UnityAction callback)
    {
        stateInfo = m_anim.GetCurrentAnimatorStateInfo(0);

        if (stateInfo.normalizedTime > 1.0f)
        {
            callback.Invoke();
        }
    }
    /// <summary>
    /// ���ե����Ȥ���������
    /// </summary>
    /// <param name="path"></param>
    private void PlayEffect(string path)
    {
        AudioManager.Instance.EffectPlay(path, false);
    }
    /// <summary>
    /// ��`�������ٶȤ�ꥻ�åȤ���
    /// </summary>
    private void ResetSpeedMultipiler()
    {
        m_anim.SetFloat(speedMultiplierId, 1);
    }

    /// <summary>
    /// �����ж������ɤ���
    /// </summary>
    /// <param name="hitPoint"></param>
    /// <returns></returns>
    IEnumerator DrawLineFixed(GameObject hitPoint)
    {
        //Player��Enemy�쥤��`���������Ĥ����
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
                                m_anim.SetFloat(speedMultiplierId, sp.MotionSpeedWhileHit);
                                Invoke("ResetSpeedMultipiler", 0.2f);

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

        }
    }
}
