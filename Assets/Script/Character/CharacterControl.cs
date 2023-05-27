using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControl : MonoBehaviour
{
    public Animator rig;
    public float JumpHeight;
    public float gravity;
    public float stepDown;
    public float airControl;
    public float groundSpeed;
    public float pushPower;

    Animator animator;
    CharacterController cc;
    WeaponActivate weaponActivate;
    WeaponReload weaponReload;
    CharacterAiming characterAiming;

    Vector2 input;
    Vector3 rootMotion;
    Vector3 velocity;

    public bool isJumping;

    int isRunningParam = Animator.StringToHash("isRunning");

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        cc = GetComponent<CharacterController>();
        weaponActivate = GetComponent<WeaponActivate>();
        weaponReload = GetComponent<WeaponReload>();
        characterAiming = GetComponent<CharacterAiming>();
    }

    // Update is called once per frame
    void Update()
    {
        input.x = Input.GetAxis("Horizontal");
        input.y = Input.GetAxis("Vertical");

        animator.SetFloat("InputX", input.x);
        animator.SetFloat("InputY", input.y);

        UpdateIsRunning();

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Jump();
        }
    }
    bool IsRunning()
    {
        bool isRunning = Input.GetKey(KeyCode.LeftShift);
        bool isFiring = weaponActivate.IsFiring();
        bool isReloading = weaponReload.isReloading;
        bool isChangingWeapon = weaponActivate.isChangingWeapon;
        bool isAiming = characterAiming.isAiming;
        return isRunning && !isFiring && !isReloading && !isChangingWeapon && !isAiming;
    }

    private void UpdateIsRunning()
    {
        bool isRunning = IsRunning();
        animator.SetBool(isRunningParam, isRunning);
        rig.SetBool(isRunningParam, isRunning);
    }

    private void OnAnimatorMove()
    {
        rootMotion += animator.deltaPosition;
    }

    private void FixedUpdate()
    {
        if (isJumping)
        {
            UpdateInAir();
        }
        else
        {
            UpdateOnGround();
        }
    }

    private void UpdateOnGround()
    {
        Vector3 stepForwardAmount = rootMotion * groundSpeed;
        Vector3 stepDownAmount = Vector3.down * stepDown;
        
        cc.Move(stepForwardAmount + stepDownAmount);
        rootMotion = Vector3.zero;

        if (!cc.isGrounded)
        {
            SetInAir(0);
        }
    }

    private void UpdateInAir()
    {
        velocity.y -= gravity * Time.deltaTime;
        Vector3 displacement = velocity * Time.deltaTime;
        displacement += CalculateAirControl();
        cc.Move(displacement);
        isJumping = !cc.isGrounded;
        rootMotion = Vector3.zero;
        animator.SetBool("isJumping", isJumping);
    }

    Vector3 CalculateAirControl()
    {
        return ((transform.forward * input.y) + (transform.right * input.x)) * airControl;
    }

    void Jump()
    {
        if (!isJumping)
        {
            float jumpVelocity = Mathf.Sqrt(2 * gravity * JumpHeight);
            SetInAir(jumpVelocity);
        }
    }

    private void SetInAir(float jumpVelocity)
    {
        isJumping = true;
        velocity = animator.velocity * groundSpeed;
        velocity.y = jumpVelocity;
        animator.SetBool("isJumping", true);
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody body = hit.collider.attachedRigidbody;

        // no rigidbody
        if (body == null || body.isKinematic)
            return;

        // We dont want to push objects below us
        if (hit.moveDirection.y < -0.3f)
            return;

        // Calculate push direction from move direction,
        // we only push objects to the sides never up and down
        Vector3 pushDir = new Vector3(hit.moveDirection.x, 0, hit.moveDirection.z);

        // If you know how fast your character is trying to move,
        // then you can also multiply the push velocity by that.

        // Apply the push
        body.velocity = pushDir * pushPower;
    }
}
