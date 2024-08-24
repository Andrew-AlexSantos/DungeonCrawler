using System.Collections;
using UnityEngine;
using DungeonCrawler.Assets.Scripts.Projectiles;
namespace Behaviors.LichBoss.states
{
    public class AttackNormal : State
    {

        private LichBossController controller;
        private LichBossHelper helper;

        private float endAttackCooldown;

        private float searchCooldown;

        public AttackNormal(LichBossController controller) : base("AttackNormal")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()
        {
            base.Enter();

            // Set variables
            endAttackCooldown = controller.attackNormalDuration;
            Debug.Log("Atacou com Normal");
            controller.stateMachine.ChangeState(controller.idleState);

            // Updater animator
            controller.thisAnimator.SetTrigger("tAttackNormal");

            Debug.Log("entrou no" + this.name);

            // Schedule attacks
            helper.StartStateCoroutine(ScheduleAttack(controller.attackNormalMagicDelay));

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
            if ((endAttackCooldown -= Time.deltaTime) <= 0f)
            {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }

        }
        private IEnumerator ScheduleAttack(float delay)
        {
            yield return new WaitForSeconds(delay);

            Debug.Log("Atacou com " + this.name);


            // Create Object
            var spawnTransform = controller.staffTop;
            var projectile = Object.Instantiate(
            controller.fireballPrefab,
            spawnTransform.position,
            spawnTransform.rotation);

            // Populate ProjectileCollision
            var ProjectileCollision = projectile.GetComponent<ProjectileCollision>();
            ProjectileCollision.attacker = controller.gameObject;
            ProjectileCollision.damage = controller.attackDamage;

            // Get Stuff
            var staffTop = controller.staffTop;
            var player = GameManager.Instance.player;
            var projectileRigidbody = projectile.GetComponent<Rigidbody>();

            // Apply impulse
            var vectorToPlayer = (player.transform.position + controller.aimOffset - spawnTransform.position).normalized;
            var forceVector = spawnTransform.rotation * Vector3.forward;
            forceVector = new Vector3(forceVector.x, vectorToPlayer.y, forceVector.z);
            forceVector *= controller.attackNormalImpulse;
            projectileRigidbody.AddForce(forceVector, ForceMode.Impulse);

            // Schedule destruction
            Object.Destroy(projectile, 20);


        }
    }
}



