using UnityEngine;

namespace DungeonCrawler.Assets.Scripts.BossBattle
{
    public class BossBattleHandler
    {

        public StateMachine stateMachine;
        public Disabled stateDisabled;
        public Waiting stateWaiting;
        public Intro stateIntro;
        public Battle stateBattle;
        public BossDefeated stateDefeated;
        public BossVictorius stateVictorius;


        public BossBattleHandler()
        {

            // Create state machine
            stateMachine = new StateMachine();
            stateDisabled = new Disabled();
            stateWaiting = new Waiting();
            stateIntro = new Intro();
            stateBattle = new Battle();
            stateDefeated = new BossDefeated();
            stateVictorius = new BossVictorius();
            stateMachine.ChangeState(stateDisabled);

            // Disable hidden parts
            GameManager.Instance.bossBattleParts.SetActive(false);

            // Register listeners
            var globalEvents = GlobalEvent.Instance;
            globalEvents.OnBossDoorOpen += (_, _) => stateMachine.ChangeState(stateWaiting);
            globalEvents.OnBossRoomEnter += (_, _) => stateMachine.ChangeState(stateIntro);
            globalEvents.OnGameOver += (_, _) => stateMachine.ChangeState(stateVictorius);
            globalEvents.OnGameWon += (_, _) => stateMachine.ChangeState(stateDefeated);
        }

        public void Update() {
            stateMachine.Update();
        }
        public bool IsActive() {
            return stateMachine.currentStateName == stateBattle.name;
        }

        public bool IsInCutscene() {
            return stateMachine.currentStateName == stateIntro.name;
        }
    }
}