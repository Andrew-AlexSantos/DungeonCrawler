using UnityEngine;

namespace Player.States {
    public class Defend : State
    {

        private PlayerController controller;

        public Defend(PlayerController controller) : base("Defend")
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            base.Enter();

            // Toggle animator 
            controller.thisAnimator.SetBool("bDefend", true);

            // Toggle hitbox
            controller.shieldHitBox.SetActive(true);

        }

        public override void Exit()
        {
            base.Exit();
            // Toggle animator 
            controller.thisAnimator.SetBool("bDefend", false);
            // Toggle hitbox
            controller.shieldHitBox.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            // Switch to Idle
            if (!controller.hasDefenseInput)
            {
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

            // Rotate Character
            controller.RotateBodyToFaceInput();
        }

    }
}