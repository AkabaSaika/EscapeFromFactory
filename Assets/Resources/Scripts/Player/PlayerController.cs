using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Bright.Serialization;
using cfg;

public class PlayerParameter
{
    private float maxHp;
    private float maxStamina;
    //private float defendPoint = 10.0f;
    private float walkSpeed;
    private float runSpeed;
    private float jumpSpeed;
    

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    //public float DefendPoint { get => defendPoint; set => defendPoint = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
}

public class PlayerController : MonoBehaviour
{
    private PlayerParameter parameter = new PlayerParameter();
    private cfg.character.Character playerParameterData;
    [SerializeField]
    private float speed;
    [SerializeField]
    private float currentHp;
    [SerializeField]
    private float jumpSpeed;
    private float mouseSensiticity;
    private float angleY;
    private float angleX;
    [SerializeField]
    private bool groundState = false;
    private bool isDead = false;
    private Vector3 camFoward;

    private float h;
    private float v;

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
    private int hitId = Animator.StringToHash("Hit");
    private int deadId = Animator.StringToHash("IsDead");
    
    private Animator anim;

    public float CurrentHp { get => currentHp; set => currentHp = value; }

    private void Awake()
    {
        playerParameterData = TablesSingLeton.Instance.Tables.TbCharacter.Get(1001);
        parameter.MaxHp = playerParameterData.MaxHp;
        parameter.WalkSpeed = playerParameterData.WalkSpeed;
        parameter.RunSpeed = playerParameterData.Runspeed;
        parameter.JumpSpeed = playerParameterData.JumpSpeed;
        CurrentHp = parameter.MaxHp;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        speed = parameter.WalkSpeed;
        mouseSensiticity = 2.4f;
        angleY = transform.eulerAngles.y;
        mainCamera = Camera.main.transform;
        angleX = mainCamera.eulerAngles.x;
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
        isGrounded = groundState;   
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            Jump();
        }
    }

    private void Update()
    {
        if(!isDead)
        {
            camFoward = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
            Move();
        }
    }

    private void LateUpdate()
    {
        if(!isDead)
        {
            Turn();
        }
    }

    private void Move()
    {
        Vector3 lookAtPoint = new Vector3(h, 0, v);
        speed = Input.GetButton("Run") ? parameter.RunSpeed : parameter.WalkSpeed;
        transform.Translate(camFoward * v * speed * Time.deltaTime,Space.World);
        transform.Translate(mainCamera.right * h * speed * Time.deltaTime,Space.World);
        anim.SetFloat(speedHID, Mathf.Abs(h) * speed);
        anim.SetFloat(speedVID, Mathf.Abs(v) * speed);
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            Debug.Log(Input.GetButton("Jump"));
            jumpSpeed = parameter.JumpSpeed;
            groundState = false;
            isGrounded = groundState;
        }
        if (!isGrounded)
        {
            jumpSpeed -= 10 * Time.deltaTime;
            Vector3 jump = new Vector3(0, jumpSpeed * Time.deltaTime, 0);
            cf = cc.Move(jump);
            if (cf == CollisionFlags.Below)
            {
                jumpSpeed = 0;
                groundState = true;
                isGrounded = groundState;
            }
        }
        if (isGrounded && cf == CollisionFlags.None)
        {
            groundState = false;
            isGrounded = groundState;
        }
    }

    public void Damaged(HitEvent hitEvent)
    {
        anim.SetTrigger(hitId);
        if(CurrentHp>0)
        {
            //受撃ボイスを再生
            int i = Random.Range(0, 4);
            string path = "Audio/Voice/univ" + (1091 + i).ToString();
            AudioManager.EffectPlay(path, false);

            CurrentHp -= hitEvent.Damage; //ライフポイントを更新
        }
        else
        {
            AudioManager.EffectPlay("Audio/Voice/univ1077",false); //死亡ボイスを再生

            gameObject.GetComponent<CharacterController>().enabled = false;
            isDead = true;
            anim.SetTrigger(deadId);
        }
        
        Debug.Log("hit by" + hitEvent.SkillName);
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

    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1.0f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
    }
}
