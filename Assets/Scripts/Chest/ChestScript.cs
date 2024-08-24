using System;
using EventsArgs;
using Item;
using UnityEngine;
using UnityEngine.Events;

namespace Chest
{
    public class ChestScript : MonoBehaviour {
        public Interaction interaction;
        public Item.Item item;
        public GameObject itemHolder;
        public ChestOpenEvent onOpen = new();

        private Animator thisAnimator;

        private void Awake() {
            thisAnimator = GetComponent<Animator>();
        }


        void Start()
        {
            interaction.OnInteraction += OnInteraction;
            interaction.SetActionText("open chest");
        }
            void Update() {

            }

            private void OnInteraction(object sender, InteractionEventArgs args) {
                Debug.Log("jogador interagiu com o bau, contendo item " + item.displayName + "!");
                //if (itemType == ItemType.Key) {}
                // Disable interaction
                interaction.SetAvailable(false);

                // Update Animator
                thisAnimator.SetTrigger("tOpen");

                // Create item object
                var itemObjectPrefab = item.ObjectPrefab;
                var rotation = item.ObjectPrefab.transform.rotation;
                var position = itemHolder.transform.position;
                var itemObject = Instantiate(item.ObjectPrefab, position, rotation);
                itemObject.transform.localScale = new Vector3(2, 2, 2);
                itemObject.transform.SetParent(itemHolder.transform);

                // Update inventory
                var itemType = item.itemType;
                if (itemType == ItemType.Key) {
                    GameManager.Instance.keys++;
                } else if (itemType == ItemType.BossKey) {
                    GameManager.Instance.hasBossKey = true;
                } else if (itemType == ItemType.Potion) {
                    var player = GameManager.Instance.player;
                    var playerLife = player.GetComponent<LifeScript>();
                playerLife.Heal();
                }

            // Call events
            onOpen?.Invoke(gameObject);

            // Update UI
            var gameplayUI = GameManager.Instance.gameplayUI;
            gameplayUI.AddObject(itemType);
            }
         }

         [Serializable] public class ChestOpenEvent : UnityEvent<GameObject> {}
     } 
  