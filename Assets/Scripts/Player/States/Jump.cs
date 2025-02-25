using UnityEngine;
using System;

namespace Player.States
{
    public class Jump : State
    {

        private PlayerController controller;

        private bool hasJumped;
        private float cooldown;

        public Jump(PlayerController controller) : base("Jump")
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            base.Enter();

            hasJumped = false;
            cooldown = 0.5f;

            // Handle animator
            controller.thisAnimator.SetBool("bJumping", true);




        }

        public override void Exit()
        {
            base.Exit();

            controller.thisAnimator.SetBool("bJumping", false);

        }

        public override void Update()
        {
            base.Update();
            // Update cooldown
            cooldown -= Time.deltaTime;
            // Switch to Idle
            if (hasJumped && controller.isGrounded && cooldown <= 0)
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
            // Jump
            if (!hasJumped)
            {
                hasJumped = true;
                ApplyImpulse();
            }
            // Create movement Vector
            Vector3 walkVector = new Vector3(controller.movementVector.x, 0, controller.movementVector.y);
            walkVector = controller.GetForward() * walkVector;
            walkVector *= controller.movementSpeed * controller.jumpMovementFactor;


            // Apply input to character
            controller.thisRigidbody.AddForce(walkVector, ForceMode.Force);

            // Rotate Character
            controller.RotateBodyToFaceInput();
        }

        private void ApplyImpulse()
        {
            // Aplica o impulse
            Vector3 forceVector = Vector3.up * controller.jumpPower;
            controller.thisRigidbody.AddForce(forceVector, ForceMode.Impulse);

        }

    }
}