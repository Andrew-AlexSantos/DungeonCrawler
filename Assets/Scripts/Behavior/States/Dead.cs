using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.MeleeCreature.States {
    public class Dead : State {

        private MeleeCreatureController controller;
        private MeleeCreatureHelper helper;
        public Dead(MeleeCreatureController controller) : base("Dead") {
            this.controller = controller;
            this.helper = controller.helper;
        }

        public override void Enter()  {
            base.Enter();
            // pause damage
            controller.thisLife.isVulnerable = false;

            // Update animator
            controller.thisAnimator.SetTrigger("tDead");

            // Disable collider
            controller.thisCollider.enabled = false;

            // Create knockout effect
            var knockoutEffect = controller.knockoutEffect;
            if (knockoutEffect != null) {
                var position = controller.transform.position;
                var rotation = knockoutEffect.transform.rotation;
                Object.Instantiate(knockoutEffect, position, rotation);
            }

        }

        public override void Exit() {
            base.Exit();
        }

        public override void Update() {
            base.Update();

            // Delete if far away from player
            var distanceToPlayer = helper.GetDistanceToPlayer();
            if (distanceToPlayer >= controller.destroyifFar) {
                Object.Destroy(controller.gameObject);
            }

        }

        public override void LateUpdate() {
            base.LateUpdate();
        }

        public override void FixedUpdate() {
            base.FixedUpdate();
        }

    }
}

