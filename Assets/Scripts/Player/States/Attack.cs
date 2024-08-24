using UnityEngine;

namespace Player.States {
    public class Attack : State {

        private PlayerController controller;

        public int stage = 1;
        private float stateTime;
        private bool firstFixedUpdate;

        public Attack(PlayerController controller) : base("Attack")
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            base.Enter();

            // ERROR: Invalid stage
            if (stage <= 0 || stage > controller.attackStages)
            {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }

            // Reset variables
            stateTime = 0;
            firstFixedUpdate = true;

            // Set animation trigger
            controller.thisAnimator.SetTrigger("tAttack" + stage);

            // Toggle hitbox
            controller.swordHitBox.SetActive(true);

        }

        public override void Exit()
        {
            base.Exit();

            // Toggle hitbox
            controller.swordHitBox.SetActive(false);
        }

        public override void Update()
        {
            base.Update();
            // Update StateTime
            stateTime += Time.deltaTime;


            // Exit after time
            if (isStateExpired())
            {
                controller.stateMachine.ChangeState(controller.idleState);
                return;
            }

            // Switch to Attack
            if (controller.AttemptToAttack())
            {
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
            if (firstFixedUpdate)
            {
                firstFixedUpdate = false;

                // Look to input
                controller.RotateBodyToFaceInput(1);

                // Impulse
                var impulseValue = controller.attackStageImpulses[stage - 1];
                var impulseVector = controller.thisRigidbody.rotation * Vector3.forward;
                impulseVector *= impulseValue;
                controller.thisRigidbody.AddForce(impulseVector, ForceMode.Impulse);
            }
        }

        public bool CanSwitchStage()
        {
            var isLastState = stage == controller.attackStages;
            var stageDuration = controller.attackStageDurations[stage - 1];
            var stateMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
            var MaxStateDuration = stageDuration + stateMaxInterval;

            // Reply
            return !isLastState && stateTime >= stageDuration && stateTime <= MaxStateDuration;
        }

        public bool isStateExpired()
        {
            var isLastState = stage == controller.attackStages;
            var stageDuration = controller.attackStageDurations[stage - 1];
            var stateMaxInterval = isLastState ? 0 : controller.attackStageMaxIntervals[stage - 1];
            var MaxStateDuration = stageDuration + stateMaxInterval;

            // Reply
            return stateTime > MaxStateDuration;
        }
    }
}