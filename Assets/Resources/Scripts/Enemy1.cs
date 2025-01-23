using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy1 : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        GetComponent<NavMeshAgent>().enabled = true;
        GetComponent<NavMeshAgent>().updatePosition = true;
        GetComponent<NavMeshAgent>().updateRotation = true;
        GetComponent<NavMeshAgent>().speed = 100;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = Utils.GetPlayer();

        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }
}
