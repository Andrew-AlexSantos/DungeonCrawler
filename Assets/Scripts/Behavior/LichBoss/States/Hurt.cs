using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.states
{
    public class Hurt : State
    {

        private LichBossController controller;
        private LichBossHelper helper;

        private float searchCooldown;
        private float timePassed;

        public Hurt(LichBossController controller) : base("Hurt")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        // public Idle(LichBossController lichBossController)
        // {
        //     this.lichBossController = lichBossController;
        // }

        public override void Enter()
        {
            base.Enter();
            // Reset Timer
            timePassed = 0;

            // Pause damage
            controller.thisLife.isVulnerable = false;

            // Updater animator
            controller.thisAnimator.SetTrigger("tHurt");            
        }
         public override void Exit() {
            base.Exit();

            controller.thisLife.isVulnerable = true;
        }
public override void Update()
        {
            base.Update();

            // Update timer
            timePassed += Time.deltaTime;

            // Switch states
            if (timePassed >= controller.hurtDuration)
            {
                if (controller.thisLife.IsDead())
                {
                    controller.stateMachine.ChangeState(controller.deadState);
                }
                else
                {
                    controller.stateMachine.ChangeState(controller.idleState);
                }
                return;
            }
        }



    }
}


