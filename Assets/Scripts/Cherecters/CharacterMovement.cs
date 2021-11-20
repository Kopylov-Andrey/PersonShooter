using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController; // котрроллер колайдера

    [Header("Movement")]

    [SerializeField] private float RifleRunSpeed;// скорость бега в обычном состоянии
    [SerializeField] private float RifleSprintSpeed;// скорость спринта
    [SerializeField] private float AimingWalkSpeed;// скорость ходьбы с прицеливанием
    [SerializeField] private float AimingRunSpeed;// скорость бега с прицеливанием
    [SerializeField] private float CrouchSpeed;//  присед
    [SerializeField] private float jumpSpeed; // сила прыжка 
    [SerializeField] private float AccelerationRate; // ускорение

    [Header("State")]
    [SerializeField] private float crouhHeight; // высота коллайдера

    private bool isAiming;// приуеливаемся
    private bool isJump;// прыгаем
    private bool isCrouch;// приседаем
    private bool isSprint;// бежим
    private bool onStairs;
    private float distanceToGround;
    


    // public
    [HideInInspector]
    public Vector3 TargetDirectionConrol;


    public float JumpSpeed => jumpSpeed;
    public float CrouhHeight => crouhHeight;
    public bool IsAiming => isAiming;
    public bool IsJump => isJump;
    public bool IsCrouch => isCrouch;
    public bool IsSprint => isSprint;
    public float DistanceToGround => distanceToGround;
    public bool IsGrounded => distanceToGround < 0.01f;
    public bool UpdatePosition = true;
    public float CurrentSpeed => GetCurrentSpeedByState();



    // private

    private Vector3 DirectionControl;
    private Vector3 Movementderection; //направление движения

    private float BaseCharacterHeight;
    private float BaseCharacterHeightOffset;



    #region UNITY EVENT
    private void Start()
    {
        BaseCharacterHeight = characterController.height;
        BaseCharacterHeightOffset = characterController.center.y;
    }

    void Update()
    {
        Move();

        UpdateDistanceToGround();
    }

    #endregion


    #region CHARACTER STATUS

    public void Sprint()
    {
        if (characterController.isGrounded == false) return;

        if (isCrouch == true) return;

        isSprint = true;
    }
    public void UnSprint()
    {
        isSprint = false;
    }
    public void Aiming()
    {
        isAiming = true;
    }
    public void UnAiming()
    {
        isAiming = false;
    }
    public void Jump()
    {
        if (characterController.isGrounded == false) return;

        isJump = true;
    }
    public void Crouch()
    {
        if (characterController.isGrounded == false) return;
        if (IsSprint == true) return;

        isCrouch = true;
        characterController.height = crouhHeight;
        characterController.center = new Vector3(0, BaseCharacterHeightOffset / 2, 0);//размер коллайдера в состояние приседа
    }
    public void UnCrouch()
    {
        isCrouch = false;
        characterController.height = BaseCharacterHeight;
        characterController.center = new Vector3(0, BaseCharacterHeightOffset, 0);//размер коллайдера в состояние приседа
    }
    public void TurnOffGravity()
    {
        Movementderection += Physics.gravity * Time.deltaTime * 0;
        onStairs = true;
    }
    public void TurnOnGravity()
    {
       
        onStairs = false;
    }
    private void Move()
    {
        DirectionControl = Vector3.MoveTowards(DirectionControl, TargetDirectionConrol, Time.deltaTime * AccelerationRate);

        if (IsGrounded == true)// если на землe, прыжок
        {
            Movementderection = DirectionControl * GetCurrentSpeedByState();// скорость игрока * направление
            if (isJump == true)
            {
                Movementderection.y = jumpSpeed;
                isJump = false;
            }

            Movementderection = transform.TransformDirection(Movementderection);
        }
        if(onStairs == false)
        Movementderection += Physics.gravity * Time.deltaTime; // добавление гравитации 

        characterController.Move(Movementderection * Time.deltaTime);// движение игрока
    }

    
    #endregion

    public float GetCurrentSpeedByState() // вернуть скорость для текущего состояния
    {
        if (isCrouch)
        {
            return CrouchSpeed;
        }
        if (IsAiming)
        {
            if (isSprint)
                return AimingRunSpeed;
            else
                return AimingWalkSpeed;           
        }
        if (IsAiming == false)
        {
            if (isSprint)
                return RifleSprintSpeed;
            else
                return RifleRunSpeed;
        }


        return RifleRunSpeed;
    }



    private void UpdateDistanceToGround()
    {
        RaycastHit hit;

        if (Physics.Raycast(transform.position, -Vector3.up, out hit, 10000) == true)
        {
            distanceToGround = Vector3.Distance(transform.position, hit.point);
        }
    }


}
