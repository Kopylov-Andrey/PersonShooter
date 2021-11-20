using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMovement : MonoBehaviour
{
    [SerializeField] private CharacterController characterController; // ���������� ���������

    [Header("Movement")]

    [SerializeField] private float RifleRunSpeed;// �������� ���� � ������� ���������
    [SerializeField] private float RifleSprintSpeed;// �������� �������
    [SerializeField] private float AimingWalkSpeed;// �������� ������ � �������������
    [SerializeField] private float AimingRunSpeed;// �������� ���� � �������������
    [SerializeField] private float CrouchSpeed;//  ������
    [SerializeField] private float jumpSpeed; // ���� ������ 
    [SerializeField] private float AccelerationRate; // ���������

    [Header("State")]
    [SerializeField] private float crouhHeight; // ������ ����������

    private bool isAiming;// �������������
    private bool isJump;// �������
    private bool isCrouch;// ���������
    private bool isSprint;// �����
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
    private Vector3 Movementderection; //����������� ��������

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
        characterController.center = new Vector3(0, BaseCharacterHeightOffset / 2, 0);//������ ���������� � ��������� �������
    }
    public void UnCrouch()
    {
        isCrouch = false;
        characterController.height = BaseCharacterHeight;
        characterController.center = new Vector3(0, BaseCharacterHeightOffset, 0);//������ ���������� � ��������� �������
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

        if (IsGrounded == true)// ���� �� ����e, ������
        {
            Movementderection = DirectionControl * GetCurrentSpeedByState();// �������� ������ * �����������
            if (isJump == true)
            {
                Movementderection.y = jumpSpeed;
                isJump = false;
            }

            Movementderection = transform.TransformDirection(Movementderection);
        }
        if(onStairs == false)
        Movementderection += Physics.gravity * Time.deltaTime; // ���������� ���������� 

        characterController.Move(Movementderection * Time.deltaTime);// �������� ������
    }

    
    #endregion

    public float GetCurrentSpeedByState() // ������� �������� ��� �������� ���������
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
