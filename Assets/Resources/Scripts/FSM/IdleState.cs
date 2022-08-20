using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class IdleState : IState
{
    private FSM manager;
    private Parameter parameter;

    public IdleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Idle");
        parameter.idleTimer = 0;
        parameter.anim.Play("Idle");
    }

    public void OnUpdate()
    {      
        Debug.Log("Update Idle");
        if (manager.DetectPlayer())
        {
            manager.TransitionState(StateType.Chase);
        }

        parameter.idleTimer += Time.deltaTime;
        if(parameter.idleTimer>=parameter.idleTime)
        {
            manager.TransitionState(StateType.Patrol);
        }
    }

    public void OnExit()
    {
        Debug.Log("Exit Idle");
    }
}

public class PatrolState : IState
{
    private FSM manager;
    private Parameter parameter;
    public int patrolPointIndex = 0;

    public PatrolState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Patrol");
        parameter.anim.Play("Walk");
        parameter.agent.speed = parameter.moveSpeed;
    }

    public void OnUpdate()
    {
        if(patrolPointIndex>parameter.patrolPoints.Length - 1)
        {
            patrolPointIndex = 0;
        }
        parameter.agent.SetDestination(parameter.patrolPoints[patrolPointIndex].position);
        if(Vector3.Distance(parameter.patrolPoints[patrolPointIndex].position,parameter.thisTansform.position)<=0.1f)
        {
            patrolPointIndex++;
            manager.TransitionState(StateType.Idle);
        }
        Debug.Log("Update Patrol");
    }

    public void OnExit()
    {

    }
}

public class AttackState : MonoBehaviour,IState
{
    private FSM manager;
    private Parameter parameter;
    private UnityAction<StateType> action;
 
    public AttackState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Attack");
        parameter.anim.Play("Attack1");
        action += manager.TransitionState;
    }

    public void OnUpdate()
    {
        
        Debug.Log("Update Attack");
        manager.OnAnimationEnd("Attack2", action);
        //StartCoroutine(OnAnimationEnd("Attack2",action));
        

    }

    public void OnExit()
    {
        Debug.Log("Exit Attack");
    }
}


/// <summary>
/// 
/// </summary>
public class ChaseState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ChaseState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Chase");
        parameter.anim.Play("Walk");
        parameter.agent.speed = parameter.chaseSpeed;
    }

    public void OnUpdate()
    {
        Debug.Log("Update Chase");
        if(Vector3.Distance(parameter.thisTansform.position,parameter.player.position)>parameter.MAX_ATTACK_DISTANCE)
        {
            parameter.agent.SetDestination(parameter.player.position);
        }
        else
        {
            parameter.agent.speed = 0;
            //parameter.rb.angularVelocity = Vector3.zero;
            //parameter.rb.velocity = Vector3.zero;
            parameter.anim.Play("Idle");
            manager.TransitionState(StateType.Battle);
        }
        
    }

    public void OnExit()
    {
        Debug.Log("Exit Chase");
    }
}

public class ReactState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ReactState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter React");
    }

    public void OnUpdate()
    {
        Debug.Log("Update React");
    }

    public void OnExit()
    {

    }
}

/// <summary>
/// 
/// </summary>
public class BattleState :IState
{
    private FSM manager;
    private Parameter parameter;

    public BattleState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        Debug.Log("Enter Battle");
        parameter.anim.Play("FightIdle");
    }

    public void OnUpdate()
    {
        Debug.Log("Update Battle");
        parameter.thisTansform.LookAt(parameter.player);
        manager.TransitionState(StateType.Attack);
    }

    public void OnExit()
    {
        Debug.Log("Exit Battle");
    }
}

/// <summary>
/// 
/// </summary>
public class DamageState :IState
{
    private FSM manager;
    private Parameter parameter;

    public DamageState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Big Hit To Head");
        Debug.Log("Enter Damage");
    }

    public void OnUpdate()
    {
        if(parameter.anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1.0f)
        {
            if (parameter.health <= 0)
            {
                manager.TransitionState(StateType.Dead);
            }
            else
            {
                manager.TransitionState(StateType.Battle);
            }
        }

        Debug.Log("Update Damage");
    }

    public void OnExit()
    {
        Debug.Log("Exit Damage");
    }
}

public class DeadState :IState
{
    private FSM manager;
    private Parameter parameter;
    private GameObject thisObject;

    public DeadState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Falling Back Death");
        Debug.Log("Enter Dead");
    }

    public void OnUpdate()
    {
        Debug.Log("Update Dead");
    }

    public void OnExit()
    {
        Debug.Log("Exit Dead");
    }
}