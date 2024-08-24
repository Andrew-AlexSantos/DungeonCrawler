using Unity.VisualScripting;
using UnityEngine;

namespace Player.States {
    public class Hurt : State
    {

        private PlayerController controller;
        private float timePassed;

        public Hurt(PlayerController controller) : base("Hurt")
        {
            this.controller = controller;
        }
        public override void Enter() {
            base.Enter();
            // Reset time
            timePassed = 0;

            // Pause damage
            controller.thisLife.isVulnerable = false;

            // Update animator
            controller.thisAnimator.SetTrigger("tHurt");

            // Update UI
            var gameplayUI = GameManager.Instance.gameplayUI;
            gameplayUI.playerHealthBar.SetHealth(controller.thisLife.health);

        }

        public override void Exit() {
            base.Exit();

            // Resume damage
            controller.thisLife.isVulnerable = true;
        }

        public override void Update()
        {
            base.Update();

            // Switch to dead 
            if (controller.thisLife.IsDead()) {
                controller.stateMachine.ChangeState(controller.deadState);
                return;
            }

            // Update time
            timePassed += Time.deltaTime;

            // Switch to idle
            if(timePassed != controller.hurtDuration) {
                controller.stateMachine.ChangeState(controller.idleState);
                return;   
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