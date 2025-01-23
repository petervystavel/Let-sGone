using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public float MaxHealth = 3;
    public float Speed = 10;
    float mHealth;

    // Start is called before the first frame update
    void Start()
    {
        mHealth = MaxHealth;

        if (TryGetComponent(out NavMeshAgent oNavMeshAgent))
        {
            oNavMeshAgent.updateRotation = true;
            oNavMeshAgent.autoBraking = false;
            oNavMeshAgent.angularSpeed = 20000;
            oNavMeshAgent.acceleration = 20000;
            oNavMeshAgent.speed = Speed;
            oNavMeshAgent.enabled = true;
            oNavMeshAgent.updatePosition = true;
            oNavMeshAgent.updateRotation = true;
        }
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

        GetComponent<Rigidbody>().AddForce(direction * intensity, ForceMode.Force);
    }
}
