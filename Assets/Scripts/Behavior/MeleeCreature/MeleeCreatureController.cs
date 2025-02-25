using System;
using System.Collections;
using System.Collections.Generic;
using Behaviors.MeleeCreature.States;
using EventsArgs;
using UnityEngine;
using UnityEngine.AI;
    
public class MeleeCreatureController : MonoBehaviour {

    [HideInInspector] public MeleeCreatureHelper helper;
    [HideInInspector] public NavMeshAgent thisAgent;
    [HideInInspector] public Animator thisAnimator;
    [HideInInspector] public LifeScript thisLife;
    [HideInInspector] public Collider thisCollider;
    [HideInInspector] public Rigidbody thisRigidbody;
 
    // State Machine
    [HideInInspector] public StateMachine stateMachine;
    [HideInInspector] public Idle idleState;
    [HideInInspector] public Follow followState;
    [HideInInspector] public Attack attackState;
    [HideInInspector] public Hurt hurtState;
    [HideInInspector] public Dead deadState ;
    [Header("General")]
    public float searchRadius = 5f;

    [Header("Idle")]
    public float targetSearchInterval = 1f;

    [Header("Follow")]
    public float ceaseFollowInterval = 4f;

    [Header("Attack")]
    public float distanceToAttack = 1f;
    public float attackRadius = 1.5f;
    public float attackSphereRadius = 1.5f;
    public float damageDelay = 0f;
    public float attackDuration = 1f;
    public int attackDamage = 1;

    [Header("Hurt")]
    public float hurtDuration = 1f;

    [Header("Dead")]
    public float destroyifFar = 30f;

    [Header("Effects")]
    public GameObject knockoutEffect;

    [Header("debug")]
    public string currentStateName;

    private void Awake() {
        
        // Get componets
        thisAgent = GetComponent<NavMeshAgent>();
        thisAnimator = GetComponent<Animator>();
        thisLife = GetComponent<LifeScript>();
        thisCollider =GetComponent<Collider>();
        thisRigidbody = GetComponent<Rigidbody>();

        // Create helper
        helper = new MeleeCreatureHelper(this);
    }
    private void Start() {
        // Create StateMachine
        stateMachine = new StateMachine();
        idleState = new Idle(this);
        followState = new Follow(this);
        attackState = new Attack(this);
        hurtState = new Hurt(this);
        deadState = new Dead(this);
        stateMachine.ChangeState(idleState);

        // Register listeners
        thisLife.OnDamage += OnDamage;

    }

    private void OnDamage(object sender, DamageEventArgs args) {
        Debug.Log("Criatura recebeu " + args.damage + " de dano de " + args.attacker.name);
        stateMachine.ChangeState(hurtState);
        
    }

    private void Update(){
        // Update StateMachine
        stateMachine.Update();
        currentStateName = stateMachine.currentStateName;

        // Update animator
        var velocityRate = thisAgent.velocity.magnitude / thisAgent.speed;
        thisAnimator.SetFloat("fVelocity", velocityRate);
        
    }

    private void LateUpdate(){
        // Update StateMachine
        stateMachine.LateUpdate();
    }

    private void FixedUpdate(){
        // Update StateMachine
        stateMachine.FixedUpdate();
    }
}
