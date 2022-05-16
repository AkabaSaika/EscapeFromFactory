using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel : MonoBehaviour
{
    private float maxHp = 100.0f;
    private float currentHp = 100.0f;
    private float maxStamina = 100.0f;
    private float currentStamina = 100.0f;
    private float attackPoint = 10.0f;
    private float defendPoint = 10.0f;
    private float walkSpeed = 3.0f;
    private float runSpeed = 10.0f;
    private float jumpSpeed = 10.0f;
    private bool groundState = false;

    public float MaxHp { get => maxHp; set => maxHp = value; }
    public float CurrentHp { get => currentHp; set => currentHp = value; }
    public float MaxStamina { get => maxStamina; set => maxStamina = value; }
    public float CurrentStamina { get => currentStamina; set => currentStamina = value; }
    public float AttackPoint { get => attackPoint; set => attackPoint = value; }
    public float DefendPoint { get => defendPoint; set => defendPoint = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public bool GroundState { get => groundState; set => groundState = value; }
    public float JumpSpeed { get => jumpSpeed; set => jumpSpeed = value; }
}
