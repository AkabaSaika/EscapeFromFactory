using System;
using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.AI;

public enum StateType
{
    Idle,Patrol,Chase,React,Attack,Battle,Damage,Dead
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
    //public Rigidbody rb;

    public float idleTimer;
    public NavMeshAgent agent;

    public float MAX_VISION_DISTANCE;
    public Transform player;
    public float MAX_ATTACK_DISTANCE;

}

public class FSM : MonoBehaviour
{
    public Parameter parameter;
    private IState currentState;
    private Dictionary<StateType, IState> states = new Dictionary<StateType, IState>();
    private SkillParameter skillParameter;
    private PlayerController player;
    private Animator playerAnim;
    public bool canHit;
    // Start is called before the first frame update
    void Start()
    {
        states.Add(StateType.Idle, new IdleState(this));
        states.Add(StateType.Patrol, new PatrolState(this));
        states.Add(StateType.Chase, new ChaseState(this));
        states.Add(StateType.React, new ReactState(this));
        states.Add(StateType.Attack, new AttackState(this));
        states.Add(StateType.Battle, new BattleState(this));
        states.Add(StateType.Damage,new DamageState(this));
        states.Add(StateType.Dead, new DeadState(this));
        parameter.thisTansform = this.transform;
        parameter.player = GameObject.FindGameObjectWithTag("Player").transform;
        parameter.anim = GetComponent<Animator>();
        parameter.agent = GetComponent<NavMeshAgent>();
        //parameter.rb = GetComponent<Rigidbody>();
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

    public void Damaged()
    {
        canHit=false;
        //string skillName = playerAnim.GetCurrentAnimatorClipInfo(0)[0].clip.name;
        string skillName;
        string tempName;
        Regex skillNamePattern = new Regex("great sword");
        foreach(var i in playerAnim.GetCurrentAnimatorClipInfo(0))
        {
            tempName=i.clip.name;
            Debug.Log(tempName);
            if(skillNamePattern.IsMatch(tempName))
            {
                skillName=tempName;
                //Debug.Log(skillName);
                UpdateHealth(skillParameter.skillDic[skillName]);
            }
        }
        //Debug.Log(skillName);
        
        if(parameter.health<=0)
        {
            TransitionState(StateType.Dead);
        }
        else
        {
            TransitionState(StateType.Damage);
        }
    }

    public void UpdateHealth(int dmg)
    {
        parameter.health-=dmg;
        Debug.Log(parameter.health);
    }
}
