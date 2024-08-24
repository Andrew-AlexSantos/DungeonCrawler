using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FaceCamera : MonoBehaviour
{
    // Start is called before the first frame update

    private Camera thisCamera;
    void Start() {
        thisCamera = Camera.main;
        
    }

    // Update is called once per frame
    void Update() {
        
        transform.LookAt(thisCamera.transform);
        
    }
}
