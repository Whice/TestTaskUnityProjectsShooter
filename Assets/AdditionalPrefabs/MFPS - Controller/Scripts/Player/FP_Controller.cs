﻿using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(FP_Input))]
[RequireComponent(typeof(FP_CameraLook))]
[RequireComponent(typeof(FP_FootSteps))]
public class FP_Controller : MonoBehaviour
{
    public bool canControl = true;
    public float gravity = 20.0f;
    /// <summary>
    /// Скорость ходьбы.
    /// </summary>
    public float walkSpeed
    {
        get => PlayerModel.instance.walkSpeed;
    }
    /// <summary>
    /// Скрорсть бега.
    /// </summary>
    public float runSpeed
    {
        get => PlayerModel.instance.runSpeed;
    }
    public float jumpForce = 8.0f;
    public float crouchSpeed = 2.0F;
    public float crouchHeight = 1.0F;

    public KeyCode crouchKey = KeyCode.LeftControl;
    public KeyCode runKey = KeyCode.LeftShift;
    public KeyCode jumpKey = KeyCode.Space;

    public bool airControl = true;
    public bool canCrouch = true;
    public bool canJump = true;
    public bool canRun = true;

    [HideInInspector]
    public CharacterController controller;

    private Vector3 moveDirection;
    private Vector3 contactPoint;
    private Vector3 hitNormal;
    private AudioSource JumpLandSource;
    private FP_FootSteps footSteps;
    private Transform myTransform;
    private FP_Input playerInput;
    private RaycastHit hit;


    private bool playerControl = false;
    private bool isCrouching = false;
    private bool grounded = false;
    private bool sliding = false;
    private bool crouch = false;
    private bool jump = false;
    private bool run = false;

    #region Бег.

    /// <summary>
    /// Окружность-картинка кнопки бега.
    /// </summary>
    public Image runButtonCircle
    {
        get => PlayerModel.instance.playerView.runButtonCircle;
    }
    /// <summary>
    /// Текст кнопки бега.
    /// </summary>
    public Text runButtonText
    {
        get => PlayerModel.instance.playerView.runButtonText;
    }
    /// <summary>
    /// Цвет кнопки и текста пи включенном беге.
    /// </summary>
    private static readonly Color enableRunColor = Color.yellow;
    /// <summary>
    /// Цвет кнопки и текста пи отключенном беге.
    /// </summary>
    private static readonly Color disableRunColor = Color.white;
    /// <summary>
    /// Бег включен.
    /// </summary>
    private bool enableRunField = false;
    /// <summary>
    /// Бег включен.
    /// Включение/отключение изменяет скорость передвижения.
    /// <br/>true - скорость бега.
    /// <br/>false - скорость ходьбы.
    /// </summary>
    private bool enableRun
    {
        get => this.enableRunField;
        set
        {
            this.enableRunField = value;
            if (value)
            {
                this.speed = this.runSpeed;
                this.runButtonCircle.color = FP_Controller.enableRunColor;
                this.runButtonText.color = FP_Controller.enableRunColor;
            }
            else
            {
                this.speed = this.walkSpeed;
                this.runButtonCircle.color = FP_Controller.disableRunColor;
                this.runButtonText.color = FP_Controller.disableRunColor;
            }
        }
    }

    #endregion Бег.

    private int antiBunnyHopFactor = 1;
    private int jumpTimer;
    private int landTimer;
    private int jumpState;
    private int runState;

    private float antiBumpFactor = 0.75F;
    private float inputModifyFactor;
    private float slideSpeed = 2.0F;
    private float minCrouchHeight;
    private float inputX, inputZ;
    private float fallStartLevel;
    private float defaultHeight;
    private float rayDistance;
    private float slideLimit;
    private float speed;
    private string surfaceTag;

    void Awake()
    {
        controller = GetComponent<CharacterController>();
        playerInput = GetComponent<FP_Input>();
        footSteps = GetComponent<FP_FootSteps>();
    }

    void Start()
    {
        defaultHeight = controller.height;
        minCrouchHeight = crouchHeight > controller.radius * 2 ? crouchHeight : controller.radius * 2;
        myTransform = transform;
        speed = walkSpeed;
        rayDistance = controller.height * 0.5F + controller.radius;
        slideLimit = controller.slopeLimit - 0.1F;
        jumpTimer = antiBunnyHopFactor;
        JumpLandSource = gameObject.AddComponent<AudioSource>();
    }


    void FixedUpdate()
    {
        // If both horizontal and vertical are used simultaneously, limit speed (if allowed), so the total doesn't exceed normal move speed
        inputModifyFactor = (inputX != 0.0F && inputZ != 0.0F) ? 0.7071F : 1.0F;

        if (grounded)
        {
            sliding = false;
            // See if surface immediately below should be slid down. We use this normally rather than a ControllerColliderHit point,
            // because that interferes with step climbing amongst other annoyances
            if (Physics.Raycast(myTransform.position, -Vector3.up, out hit, rayDistance))
            {
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
                    sliding = true;
            }
            // However, just raycasting straight down from the center can fail when on steep slopes
            // So if the above raycast didn't catch anything, raycast down from the stored ControllerColliderHit point instead
            else
            {
                Physics.Raycast(contactPoint + Vector3.up, -Vector3.up, out hit);
                if (Vector3.Angle(hit.normal, Vector3.up) > slideLimit && CanSlide())
                    sliding = true;
            }

            // If sliding (and it's allowed), or if we're on an object tagged "Slide", get a vector pointing down the slope we're on
            if (sliding)
            {
                hitNormal = hit.normal;
                moveDirection = new Vector3(hitNormal.x, -hitNormal.y, hitNormal.z);
                Vector3.OrthoNormalize(ref hitNormal, ref moveDirection);
                moveDirection *= slideSpeed;
                playerControl = false;
            }
            // Otherwise recalculate moveDirection directly from axes, adding a bit of -y to avoid bumping down inclines
            else
            {
                moveDirection = new Vector3(inputX * inputModifyFactor, -antiBumpFactor, inputZ * inputModifyFactor);
                moveDirection = myTransform.TransformDirection(moveDirection) * speed;
                playerControl = true;
            }

            // Jump! But only if canJump, the jump button has been released and player has been grounded for a given number of frames
            if (!jump)
                jumpTimer++;
            else if (canJump && jumpTimer >= antiBunnyHopFactor)
            {
                moveDirection.y = jumpForce;
                jumpTimer = 0;
            }
        }
        else
        {
            // If air control is allowed, check movement but don't touch the y component
            if (airControl && playerControl)
            {
                moveDirection.x = inputX * speed * inputModifyFactor;
                moveDirection.z = inputZ * speed * inputModifyFactor;
                moveDirection = myTransform.TransformDirection(moveDirection);
            }
        }

        // Apply gravity
        moveDirection.y -= gravity * Time.deltaTime;
        // Move the controller, and set grounded true or false depending on whether we're standing on something
        grounded = (controller.Move(moveDirection * Time.deltaTime) & CollisionFlags.Below) != 0;
    }

    void Update()
    {
        if (!canControl)
            return;

        inputX = Input.GetAxis("Horizontal");
        inputZ = Input.GetAxis("Vertical");
        crouch = Input.GetKey(crouchKey);
        run = Input.GetKey(runKey);
        jump = Input.GetKey(jumpKey);

        this.enableRun = this.run;

        if (jumpState == 0 && CanStand() && jump && jumpTimer >= antiBunnyHopFactor)
        {
            PlaySound(footSteps.jumpSound, JumpLandSource);
            jumpState++;
        }

        if ((Mathf.Abs((transform.position - contactPoint).magnitude) > 2))
            landTimer = 1;

        isCrouching = crouch && canCrouch;

        if (grounded)
        {
            if (isCrouching)
            {
                ArenaModel.instance.ActivateAllDinamicObjectsOnArena(false);
                //controller.center = Vector3.Lerp(controller.center, new Vector3(controller.center.x, -(defaultHeight - minCrouchHeight) / 2, controller.center.z), 15 * Time.deltaTime);
                //controller.height = Mathf.Lerp(controller.height, minCrouchHeight, 15 * Time.deltaTime);
            }
            else
            {
                ArenaModel.instance.ActivateAllDinamicObjectsOnArena(true);
                if (CanStand())
                {
                    //controller.center = Vector3.Lerp(controller.center, Vector3.zero, 15 * Time.deltaTime);
                    //controller.height = Mathf.Lerp(controller.height, defaultHeight, 15 * Time.deltaTime);
                }
            }
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (!IsGrounded() && landTimer == 1)
            PlaySound(footSteps.landSound, JumpLandSource);
        landTimer = 0;
        jumpState = 0;
        contactPoint = hit.point;
        surfaceTag = hit.collider.tag;
    }

    void PlaySound(AudioClip audio, AudioSource source)
    {
        source.clip = audio;
        if (audio)
            source.Play();
    }
    public bool IsGrounded()
    {
        return grounded;
    }
    public bool IsCrouching()
    {
        return crouch;
    }
    public bool IsRunning()
    {
        return run;
    }
    private bool CanStand()
    {
        RaycastHit hitAbove = new RaycastHit();
        return !Physics.SphereCast(controller.bounds.center, controller.radius, Vector3.up, out hitAbove,
                                   controller.height / 2 + 0.5F);
    }
    private bool CanSlide()
    {
        return new Vector3(controller.velocity.x, 0, controller.velocity.z).magnitude < walkSpeed / 2;
    }
    public string SurfaceTag()
    {
        return surfaceTag;
    }

}