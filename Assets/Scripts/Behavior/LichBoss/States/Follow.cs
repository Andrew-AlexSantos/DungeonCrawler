using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.states
{
    public class Follow : State
    {

        private LichBossController controller;
        private LichBossHelper helper;


        private readonly float attackAttemptInterval = 0.5f;
        private float attackAttemptCooldown = 0;
        private readonly float targetUpdateInterval = 0.5f;
        private float targetUpdateCooldown = 0f;
        private float ceaseFollowCooldown = 0f;


        public Follow(LichBossController controller) : base("Follow")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        // public Follow(LichBossController lichBossController)
        // {
        //     this.lichBossController = lichBossController;
        // }

        public override void Enter()
        {
            base.Enter();

            // Reset stuff
            attackAttemptCooldown = attackAttemptInterval;
            targetUpdateCooldown = 0;
            ceaseFollowCooldown = controller.ceaseFollowInterval;
            Debug.Log("entrou no" + this.name);

        }

        public override void Update()
        {
            base.Update();

            // Update destination
            if ((targetUpdateCooldown -= Time.deltaTime) <= 0)
            {
                targetUpdateCooldown = targetUpdateInterval;
                var player = GameManager.Instance.player;
                var playerPosition = player.transform.position;
                controller.thisAgent.SetDestination(playerPosition);
            }

            // Attempt to Attack with Ritual
            if ((attackAttemptCooldown -= Time.deltaTime) <= 0)
            {
                attackAttemptCooldown = attackAttemptInterval;

                // Ritual
                var distanceToPlayer = helper.GetDistanceToPlayer();
                var isCloseEnoughToRitual = distanceToPlayer <= controller.distanceToRitual;
                if (isCloseEnoughToRitual)
                {
                    controller.stateMachine.ChangeState(controller.attackRitualState);
                    return;
                }
            }

            // attempt to cease follow (Normal and Super)
            if ((ceaseFollowCooldown -= Time.deltaTime) <= 0) {
                State newState = helper.HasLowHealth() ? controller.attackSuperState : controller.attackNormalState;
                controller.stateMachine.ChangeState(newState);
                return;

            }

        }

    }
}

    



