using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss.states
{
    public class Idle : State
    {

        private LichBossController controller;
        private LichBossHelper helper;

        private float searchCooldown;
        private LichBossController lichBossController;
        public float stateTime;

        public Idle(LichBossController controller) : base("Idle")
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
            stateTime = 0;

            Debug.Log("entrou no" + this.name);

        }

        public override void Exit()
        {
            base.Exit();

            // Stop following
            controller.thisAgent.ResetPath();
        }
        public override void Update()
        {
            base.Update();
            stateTime += Time.deltaTime;
            
            // ignore if game is over
            if (GameManager.Instance.isGameOver) return;
            
            // Switch to Follow
            if (stateTime >= controller.idleDuration)
            {
                controller.stateMachine.ChangeState(controller.followState);
                return;
            }
        }


    }
}


