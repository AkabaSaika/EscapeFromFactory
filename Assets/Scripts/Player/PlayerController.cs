using System.Collections;
using System.Collections.Generic;
using System;
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

    private bool isMoving;

    private CharacterController cc;
    private Transform mainCamera;
    private CameraController cameraController;

    private CollisionFlags cf;
 
    [SerializeField]
    private bool isGrounded;
    private bool canCancel = false;

    [SerializeField]
    public bool canTurn;

    private int speedZID = Animator.StringToHash("SpeedZ");
    private int speedRotateID = Animator.StringToHash("SpeedRotate");
    private int speedHID = Animator.StringToHash("SpeedH");
    private int speedVID = Animator.StringToHash("SpeedV");
    private int hitId = Animator.StringToHash("Hit");
    private int deadId = Animator.StringToHash("IsDead");
    private int JumpId = Animator.StringToHash("Jump");
    private int LandId = Animator.StringToHash("Land");
    
    private Animator anim;
    private WarriorAnimsFREE.IKHands IKHands;

    public float CurrentHp { get => currentHp; set => currentHp = value; }

    [TextArea]
    public string currentState;

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
        IKHands = GetComponent<WarriorAnimsFREE.IKHands>();

        speed = parameter.WalkSpeed;
        mouseSensiticity = 2.4f;
        angleY = transform.eulerAngles.y;
        mainCamera = Camera.main.transform;
        cameraController = mainCamera.GetComponent<CameraController>();
        angleX = mainCamera.eulerAngles.x;
        Cursor.visible = false;
        canTurn = true;
        //Cursor.lockState = CursorLockMode.Locked;
        isGrounded = groundState;   
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            //Jump();
            isMoving = Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0;
        }
    }

    private void Update()
    {
        currentState = anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree").ToString();
        if(!isDead)
        {
            camFoward = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
            
            if(Input.GetKeyDown(KeyCode.Space))
            {
                cameraController.LockOn();
            }
            if (cameraController.IsLockOn)
            {
                MoveWhileLock();
            }
            else
            {
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Blend Tree"))
                { Turn(); }
                Move();
            }


            //特定のアニメションを再生する時にIKの適用状態を調整する
            if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("great sword idle (5)"))
            {
                IKHands.SetIKOff();
            }
            else
            {
                IKHands.SetIKOn();
            }

            //ゲームクリアの判定処理
            RaycastHit hit;
            if(Physics.Linecast(gameObject.transform.position,gameObject.transform.position+new Vector3(0,-0.5f,0),out hit,1<<10))
            {
                if(hit.collider.gameObject.name=="GameClear")
                {
                    //GameObject.Find("Game").GetComponent<GameManager>().clear();
                    GameManager.Instance.gameClearHandler.Invoke();
                }
            }   
        }
    }

    private void Move()
    {
        Vector3 lookAtPoint = new Vector3(h, 0, v);
        speed = Input.GetButton("Run") ? parameter.RunSpeed : parameter.WalkSpeed;
        //transform.Translate(camFoward * v * speed * Time.deltaTime,Space.World);
        //transform.Translate(mainCamera.right * h * speed * Time.deltaTime,Space.World);
        
        anim.SetFloat(speedVID, Convert.ToSingle(isMoving) * speed);
    }

    private void MoveWhileLock()
    {
        speed = Input.GetButton("Run") ? parameter.RunSpeed : parameter.WalkSpeed;
        anim.SetFloat(speedHID, h * speed);
        anim.SetFloat(speedVID, v * speed);
    }

    private void Jump()
    {
        if (Input.GetButton("Jump") && isGrounded)
        {
            Debug.Log(Input.GetButton("Jump"));
            jumpSpeed = parameter.JumpSpeed;
            isGrounded = false;
            anim.SetTrigger(JumpId);
        }
        if (!isGrounded)
        {
            jumpSpeed -= 10 * Time.deltaTime;
            Vector3 jump = new Vector3(0, jumpSpeed * Time.deltaTime, 0);
            cf = cc.Move(jump);
            if (cf == CollisionFlags.Below)
            {
                anim.SetTrigger(LandId);
                jumpSpeed = 0;
                isGrounded = true;   
            }
        }
        if (isGrounded && cf == CollisionFlags.None)
        {
            isGrounded = false;
        }
    }

    public void Damaged(HitEvent hitEvent)
    {
        anim.SetTrigger(hitId);

        if(CurrentHp<=0)
        {
            AudioManager.Instance.EffectPlay("Audio/Voice/univ1077",false); //死亡ボイスを再生

            gameObject.GetComponent<CharacterController>().enabled = false;
            isDead = true;
            anim.SetTrigger(deadId);
        }
        else
        {
            //受撃ボイスを再生
            int i = UnityEngine.Random.Range(0, 4);
            string path = "Audio/Voice/univ" + (1091 + i).ToString();
            AudioManager.Instance.EffectPlay(path, false);

            CurrentHp -= hitEvent.Damage; //ライフポイントを更新
        }

        Debug.Log("hit by" + hitEvent.SkillName);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            DoorController dc = other.GetComponentInParent<DoorController>();
            dc.doorChange += delegate { DoorController.OpenDoor(dc.door); };
            dc.doorChange(dc.door);
            
        }
        Debug.Log(other.name);
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Door"))
        {
            DoorController dc = other.GetComponentInParent<DoorController>();
            dc.doorChange += delegate { DoorController.CloseDoor(dc.door); };
            dc.doorChange(dc.door);

        }
    }

    private void PickUp(int id)
    {
        
    }

    /// <summary>
    /// ロックオンしていない場合
    /// </summary>
    public void Turn()
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
