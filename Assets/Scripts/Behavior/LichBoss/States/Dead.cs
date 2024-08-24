using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Assets.Scripts;
using DungeonCrawler.Assets.Scripts.EventArgs;
using UnityEngine;

namespace Behaviors.LichBoss.states
{
    public class Dead : State
    {

        private LichBossController controller;
        private LichBossHelper helper;

        private float searchCooldown;

        public Dead(LichBossController controller) : base("Dead")
        {
            this.controller = controller;
            this.helper = controller.helper;
        }

        // public Dead(LichBossController lichBossController)
        // {
        //     this.lichBossController = lichBossController;
        // }

        public override void Enter()
        {
            base.Enter();
            // pause damage
            controller.thisLife.isVulnerable = false;

            // Updater animator
            controller.thisAnimator.SetTrigger("tDead");

            // Game Won
            GlobalEvent.Instance.InvokeGameWon(this, new GameWonArgs());

        }
        



    }
}


