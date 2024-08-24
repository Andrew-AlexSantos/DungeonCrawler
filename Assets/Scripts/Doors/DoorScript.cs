using System.Collections;
using System.Collections.Generic;
using DungeonCrawler.Assets.Scripts;
using DungeonCrawler.Assets.Scripts.EventArgs;
using EventsArgs;
using Item;
using UnityEngine;

public class DoorScript : MonoBehaviour
{
    public Interaction interaction;
    public Item.Item requiredKey;
    private Animator thisAnimator;
    private bool isOpen;

    private void Awake()
    {
        thisAnimator = GetComponent<Animator>();
    }
    void Start()
    {
        interaction.OnInteraction += OnInteraction;
        interaction.SetActionText("open door");
    }

    void Update()
    {
        // if door is still closed...
        if (!isOpen)
        {
            // check if player has key
            var hasKey = false;
            if (requiredKey == null) {
                hasKey = true;
            }
            else if (requiredKey.itemType == ItemType.Key) {
                hasKey = GameManager.Instance.keys > 0;
            }

            else if (requiredKey.itemType == ItemType.BossKey) {
                hasKey = GameManager.Instance.hasBossKey;
            }

            // switch (requiredKey.itemType)
            // {
            //     case ItemType.Key:
            //         hasKey = GameManager.Instance.keys > 0;
            //         break;
            //     case ItemType.BossKey:
            //         hasKey = GameManager.Instance.hasBossKey;
            //         break;
            //     default:
            //         if (requiredKey == null) {
            //             hasKey = true;
            //         }
            //         break;

            // Toggle availability 
            interaction.SetAvailable(hasKey);
        }

    }

    private void OnInteraction(object sender, InteractionEventArgs args)
    {
        Debug.Log("jogador interagiu com a porta!");

        if (!isOpen)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }
    }
    private void OpenDoor()
    {
        // Set as open 
        isOpen = true;

        // Take key
        GameManager.Instance.keys--;
        
        if (requiredKey != null) {
            if (requiredKey.itemType == ItemType.Key)
                GameManager.Instance.keys--;
        }
        else if (requiredKey.itemType == ItemType.BossKey) {
            GameManager.Instance.hasBossKey = false;
        }

        // Update UI
        
            var gameplayUI = GameManager.Instance.gameplayUI;
            gameplayUI.RemoveObject(requiredKey.itemType);
        

        // Disable interaction
        interaction.SetAvailable(false);

        // Update Animator
        thisAnimator.SetTrigger("tOpen");
    }

    private void CloseDoor()
    {
        // Set as open 
        isOpen = false;

        // Disable interaction
        interaction.SetAvailable(false);

        // Update Animator
        thisAnimator.SetTrigger("tClose");

        // Boss door
        var isBossDoor = requiredKey.itemType == ItemType.BossKey;
        if (isBossDoor)
        {
            GlobalEvent.Instance.InvokeOnBossDoorOpen(this, new BossDoorOpenArgs());

        }

    }

}