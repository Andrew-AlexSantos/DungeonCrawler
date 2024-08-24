using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Behaviors.LichBoss
{
    public class LichBossHelper : MonoBehaviour {

        private LichBossController controller;

        public LichBossHelper(LichBossController controller) {
            this.controller = controller;
        }

        public float GetDistanceToPlayer() {
            var player = GameManager.Instance.player;
            var playerPosition = player.transform.position;
            var origin = controller.transform.position;
            var positionDifference = playerPosition - origin;
            var distance = positionDifference.magnitude;
            return distance;
        }

        public bool HasLowHealth()
        {
            var life = controller.thisLife;
            var lifeRate = (float)life.health / (float)life.maxHealth;
            return lifeRate <= controller.lowHealthTreshold;
        }

        public void StartStateCoroutine(IEnumerator enumerator) {
            controller.StartCoroutine(enumerator);
            controller.stateCoroutines.Add(enumerator);
        }

        public void ClearStateCoroutines() {
            foreach (var enumerator in controller.stateCoroutines) {
                controller.StopCoroutine(enumerator);
            }
            controller.stateCoroutines.Clear();
        }
    }
}