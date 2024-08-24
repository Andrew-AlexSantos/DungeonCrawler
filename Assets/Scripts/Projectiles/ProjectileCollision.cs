
using UnityEngine;

namespace DungeonCrawler.Assets.Scripts.Projectiles
{
    public class ProjectileCollision : MonoBehaviour  {

        public GameObject hitEffect;
        [HideInInspector] public GameObject attacker;
        [HideInInspector] public int damage;

        private void Update() {

        }

        private void Start()  {

        }

        private void OnCollisionEnter(Collision collision) {

            Debug.Log("Acertou" + collision.gameObject.name);

            // Process player collision
            var hitObject = collision.gameObject;
            var hitLayer = hitObject.layer;
            var collidedWithPlayer = hitLayer == LayerMask.NameToLayer("Player");
            if (collidedWithPlayer) {
                var hitLife = hitObject.GetComponent<LifeScript>();
                if (hitLife != null) {
                    hitLife.InflicDamage(attacker, damage);
                }

                
            }

            // Create effect
            if (hitEffect != null) {
                var effect = Instantiate(hitEffect, transform.position, transform.rotation);
                Destroy(effect, 10);
            }

            // Destroy Projectile
            Destroy(gameObject);

        }
    }
}