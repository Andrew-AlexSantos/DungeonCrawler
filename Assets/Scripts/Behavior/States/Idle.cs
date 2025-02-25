using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.MeleeCreature.States {
    public class Idle : State {

        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;

        private float searchCooldown;
        public Idle(MeleeCreatureController controller) : base("Idle") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()  {
            base.Enter();

            // Reset some stuff
            searchCooldown = controller.targetSearchInterval;
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update(){
            base.Update();

            // ignore if game is over
            if (GameManager.Instance.isGameOver) return;
            
            // Update cooldown
            searchCooldown -= Time.deltaTime;
            if(searchCooldown <= 0 ) {
                searchCooldown = controller.targetSearchInterval;

                // Search for player
                if(helper.IsPlayerOnSight()) {
                    controller.stateMachine.ChangeState(controller.followState);
                    return;
                }

            }

        }

        public override void LateUpdate()
        {
            base.LateUpdate();
        }

        public override void FixedUpdate()
        {
            base.FixedUpdate();
        }

    }
}

