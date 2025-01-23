using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 3;
    public float Speed = 10;
    float mHealth;

    NavMeshAgent mNavMeshAgent;

    // Start is called before the first frame update
    void Start()
    {
        mHealth = MaxHealth;

        mNavMeshAgent = GetComponent<NavMeshAgent>();
        mNavMeshAgent.updateRotation = true;
        mNavMeshAgent.autoBraking = false;
        mNavMeshAgent.angularSpeed = 20000;
        mNavMeshAgent.acceleration = 20000;
        mNavMeshAgent.speed = Speed;
        mNavMeshAgent.enabled = true;
        mNavMeshAgent.updatePosition = true;
        mNavMeshAgent.updateRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameManager.Player;

        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    public void TakeDamage(float damage, Vector3 direction, float intensity) 
    {
        mHealth -= damage;
        if (mHealth <= 0)
        {
            Destroy(gameObject);
        }

        mNavMeshAgent.updatePosition = false;
        GetComponent<Rigidbody>().AddForce(direction * intensity, ForceMode.Impulse);
    }
}
