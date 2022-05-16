﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float speed;
    [SerializeField]
    private float jumpSpeed;
    private float mouseSensiticity;
    private float angleY;
    private float angleX;


    private float h;
    private float v;

    private PlayerModel pm;
    private CharacterController cc;
    private Transform mainCamera;

    private CollisionFlags cf;
 
    [SerializeField]
    private bool isGrounded;
    private bool canCancel = false;

    private int speedZID = Animator.StringToHash("SpeedZ");
    private int speedRotateID = Animator.StringToHash("SpeedRotate");
    private int speedHID = Animator.StringToHash("SpeedH");
    private int speedVID = Animator.StringToHash("SpeedV");
    private int attack1ID = Animator.StringToHash("Attack1");
    private int attack2ID = Animator.StringToHash("Attack2");
    private int attack3ID = Animator.StringToHash("Attack3");
    private int cancelID = Animator.StringToHash("CanCancel");
    private Animator anim;

    private AnimationEvent attack1Combo = new AnimationEvent();

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        pm = new PlayerModel();
        cc = GetComponent<CharacterController>();
        speed = pm.WalkSpeed;
        mouseSensiticity = 2.4f;
        angleY = transform.eulerAngles.y;
        mainCamera = Camera.main.transform;
        angleX = mainCamera.eulerAngles.x;
        //Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        isGrounded = pm.GroundState;

        AddAnimationEvent();
    }

    private void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Jump();
        
    }

    private void Update()
    {
        Turn();
        Move();
        anim.SetFloat(speedHID,Mathf.Abs(h) * speed);
        anim.SetFloat(speedVID, Mathf.Abs(v) * speed);
        Attack();

        //Debug.Log(canCancel);
    }

    private void Move()
    {
        Vector3 lookAtPoint = new Vector3(h, 0, v);
        //transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(Vector3.ProjectOnPlane( Camera.main.transform.forward,Vector3.up)), 15f * Time.deltaTime);
        speed = Input.GetButton("Run") ? pm.RunSpeed : pm.WalkSpeed;
        transform.Translate(transform.forward * v * speed * Time.deltaTime,Space.World);
        //transform.Translate(Vector3.ProjectOnPlane(Camera.main.transform.forward,Vector3.up) * v * speed * Time.deltaTime, Space.World);
        transform.Translate(transform.right * h * speed * Time.deltaTime,Space.World);

        //transform.LookAt(transform.forward + lookAtPoint);
    }


    private void Turn()
    {
        Vector3 camFoward = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
        transform.localRotation = Quaternion.Slerp(transform.rotation,Quaternion.Euler(new Vector3(0,mainCamera.rotation.eulerAngles.y,0)), Time.time*0.1f);
    }

    private void Jump()
    {

        if (Input.GetButton("Jump") && isGrounded)
        {
            Debug.Log(Input.GetButton("Jump"));
            jumpSpeed = pm.JumpSpeed;
            pm.GroundState = false;
            isGrounded = pm.GroundState;
        }
        if (!isGrounded)
        {
            jumpSpeed -= 10 * Time.deltaTime;
            Vector3 jump = new Vector3(0, jumpSpeed * Time.deltaTime, 0);
            cf = cc.Move(jump);
            if (cf == CollisionFlags.Below)
            {
                jumpSpeed = 0;
                pm.GroundState = true;
                isGrounded = pm.GroundState;
            }
        }
        if (isGrounded && cf == CollisionFlags.None)
        {
            pm.GroundState = false;
            isGrounded = pm.GroundState;
        }
    }

    private void Attack()
    {
        if (Input.GetMouseButtonDown(0) && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2") && !anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3")) 
        {
            anim.SetTrigger(attack1ID);
        }
        if(Input.GetMouseButtonDown(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack1"))
        {
            anim.SetTrigger(attack2ID);
        }
        if(Input.GetMouseButtonDown(0)&&anim.GetCurrentAnimatorStateInfo(0).IsName("Attack2"))
        {
            anim.SetTrigger(attack3ID);
            //Debug.Log(anim.GetCurrentAnimatorStateInfo(0).IsName("Attack3"));
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("item"))
        {
            SendMessage("PickUp", int.Parse(other.name));
        }
    }

    private void PickUp(int id)
    {
        
    }

    private void CancelState()
    {
        anim.SetBool(cancelID, true);
    }

    /// <summary>
    /// キャンセルのタイミングを設置する
    /// </summary>
    private void AddAnimationEvent()
    {
        AnimationClip[] clips = anim.runtimeAnimatorController.animationClips;
        foreach(var clip in clips)
        {
            if(string.Equals(clip.name,"great sword slash"))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName = "CancelState";
                events.time = 1.0f;
                clip.AddEvent(events);
            }
            if(string.Equals(clip.name,"great sword slash (3)"))
            {
                AnimationEvent events = new AnimationEvent();
                events.functionName = "CancelState";
                events.time = 1.0f;
                clip.AddEvent(events);
            }
        }
    }
}