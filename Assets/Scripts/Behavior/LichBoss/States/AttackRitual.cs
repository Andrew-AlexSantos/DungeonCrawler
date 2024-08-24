using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.states
{
    public class AttackRitual : State
    {

        private LichBossController controller;
        private LichBossHelper helper;
        private float endAttackCooldown;

        private float searchCooldown;
        public AttackRitual(LichBossController controller) : base("AttackRitual")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        // public AttackRitual(LichBossController lichBossController)
        // {
        //     this.lichBossController = lichBossController;
        // }

        public override void Enter()
        {
            base.Enter();
            // Set variables
            endAttackCooldown = controller.attackRitualDuration;
            Debug.Log("Atacou com o Ritual");
            controller.stateMachine.ChangeState(controller.idleState);
            Debug.Log("entrou no" + this.name);

            // Updater animator
            controller.thisAnimator.SetTrigger("tAttackRitual");

            // Schedule attacks
            helper.StartStateCoroutine(ScheduleAttack(controller.attackRitualDelay));
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
            var GameObject = Object.Instantiate(
            controller.ritualPrefab,
            controller.staffBottom.position,
            controller.ritualPrefab.transform.rotation);

            // Schedule destruction
            Object.Destroy(GameObject, 10);

            // Damage Player
            if (helper.GetDistanceToPlayer() <= controller.distanceToRitual) {
                var playerLife = GameManager.Instance.player.GetComponent<LifeScript>();
                playerLife.InflicDamage(controller.gameObject, controller.attackDamage);
            }
        }
    }
 }


