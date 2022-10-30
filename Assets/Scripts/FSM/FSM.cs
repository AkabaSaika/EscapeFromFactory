using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using DG.Tweening;

public enum StateType
{
    Idle,Patrol,Chase,Return,Attack,Battle,Damage,Dead
}

[Serializable]
public class Parameter
{
    public int health;
    public float moveSpeed;
    public float chaseSpeed;
    public float idleTime;
    public Transform[] patrolPoints;
    public Transform thisTansform;
    public Transform[] chasePoints;
    public Animator anim;
    public Transform respawnPoint;
    public Rigidbody rb;

    public float idleTimer;
    public NavMeshAgent agent;

    public float MAX_VISION_DISTANCE;
    public Transform player;
    public float MAX_ATTACK_DISTANCE;
    public float MAX_CHASE_DISTANCE;

}

public class FSM : MonoBehaviour,Observer
{
    public Parameter parameter;
    public IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    private SkillParameter skillParameter;
    private PlayerController player;
    private Animator playerAnim;
    private AnimatorStateInfo currAnimInfo, lastAnimInfo;
    public bool canHit;
    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.Return, new ReturnState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Battle, new BattleState(this));
        states.Add(StateType.Damage,new DamageState(this));
        states.Add(StateType.Dead, new DeadState(this));
        parameter.thisTansform = this.transform;
        parameter.player = GameObject.FindGameObjectWithTag("Player").transform;
        parameter.anim = GetComponent<Animator>();
        parameter.agent = GetComponent<NavMeshAgent>();
        parameter.rb = GetComponent<Rigidbody>();
        TransitionState(StateType.Idle);
        skillParameter=GameObject.Find("Game").GetComponent<SkillParameter>();
        player=GetComponent<PlayerController>();
        playerAnim=GameObject.FindGameObjectWithTag("Player").GetComponent<Animator>();
        canHit=true;
        
    }

    // Update is called once per frame
    void Update()
    {
        currentState.OnUpdate();  
        
    }

    public void TransitionState(StateType type)
    {
        if(currentState!=null)
        {
            currentState.OnExit();
        }
        currentState = states[type];
        currentState.OnEnter();
    }

/// <summary>
/// 角度と距離によってプレイヤーの位置を検知する
/// </summary>
/// <returns></returns>
    public bool DetectPlayer()
    {
        Vector3 verticalOffset = new Vector3(0, 0.5f, 0);
        if ((parameter.thisTansform.position - parameter.player.position).magnitude <= parameter.MAX_VISION_DISTANCE)
        {
            float angleBetweenPlayerAndEnemy = Vector3.Angle(parameter.thisTansform.forward, parameter.player.position - parameter.thisTansform.position);
            if (angleBetweenPlayerAndEnemy <= 30 && angleBetweenPlayerAndEnemy >= 0)
            {
                //Ray ray = new Ray(parameter.thisTansform.position, parameter.player.position - parameter.thisTansform.position);
                //Debug.DrawLine(parameter.thisTansform.position, parameter.player.position, Color.red);
                Debug.DrawRay(parameter.thisTansform.position, parameter.player.position - parameter.thisTansform.position, Color.red);
                RaycastHit hitInfo;
                if (Physics.Raycast(parameter.thisTansform.position + verticalOffset, parameter.player.position - parameter.thisTansform.position + verticalOffset, out hitInfo))
                {
                    if (hitInfo.transform.gameObject.tag == "Player")
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public void UpdateHitEvent(HitEvent hitEvent)
    {
        int damage = hitEvent.Damage;
    }

    public void Damaged(HitEvent hitEvent)
    {
        //canHit=false;
        UpdateHealth(hitEvent.Damage);
        Debug.Log(hitEvent.SkillName+"hit me!");
        
        
        if(parameter.health<=0)
        {
            TransitionState(StateType.Dead);
        }
        else
        {
            //transform.DOMove(parameter.player.TransformPoint(parameter.player.localPosition+hitEvent.m_hitOffset),0.5f).SetRelative(true);
            TransitionState(StateType.Damage);
        }
    }

    public void UpdateHealth(int dmg)
    {
        parameter.health-=dmg;
        Debug.Log(parameter.health);
    }


    public void OnAnimationEnd(string stateName, UnityAction<StateType> callback)
    {
        currAnimInfo = parameter.anim.GetCurrentAnimatorStateInfo(0);

        if (currAnimInfo.IsName(stateName)&&currAnimInfo.normalizedTime>1.0f)
        {
            callback.Invoke(StateType.Battle);
        }
    }

    private void OnAnimatorIK(int layerIndex)
    {
        parameter.anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot, 1.0f);
        parameter.anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
        parameter.anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
        parameter.anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
    }


   // private void OnGUI()
   // {
//#if UNITY_EDITOR
     //   GUI.Box(new Rect(Camera.main.WorldToScreenPoint(transform.position).x, Camera.main.WorldToScreenPoint(transform.position).y, 100, 30), currentState.ToString());
//#endif
   // }

}
