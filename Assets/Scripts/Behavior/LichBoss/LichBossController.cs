using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using EventsArgs;
using System;
using Behaviors.LichBoss.states;

namespace Behaviors.LichBoss
{
    public class LichBossController : MonoBehaviour
    {
        
        [HideInInspector] public LichBossHelper helper;
        [HideInInspector] public NavMeshAgent thisAgent;
        [HideInInspector] public Animator thisAnimator;
        [HideInInspector] public LifeScript thisLife;

        // State Machine
        [HideInInspector] public StateMachine stateMachine;
        [HideInInspector] public Idle idleState;
         [HideInInspector] public Follow followState;
         [HideInInspector] public AttackNormal attackNormalState;
         [HideInInspector] public AttackSuper attackSuperState;
         [HideInInspector] public AttackRitual attackRitualState;
         [HideInInspector] public Hurt hurtState;
         [HideInInspector] public Dead deadState;

         [Header("General")]
         public float lowHealthTreshold = 0.4f;
        public Transform staffTop;
        public Transform staffBottom;
        public Vector3 aimOffset = new Vector3(0, 1.4f, 0);

         [Header("Idle")]
         public float idleDuration = 0.3f;

         [Header("Follow")]
         public float ceaseFollowInterval = 4f;

         [Header("Attack")]
        public int attackDamage = 1;

        [Header("Attack Normal")]
        public float attackNormalMagicDelay = 0f;
        public float attackNormalDuration = 0f;
        public float attackNormalImpulse = 10;

        [Header("Attack Ritual")]
        public float distanceToRitual = 2.5f;
        public float attackRitualDelay = 0f;
        public float attackRitualDuration = 0f;

        [Header("Attack Super")]
        public float attackSuperMagicDelay = 0f;
        public float attackSuperDuration = 1f;
        public float attackSuperMagicDuration = 1f;
        public int attackSuperMagicCount = 5;
        public float attackSuperImpulse = 10;

         [Header("Hurt")]
         public float hurtDuration = 0.5f;

        [Header("Magic")]
        public GameObject fireballPrefab;
        public GameObject energyballPrefab;
        public GameObject ritualPrefab;
         //[Header("Dead")]
         // public float destroyifFar = 30f;

        [Header("debug")]
        public string currentStateName;

        // State Coroutines
        [HideInInspector] public List<IEnumerator> stateCoroutines = new List<IEnumerator>();

        private void Awake()
        {

            // Get componets
            thisAgent = GetComponent<NavMeshAgent>();
            thisAnimator = GetComponent<Animator>();
            thisLife = GetComponent<LifeScript>();
            // Create helper
            helper = new LichBossHelper(this);
        }

        private void Start() {
        // Create StateMachine
        stateMachine = new StateMachine();
        idleState = new Idle(this);
        followState = new Follow(this);
        attackNormalState = new AttackNormal(this);
        attackSuperState = new AttackSuper(this);
        attackRitualState = new AttackRitual(this);        // attackState = new Attack(this);
        hurtState = new Hurt(this);
        deadState = new Dead(this);
        stateMachine.ChangeState(idleState);

        // Register listeners
        thisLife.OnDamage += OnDamage;
    }
    
     private void OnDamage(object sender, DamageEventArgs args) {
        Debug.Log("Lich Boss recebeu " + args.damage + " de dano de " + args.attacker.name);
        stateMachine.ChangeState(hurtState);
    }
     private void Update(){
        // Update StateMachine
        var bossBattleHandler = GameManager.Instance.bossBattleHandler;
        if (bossBattleHandler.IsActive()) {
            stateMachine.Update();
        }
        currentStateName = stateMachine.currentStateName;

         // Update animator
        var velocityRate = thisAgent.velocity.magnitude / thisAgent.speed;
        thisAnimator.SetFloat("fVelocity", velocityRate);

        // Face to player
        if (!thisLife.IsDead()) {
            var player = GameManager.Instance.player;
            var vecToPlayer = player.transform.position - transform.position;
            vecToPlayer.y = 0;
            vecToPlayer.Normalize();
            var desiredRotation = Quaternion.LookRotation(vecToPlayer);
            var newRotation = Quaternion.LerpUnclamped(transform.rotation, desiredRotation, 0.2f);
            transform.rotation = newRotation;
            }
    }

    private void LateUpdate(){
        // Update StateMachine
        stateMachine.LateUpdate();
        
    }

    private void FixedUpdate(){
        // Update StateMachine
        stateMachine.FixedUpdate();
    }

        public static implicit operator LichBossController(MeleeCreatureController v)
        {
            throw new NotImplementedException();
        }
    }
}
