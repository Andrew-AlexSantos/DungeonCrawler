using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NavigationExemplo : MonoBehaviour
{
    public GameObject target;
    private NavMeshAgent thisAgent;


    void Awake() {
        thisAgent = GetComponent<NavMeshAgent>();
    }
    // Update is called once per frame
    void Update() {
        thisAgent.SetDestination(target.transform.position);
    }
}
