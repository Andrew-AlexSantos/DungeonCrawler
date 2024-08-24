using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Assets.Scripts.Projectiles;
using EventsArgs;
using UnityEngine;


namespace Behaviors.LichBoss.states
{
    public class AttackSuper : State
    {

        private LichBossController controller;
        private LichBossHelper helper;
        private float endAttackCooldown;


        private float searchCooldown;

        public AttackSuper(LichBossController controller) : base("AttackSuper")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        // public AttackSuper(LichBossController lichBossController)
        // {
        //     this.lichBossController = lichBossController;
        // }

        public override void Enter() {
            base.Enter();
            // Set variables
            endAttackCooldown = controller.attackSuperDuration;
            Debug.Log("Atacou com o Super");
            controller.stateMachine.ChangeState(controller.idleState);
            Debug.Log("entrou no" + this.name);

            // Updater animator
            controller.thisAnimator.SetTrigger("tAttackSuper");

            // Schedule attacks
                var delayStep = controller.attackSuperMagicDuration / (controller.attackSuperMagicCount - 1);
            for (int i = 0; i < controller.attackSuperMagicCount - 1; i++) {
                var delay = controller.attackSuperMagicDelay + delayStep * i;
            helper.StartStateCoroutine(ScheduleAttack(delay));
            }   
        }

        public override void Exit()
        {
            base.Exit();
            helper.ClearStateCoroutines();
        }
        public override void Update()
        {
            base.Update();
            
            // End attack
            if ((endAttackCooldown -= Time.deltaTime) <= 0f) {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }


        private IEnumerator ScheduleAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            Debug.Log("Atacou com " + this.name);
            // Create Object
            var spawnTransform = controller.staffTop;
            var projectile = Object.Instantiate(
            controller.energyballPrefab,
            spawnTransform.position,
            spawnTransform.rotation);


            // Populate ProjectileCollision
            var ProjectileCollision = projectile.GetComponent<ProjectileCollision>();
            ProjectileCollision.attacker = controller.gameObject;
            ProjectileCollision.damage = controller.attackDamage;

            // Get Stuff
            var player = GameManager.Instance.player;
            var projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply impulse
            var vectorToPlayer = (player.transform.position + controller.aimOffset - spawnTransform.position).normalized;
            var forceVector = spawnTransform.rotation * Vector3.forward;
            forceVector = new Vector3(forceVector.x, vectorToPlayer.y, forceVector.z);
            forceVector *= controller.attackSuperImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 30);
        }

    }
}
