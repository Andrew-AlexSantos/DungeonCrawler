using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttachTO : MonoBehaviour
{

    public GameObject ObjectToFollow;


    // Start is called before the first frame update
    void Start() {
        
    }

    // Update is called once per frame
    void Update() {
        if (ObjectToFollow != null)
        {
            transform.position = ObjectToFollow.transform.position;
        }
    }
}
