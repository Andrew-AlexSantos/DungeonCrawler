using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldHitBoxScript : MonoBehaviour
{

    public PlayerController playerController;



    private void OnTriggernter(Collider collision) {
        playerController.OnShieldColliderEnter(collision);
    }

}
