using System;
using UnityEngine;

namespace DungeonCrawler.Assets.Scripts.BossBattle
{
    public class Waiting : State {
        
        public Waiting() : base("Waiting") { }


        public override void Enter() { 
            base.Enter();
            
        }

        public override void Exit() {
            base.Exit();
        }

    }
}