using System;
using UnityEngine;


namespace DungeonCrawler.Assets.Scripts.BossBattle {
    public class Disabled : State {
        public Disabled() : base("Disabled") {

         }

        public override void Enter() {
            base.Enter();

            GameManager.Instance.boss.SetActive(false);
        }

        public override void Exit() {
            base.Exit();

            GameManager.Instance.boss.SetActive(true);
        }


        
    }
}