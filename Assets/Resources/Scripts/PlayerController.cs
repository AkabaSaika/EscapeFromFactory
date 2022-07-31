using System.Collections;
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
    [SerializeField]
    
    private Vector3 camFoward;

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
    
    private Animator anim;

    private AnimationEvent attack1Combo = new AnimationEvent();

    private WeaponBehaviour weaponBehaviour;
    //private Weapon weapon;

    

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
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        isGrounded = pm.GroundState;
        //weapon=GetComponent<GreatSword>();
        
    }

    private void FixedUpdate()
    {
        h = Input.GetAxis("Horizontal");
        v = Input.GetAxis("Vertical");
        Jump();
    }

    private void Update()
    {
        camFoward = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
        //if(!weapon.isAttacking)
        //{
            Move();
        //}
        anim.SetFloat(speedHID,Mathf.Abs(h) * speed);
        anim.SetFloat(speedVID, Mathf.Abs(v) * speed);
    }

    private void LateUpdate()
    {
        Turn();
    }

    private void Move()
    {
        Vector3 lookAtPoint = new Vector3(h, 0, v);
        speed = Input.GetButton("Run") ? pm.RunSpeed : pm.WalkSpeed;
        //if(Input.GetKey(KeyCode.W))
        //{
        //    transform.forward=camFoward;
        //}
        //if(Input.GetKey(KeyCode.S))
        //{
        //    transform.forward=-camFoward;
        //}
        //if(Input.GetKey(KeyCode.A))
        //{
        //    transform.forward=mainCamera.right*(-1);
        //}
        //if(Input.GetKey(KeyCode.D))
        //{
        //    transform.forward=mainCamera.right;
        //}
        //if(Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.A))
        //{
        //    transform.forward=camFoward-mainCamera.right;
        //}
        //if(Input.GetKey(KeyCode.W)&&Input.GetKey(KeyCode.D))
        //{
        //    transform.forward=camFoward+mainCamera.right;
        //}
        //if(Input.GetKey(KeyCode.S)&&Input.GetKey(KeyCode.A))
        //{
        //    transform.forward=-camFoward-mainCamera.right;
        //}
        //if(Input.GetKey(KeyCode.S)&&Input.GetKey(KeyCode.D))
        //{
        //    transform.forward=-camFoward+mainCamera.right;
        //}
        transform.Translate(camFoward * v * speed * Time.deltaTime,Space.World);
        transform.Translate(mainCamera.right * h * speed * Time.deltaTime,Space.World);
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

    private void Turn()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.forward = camFoward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.forward = -camFoward;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.forward = mainCamera.right * (-1);
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.forward = mainCamera.right;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.A))
        {
            transform.forward = camFoward - mainCamera.right;
        }
        if (Input.GetKey(KeyCode.W) && Input.GetKey(KeyCode.D))
        {
            transform.forward = camFoward + mainCamera.right;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.A))
        {
            transform.forward = -camFoward - mainCamera.right;
        }
        if (Input.GetKey(KeyCode.S) && Input.GetKey(KeyCode.D))
        {
            transform.forward = -camFoward + mainCamera.right;
        }
    }


}
