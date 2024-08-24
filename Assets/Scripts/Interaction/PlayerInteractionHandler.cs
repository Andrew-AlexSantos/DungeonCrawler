using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Chest;
using EventsArgs;
using Unity.VisualScripting;
public class PlayerInteractionHandler : MonoBehaviour {

    private Interaction currentInteraction;   
    private readonly float scanInterval = 0.5f;
    private float scanCooldown = 0;

    void Start() {
        
    }


    void Update() {
        // Scan objects
        if ((scanCooldown -= Time.deltaTime) <= 0f) {
            scanCooldown = scanInterval;
            scanObjects();
        }

        // Process input
            if (Input.GetKeyDown(KeyCode.E)) {
            if (currentInteraction != null) {
                currentInteraction.Interact();
            }
        } 
        
    }

    private void scanObjects() {
        Interaction nearestInteraction = GetNearestInteraction(transform.position);
        if (nearestInteraction != currentInteraction) {
            currentInteraction?.SetActive(false);
            nearestInteraction?.SetActive(true);
            currentInteraction = nearestInteraction;
        }
    }

    public Interaction GetNearestInteraction(Vector3 position) {
            // Create cache
        float closestDst = -1;
        Interaction closestInteraction = null;

        // Interate through objects
        var interactionList = GameManager.Instance.interactionList;
        foreach (Interaction interaction in interactionList) {
            var dst = (interaction.transform.position - position).magnitude;
            var isAvailable = interaction.IsAvailable();
            var isCloseEnough = dst <= interaction.radius;
            var isCacheInvalid = closestDst < 0;
            if (isCloseEnough && isAvailable)
            {
                if (isCacheInvalid || dst < closestDst)
                {
                    closestDst = dst;
                    closestInteraction = interaction;
                }
            }
        }
        return closestInteraction;
    }    
}
