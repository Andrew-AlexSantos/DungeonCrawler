using System;
using UnityEngine;
using Unity.VisualScripting;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Data;
using JetBrains.Annotations;
using Player.States;
using EventsArgs;
using System.Runtime.CompilerServices;
using DungeonCrawler.Assets.Scripts.EventArgs;
using DungeonCrawler.Assets.Scripts;
using UnityEngine.AI;



public class PlayerController : MonoBehaviour
{
    // Public Properties

    // private properties

    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Walking walkingState;
    [HideInInspector] public Jump jumpState;
    [HideInInspector] public Attack attackState;
    [HideInInspector] public Die dieState;
    [HideInInspector] public Defend defendState;
    [HideInInspector] public Hurt hurtState;
    [HideInInspector] public Dead deadState;



    // Componets
    [HideInInspector]
    public Rigidbody thisRigidbody;
    [HideInInspector]
    public Collider thisCollider;
    [HideInInspector]
    public Animator thisAnimator;
    [HideInInspector] public LifeScript thisLife;


    [Header("Jump")]
    public float jumpPower = 10;
    public float jumpMovementFactor = 1f;
    [HideInInspector]
    public bool hasJumpInput;


    // Movement
    [Header("Movement")]
    public float movementSpeed = 10;
    public float maxSpeed = 10;
    [HideInInspector]
    public Vector2 movementVector;


    // Slope
    [Header("Slope")]
    [HideInInspector]
    public bool isGrounded;
    [HideInInspector]
    public bool isOnSlope;
    [HideInInspector]
    public float slopeAngle;
    [HideInInspector]
    public Vector3 slopeNormal;
    public float maxSlopeAngle = 45;

    // Attack
    [Header("Attack")]
    [SerializeField] public int attackStages;
    public List<float> attackStageDurations;
    public List<float> attackStageMaxIntervals;
    public List<float> attackStageImpulses;
    public GameObject swordHitBox;
    public float swordKnockbackImpulse = 10;
    public List<int> damageByStage;

    // Defend
    [Header("Defend")]
    public GameObject shieldHitBox;
    public float shieldKnockbackImpulse = 10;
    [HideInInspector] public bool hasDefenseInput;

    // Hurt
    [Header("Hurt")]
    public float hurtDuration = 0.2f;

    // Efects
    [Header("Effects")]
    public GameObject hitEffect;

    void Awake()
    {
        thisRigidbody = GetComponent<Rigidbody>();
        thisAnimator = GetComponent<Animator>();
        thisLife = GetComponent<LifeScript>();

        if (thisLife != null) {
            thisLife.OnDamage += OnDamage;
            thisLife.OnHeal += OnHeal;
            thisLife.canInflictDamageDelegate += CanInflictDamage;

        }
    }

    private bool CanInflictDamage(GameObject attacker, int damage)
    {
        var isDefending = stateMachine.currentStateName == defendState.name;
        if (isDefending)
        {
            Vector3 playerDirection = transform.TransformDirection(Vector3.forward);
            Vector3 attackDirection = (transform.position - attacker.transform.position).normalized;
            float dot = Vector3.Dot(playerDirection, attackDirection);
            if (dot < -0.25)
            {
                return false;
            }
        }
        return true;

    }


    // Start is called before the first frame update
    void Start()
    {
        // StateMachine and its states

        stateMachine = new StateMachine();
        idleState = new Idle(this);
        walkingState = new Walking(this);
        jumpState = new Jump(this);
        deadState = new Dead(this);
        hurtState = new Hurt(this);
        attackState = new Attack(this);
        defendState = new Defend(this);
        stateMachine.ChangeState(walkingState);

        // Toggle hitbox
        swordHitBox.SetActive(false);
        shieldHitBox.SetActive(false);

        // Update UI
        var gameplayUI = GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetMaxHealth(thisLife.maxHealth);
    }

    // Update is called once per frame
    void Update()
    {


        // Create input vector
        bool isUp = Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow);
        bool isDown = Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow);
        bool isLeft = Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow);
        bool isRight = Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow);
        float inputY = isUp ? 1 : isDown ? -1 : 0;
        float inputX = isRight ? 1 : isLeft ? -1 : 0;
        movementVector = new Vector2(inputX, inputY);
        hasJumpInput = Input.GetKey(KeyCode.Space);


        // Check defense Input
        hasDefenseInput = Input.GetMouseButton(1);

        // Update Animator
        float velocity = thisRigidbody.velocity.magnitude;
        float velocityRate = velocity / maxSpeed;
        thisAnimator.SetFloat("fVelocity", velocityRate);

        // Physics updates
        DetectGround();
        DetectSlope();

        // StateMachine
        var bossBattleHandler = GameManager.Instance.bossBattleHandler;
        var isInCutscene = bossBattleHandler.IsInCutscene();
        if(isInCutscene && stateMachine.currentStateName != idleState.name) {
            stateMachine.ChangeState(idleState);
        }
        stateMachine.Update();
    }

    void LateUpdate()
    {
        stateMachine.LateUpdate();
    }

    void FixedUpdate()
    {
        // Apply gravity
        Vector3 gravityForce = Physics.gravity * (isOnSlope ? 0.25f : 1f);
        thisRigidbody.AddForce(gravityForce, ForceMode.Acceleration);

        // Limit Speed
        LimitSpeed();

        // StateMachine
        stateMachine.FixedUpdate();
    }

    private void OnDamage(object sender, DamageEventArgs args) {
        // ignore if game is over
        if (GameManager.Instance.isGameOver) return;

        Debug.Log("Player recebeu um dano de " + args.damage + " do " + args.attacker.name);
        stateMachine.ChangeState(hurtState);

    }

    private void OnHeal(object sender, HealEventsArgs args) {
        var gameplayUI = GameManager.Instance.gameplayUI;
        gameplayUI.playerHealthBar.SetHealth(thisLife.health);
         Debug.Log("Player recebeu uma cura");
    }

    public void OnSwordCollisionEnter(Collider other) {
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var otherLife = otherObject.GetComponent<LifeScript>();
        var otherCollider = otherObject.GetComponent<Collider>();

        int bit = 1 << otherObject.layer;
        int mask = LayerMask.GetMask("Target", "Creatures");
        bool isBitInMask = (bit & mask) == bit;
        bool isTarget = isBitInMask;

        if (isTarget && otherRigidbody != null) {
            
            // Life
            if (otherLife != null) {
                var damage = damageByStage[attackState.stage - 1];
                otherLife.InflicDamage(gameObject, damage);
            }

            // Knockback
            if (otherRigidbody != null) {
                var positionDiff = otherObject.transform.position - gameObject.transform.position;
                var impulseVector = new Vector3(positionDiff.normalized.x, 0, positionDiff.normalized.z);
                impulseVector *= swordKnockbackImpulse;
                otherRigidbody.AddForce(impulseVector, ForceMode.Impulse);
            }
            

            // Hit effect
            if(hitEffect != null) {
                var hitPosition = otherCollider.ClosestPointOnBounds(swordHitBox.transform.position);
                var hitRotation = hitEffect.transform.rotation;
                Instantiate(hitEffect, hitPosition, hitRotation);

            }
        }
    }
    public void OnShieldColliderEnter(Collider other)
    {
        var otherObject = other.gameObject;
        var otherRigidbody = otherObject.GetComponent<Rigidbody>();
        var isTarget = true;
        if (isTarget && otherRigidbody != null) {
            var positionDiff = otherObject.transform.position - gameObject.transform.position;
            var impulseVector = new Vector3(positionDiff.normalized.x, 0, positionDiff.normalized.z);
            impulseVector *= shieldKnockbackImpulse;
            otherRigidbody.AddForce(impulseVector, ForceMode.Impulse);
        }
    }

    public Quaternion GetForward()
    {
        Camera camera = Camera.main;
        float eulerY = camera.transform.eulerAngles.y;
        return Quaternion.Euler(0, eulerY, 0);
    }

    public void RotateBodyToFaceInput(float alpha = 0.225f)
    {

        if (movementVector.IsZero()) return;

        // Calculate rotation
        Camera camera = Camera.main;
        Vector3 inputVector = new Vector3(movementVector.x, 0, movementVector.y);
        Quaternion q1 = Quaternion.LookRotation(inputVector, Vector3.up);
        Quaternion q2 = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0);
        Quaternion toRotation = q1 * q2;
        Quaternion newRotation = Quaternion.LerpUnclamped(transform.rotation, toRotation, alpha);

        // apply rotation
        thisRigidbody.MoveRotation(newRotation);


    }

    public bool AttemptToAttack()
    {

        if (Input.GetMouseButtonDown(0))
        {
            var isAttacking = stateMachine.currentStateName == attackState.name;
            var canAttack = !isAttacking || attackState.CanSwitchStage();
            if (canAttack)
            {
                var attackStage = isAttacking ? (attackState.stage + 1) : 1;
                attackState.stage = attackStage;
                stateMachine.ChangeState(attackState);
                return true;
            }
        }
        return false;
    }


    private void DetectGround()
    {
        // Reset flag
        isGrounded = false;

        // Detect ground
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.1f;
        LayerMask groundLayer = GameManager.Instance.groundLayer;
        if (Physics.Raycast(origin, direction, maxDistance, groundLayer))
        {
            isGrounded = true;

        }
    }

    private void DetectSlope()
    {
        // Reset flag
        isOnSlope = false;
        slopeNormal = Vector3.zero;

        // Detect ground
        Vector3 origin = transform.position;
        Vector3 direction = Vector3.down;
        float maxDistance = 0.1f;
        if (Physics.Raycast(origin, direction, out var slopeHitInfo, maxDistance)) {
            float angle = Vector3.Angle(Vector3.up, slopeHitInfo.normal);
            isOnSlope = angle < maxSlopeAngle && angle != 0;
            slopeNormal = isOnSlope ? slopeHitInfo.normal : Vector3.zero;
        }
    }

    private void LimitSpeed() {

        Vector3 flatVelocity = new Vector3(thisRigidbody.velocity.x, y: 0, thisRigidbody.velocity.z);
        if (flatVelocity.magnitude > maxSpeed) {
            Vector3 limitedVelocity = flatVelocity.normalized * maxSpeed;
            thisRigidbody.velocity = new Vector3(limitedVelocity.x, thisRigidbody.velocity.y, limitedVelocity.z);
        }
        // thisRigidbody
        // maxSpeed


    } 
        private void OnTriggerEnter(Collider other) {
            if (other.CompareTag("BossRoomSensor")) {
                GlobalEvent.Instance.InvokeBossRoomEnter(this, new BossRoomEnterArgs());
            Destroy(other.gameObject);
         }
    }
}