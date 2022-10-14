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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Idle");
#endif

        parameter.idleTimer = 0;
        parameter.anim.Play("Idle");
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Idle");
#endif
        if (manager.DetectPlayer())//プレイヤーを検出したらchaseステートに遷移する
        {
            manager.TransitionState(StateType.Chase);
        }

        parameter.idleTimer += Time.deltaTime;
        if(parameter.idleTimer>=parameter.idleTime)
        {
            if (parameter.patrolPoints[0] != null)//パトロールルートがある場合、パトロールステートに遷移する
            {
                manager.TransitionState(StateType.Patrol);
            }
        }
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Idle");
#endif
        
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Patrol");
#endif
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
        if (Vector3.Distance(parameter.patrolPoints[patrolPointIndex].position,parameter.thisTansform.position)<=0.1f)
        {
            patrolPointIndex++;
            manager.TransitionState(StateType.Idle);
        }
        if (manager.DetectPlayer())
        {
            manager.TransitionState(StateType.Chase);
        }

#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Patrol");
#endif
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Attack");
#endif
        parameter.anim.Play("Attack1");
        action += manager.TransitionState;
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Attack");
#endif
        manager.OnAnimationEnd("Attack2", action);
        //StartCoroutine(OnAnimationEnd("Attack2",action));
        if(parameter.anim.GetCurrentAnimatorStateInfo(0).normalizedTime>1.0f&& Vector3.Distance(parameter.thisTansform.position,parameter.player.position)>parameter.MAX_ATTACK_DISTANCE)
        {
            manager.TransitionState(StateType.Chase);
        }

    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Attack");
#endif


        parameter.thisTansform.LookAt(parameter.player.transform);
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Chase");
#endif
        parameter.anim.Play("run");
        parameter.agent.ResetPath();
        
        
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Chase");
#endif
        if (Vector3.Distance(parameter.thisTansform.position, parameter.player.position) > parameter.MAX_CHASE_DISTANCE && parameter.patrolPoints[0] != null)
        {
            manager.TransitionState(StateType.Patrol);
        }
        else if (Vector3.Distance(parameter.thisTansform.position, parameter.player.position) > parameter.MAX_CHASE_DISTANCE && parameter.patrolPoints[0] == null)
        {
            manager.TransitionState(StateType.Return);
        }
        else if (Vector3.Distance(parameter.thisTansform.position, parameter.player.position) > parameter.MAX_ATTACK_DISTANCE)
        {
            parameter.agent.SetDestination(parameter.player.position);
           
            
        }
        else
        {
            parameter.agent.isStopped = true;
            parameter.anim.Play("Idle");
            manager.TransitionState(StateType.Battle);
        }



    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Chase");
#endif
    }
}

public class ReturnState : IState
{
    private FSM manager;
    private Parameter parameter;

    public ReturnState(FSM manager)
    {
        this.manager = manager;
        this.parameter = manager.parameter;
    }

    public void OnEnter()
    {
        parameter.anim.Play("Walk");
        parameter.agent.ResetPath();
        parameter.agent.speed = parameter.moveSpeed;
#if UNITY_EDITOR
        Debug.Log("Enter Return");
#endif
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log("Update Return");
#endif
        //parameter.agent.SetDestination(parameter.respawnPoint);
        parameter.agent.SetDestination(parameter.respawnPoint.position);
        if(Vector3.Distance(parameter.thisTansform.position,parameter.respawnPoint.position)<0.1f)
        {
            manager.TransitionState(StateType.Idle);
        }
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Battle");
#endif
        parameter.anim.Play("FightIdle");
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Battle");
#endif
        parameter.thisTansform.LookAt(parameter.player);
        manager.TransitionState(StateType.Attack);
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Battle");
#endif
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Damage");
#endif
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
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Damage");
#endif
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Damage");
#endif
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
        parameter.thisTansform.gameObject.GetComponent<CharacterController>().enabled = false;
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Enter Dead");
#endif
    }

    public void OnUpdate()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Update Dead");
#endif
    }

    public void OnExit()
    {
#if UNITY_EDITOR
        Debug.Log(manager.gameObject.name + " Exit Dead");
#endif
    }
}