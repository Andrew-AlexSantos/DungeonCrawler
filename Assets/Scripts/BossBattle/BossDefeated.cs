using System;
using UnityEngine;



namespace DungeonCrawler.Assets.Scripts.BossBattle {
    public class BossDefeated : State {
        public BossDefeated() : base("BossDefeated") {
         }


        public override void Enter() {
            base.Enter();
            // disable animations
            var gameManager = GameManager.Instance; 
            var boss = gameManager.boss;

            // var bossAnimator = boss.GetComponent<Animator>();
            // bossAnimator.enabled = false;

            // Create death sequence
            var sequencePrefab = gameManager.bossDeathSequence;
            UnityEngine.Object.Instantiate(sequencePrefab, boss.transform.position, sequencePrefab.transform.rotation);
        }
        

        public override void Exit() {
            base.Exit();
        }


        
    }
}