using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Assets.Scripts;
using DungeonCrawler.Assets.Scripts.EventArgs;
using UnityEngine;

namespace Player.States {
    public class Dead : State
    {

        private PlayerController controller;

        public Dead(PlayerController controller) : base("Dead")
        {
            this.controller = controller;
        }
        public override void Enter()
        {
            base.Enter();
            controller.thisAnimator.SetTrigger("tGameOver");

            // Make player invulnerable
            controller.thisLife.isVulnerable = false;

            // GameOver
            GlobalEvent.Instance.InvokeGameOver(this, new GameOverArgs());
        }

        public override void Exit()
        {
            base.Exit();
        }

        public override void Update()
        {
            base.Update();

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