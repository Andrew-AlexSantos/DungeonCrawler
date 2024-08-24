using System;
using UnityEngine;


namespace DungeonCrawler.Assets.Scripts.BossBattle {
    public class Intro : State {

        private readonly float duration = 3f;
        private float timeElapsed = 0;


        public Intro() : base("Intro") {
         }

         

        public override void Enter() {
            base.Enter();

            // Reset stuffs
            timeElapsed = 0;

            // Enable hidden parts
            GameManager.Instance.bossBattleParts.SetActive(true);

            // Stop gameplay music
            var gameManager = GameManager.Instance;
            var gameplayMusic = gameManager.gameplayMusic;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(gameplayMusic, 0, 2f));
            //gameplayMusic.Stop();

            // Play boss music
            var bossMusic = gameManager.bossMusic;
            var bossMusicVolume = bossMusic.volume;
            bossMusic.volume = 0;
            gameManager.StartCoroutine(FadeAudioSource.StartFade(bossMusic, bossMusicVolume, 0.5f));
            bossMusic.Play();

        }



        public override void Exit() {
            base.Exit();

        }

        public override void Update() {
            base.Update();
            timeElapsed += Time.deltaTime;
            if( timeElapsed >= duration) {
                var bossBattleHandler = GameManager.Instance.bossBattleHandler;
                bossBattleHandler.stateMachine.ChangeState(bossBattleHandler.stateBattle);
            }
        }



    }
}