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
    private float walkSpeed;
    private float runSpeed;
    private float jumpSpeed;
    private float maxTenacity;
    

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
    public float MaxTenacity { get => maxTenacity; set => maxTenacity = value; }
}

public class PlayerController : MonoBehaviour
{
    //プレイヤーパラメータのオブジェクト
    public PlayerParameter parameter = new PlayerParameter();
    //シリアス化されたパラメータ数値を格納するオブジェクト
    private cfg.character.Character playerParameterData;
    //プレイヤーの移動速度
    [SerializeField]
    private float speed;
    //プレイヤー現在のライフポイント
    [SerializeField]
    private float currentHp;
    //プレイヤー現在の強靭度
    [SerializeField]
    private float currentTenacity;
    //方向入力の水平軸
    private float h;
    //方向入力の垂直軸
    private float v;
    //プレイヤー移動状態のフラッグ
    private bool isMoving;
    //プレイヤー死亡状態のフラッグ
    private bool isDead = false;
    //メインカメラのTransform
    private Transform mainCamera;
    //カメラの前方向
    private Vector3 camFoward;
    //メインカメラのコントローラーのコムポーネント
    private CameraController cameraController;
    //プレイヤーのアニメーターのコムポーネント
    private Animator anim;

    private int speedHID = Animator.StringToHash("SpeedH");
    private int speedVID = Animator.StringToHash("SpeedV");
    private int hitId = Animator.StringToHash("Hit");
    private int deadId = Animator.StringToHash("IsDead");
    private int blowId = Animator.StringToHash("Blow");
    private int normalId = Animator.StringToHash("Normal");
    private int greatSwordId = Animator.StringToHash("GreatSword");
    private int katanaId = Animator.StringToHash("Katana");
    private int sheathingId = Animator.StringToHash("Sheathing");
    
    
    [SerializeField]
    private WarriorAnimsFREE.IKHands IKHands;

    public float CurrentHp { get => currentHp; set => currentHp = value; }



    private void Awake()
    {
        playerParameterData = TablesSingLeton.Instance.Tables.TbCharacter.Get(1001);
        parameter.MaxHp = playerParameterData.MaxHp;
        parameter.WalkSpeed = playerParameterData.WalkSpeed;
        parameter.RunSpeed = playerParameterData.Runspeed;
        parameter.JumpSpeed = playerParameterData.JumpSpeed;
        parameter.MaxTenacity = playerParameterData.MaxTenacity;
        CurrentHp = parameter.MaxHp;
        currentTenacity = parameter.MaxTenacity;
    }

    void Start()
    {
        anim = gameObject.GetComponent<Animator>();
        speed = parameter.WalkSpeed;
        mainCamera = Camera.main.transform;
        cameraController = mainCamera.GetComponent<CameraController>();
        Cursor.visible = false;
        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        if(!isDead)
        {
            h = Input.GetAxis("Horizontal");
            v = Input.GetAxis("Vertical");
            isMoving = Mathf.Abs(h) > 0 || Mathf.Abs(v) > 0;
        }
    }

    private void Update()
    {
        if(!isDead)
        {
            
            camFoward = Vector3.ProjectOnPlane(mainCamera.forward, Vector3.up);
            SwitchWeapon();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                cameraController.LockOn();
            }
            if (cameraController.IsLockOn)
            {
                MoveWhileLock();
            }
            else
            {
                if(anim.GetCurrentAnimatorStateInfo(0).IsName("Great Sword")||anim.GetCurrentAnimatorStateInfo(0).IsName("Normal")||anim.GetCurrentAnimatorStateInfo(0).IsName("Katana"))
                { Turn(); }
                Move();
            }

            if (gameObject.GetComponent<WarriorAnimsFREE.IKHands>())
            {//特定のアニメションを再生する時にIKの適用状態を調整する
                
                if (anim.GetCurrentAnimatorClipInfo(0)[0].clip.name.Equals("great sword idle (5)"))
                {
                    IKHands.SetIKOff();
                }
                else
                {
                    IKHands.SetIKOn();
                }
            }
  
            if(currentTenacity<parameter.MaxTenacity)
            {
                StartCoroutine("RecoverTenacity");
            }
            else
            {
                StopCoroutine("RecoverTenacity");
            }
        }
    }
    /// <summary>
    /// 移動処理
    /// </summary>
    private void Move()
    {
        //入力に応じてキャラクターの向きを調整する
        Vector3 lookAtPoint = new Vector3(h, 0, v);
        //走る状態に応じてspeed値を決める
        speed = Input.GetButton("Run") ? parameter.RunSpeed : parameter.WalkSpeed;
        //speed値をアニメーターに渡す
        anim.SetFloat(speedVID, Convert.ToSingle(isMoving) * speed);
    }
    /// <summary>
    /// 敵をロックオンしている場合の移動処理
    /// </summary>
    private void MoveWhileLock()
    {
        speed = Input.GetButton("Run") ? parameter.RunSpeed : parameter.WalkSpeed;
        anim.SetFloat(speedHID, h * speed);
        anim.SetFloat(speedVID, v * speed);
    }
    /// <summary>
    /// ダメージを受けた場合の処理
    /// </summary>
    /// <param name="hitEvent">ダメージの元から渡されたHitEventオブジェクト</param>
    public void Damaged(HitEvent hitEvent)
    {
        //ライフポイントを更新
        CurrentHp -= hitEvent.Damage; 
        //強靭度を減る
        currentTenacity -= 15;
        //死亡時の処理
        if (CurrentHp<=0)
        {
            //死亡ボイスを再生
            AudioManager.Instance.EffectPlay("Audio/Voice/univ1077",false); 
            //キャラクターを操作不可にする
            gameObject.GetComponent<Rigidbody>().useGravity=false;
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            //死亡フラグをtrueにする
            isDead = true;
            //死亡アニメーションを再生
            anim.SetTrigger(deadId);
        }
        //死亡していない場合の処理
        else
        {
            //ランダムに受撃ボイスを再生
            int i = UnityEngine.Random.Range(0, 4);
            string path = "Audio/Voice/univ" + (1091 + i).ToString();
            AudioManager.Instance.EffectPlay(path, false);
            //爆発に飛ばされた時の処理
            if(hitEvent.SkillName=="Explosion")
            {
                //飛ばされたアニメーションを再生
                anim.SetTrigger(blowId);
            }
            //通常攻撃を受け、強靭度が0以下になる場合の処理
            else if(currentTenacity<=0)
            {
                //受撃アニメーションを再生
                anim.SetTrigger(hitId);
                //強靭度を最大値に回復する
                currentTenacity = parameter.MaxTenacity;
            }
        }

        Debug.Log("hit by" + hitEvent.SkillName);
    }

    private void PickUp(int id)
    {
        
    }

    /// <summary>
    /// ロックオンしていない場合の転向処理
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

    /// <summary>
    /// 足のIK処理
    /// </summary>
    /// <param name="layerIndex"></param>
    private void OnAnimatorIK(int layerIndex)
    {
        anim.SetIKPositionWeight(AvatarIKGoal.LeftFoot,1.0f);
        anim.SetIKPositionWeight(AvatarIKGoal.RightFoot, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.LeftFoot, 1.0f);
        anim.SetIKRotationWeight(AvatarIKGoal.RightFoot, 1.0f);
    }
    /// <summary>
    /// 武器スイッチの処理
    /// </summary>
    private void SwitchWeapon()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1) && !gameObject.GetComponent<HeavyFullMetalSword>()&&!gameObject.GetComponent<WarriorAnimsFREE.IKHands>())
        {
            IKHands = gameObject.AddComponent<WarriorAnimsFREE.IKHands>();
            IKHands.canBeUsed = true;
            anim.SetTrigger(sheathingId);
            anim.SetBool(greatSwordId,true);
            anim.SetBool(katanaId, false);
            anim.SetBool(normalId, false);
        }
        if(Input.GetKeyDown(KeyCode.Alpha3))
        {
            Destroy(gameObject.GetComponent<WarriorAnimsFREE.IKHands>());
            anim.SetTrigger(sheathingId);
            anim.SetBool(greatSwordId, false);
            anim.SetBool(katanaId, false);
            anim.SetBool(normalId, true);
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)&&!gameObject.GetComponent<Katana>())
        {
            Destroy(gameObject.GetComponent<WarriorAnimsFREE.IKHands>());
            anim.SetTrigger(sheathingId);
            anim.SetBool(greatSwordId, false);
            anim.SetBool(normalId, false);
            anim.SetBool(katanaId, true);
        }
    }
    /// <summary>
    /// 強靭度が自動的に回復する
    /// </summary>
    /// <returns></returns>
    private IEnumerator RecoverTenacity()
    {
        currentTenacity += 0.01f;
        yield return new WaitForSeconds(0.1f);
    }


    private void OnGUI()
    {
#if UNITY_EDITOR
        GUI.Box(new Rect(100, 100, 50, 30), anim.speed.ToString());
#endif
    }
    public void GetHeal(int healPoint)
    {
        if(currentHp<parameter.MaxHp-0.1)
        {
            AudioManager.Instance.EffectPlay("Audio/Voice/univ0006",false);
        }
        currentHp+=healPoint;
        currentHp=currentHp>parameter.MaxHp?parameter.MaxHp:currentHp;
    }
}
