using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type 
    {
        None = -1,

        Enemy1,
        Enemy2,
        Enemy3,

        Count
    }

    protected Type mType;

    public float MaxHealth = 3;
    public float Speed = 10;
    public float DetectionDistance = 10;
    public bool NoMove = false;

    float mHealth;

    public Timer AttackColor = new Timer(1f);
    public Timer HurtColor = new Timer(0.2f);
    public Timer BeforeDie = new Timer(0.5f);

    NavMeshAgent mNavMeshAgent;

    Color mBaseColor;

    GameObject mRealRender;
    Renderer mRenderer;
    GameObject mCAC;

    void Start()
    {
        mNavMeshAgent = GetComponent<NavMeshAgent>();

        mHealth = MaxHealth;

        mNavMeshAgent.updateRotation = true;
        mNavMeshAgent.autoBraking = false;
        mNavMeshAgent.angularSpeed = 20000;
        mNavMeshAgent.acceleration = 20000;
        mNavMeshAgent.speed = Speed;
        mNavMeshAgent.enabled = true;
        mNavMeshAgent.updatePosition = true;
        mNavMeshAgent.updateRotation = true;

        mRealRender = transform.Find("RealRender").gameObject;
        mRenderer = mRealRender.GetComponent<Renderer>();
        mBaseColor = mRenderer.material.color;

        //mCAC = transform.Find("CAC").gameObject;

        OnStart();
    }

    protected virtual void OnStart() { }

    public void Initialize(Vector3 position)
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(position, out hit, 10, NavMesh.AllAreas))
        {
            mNavMeshAgent.Warp(hit.position);
        }
        else
        {
            Debug.Log("Cannot find position on navmesh");
        }
    }

    void Update()
    {
        if (HurtColor.Update()) 
        {
            mRenderer.material.color = mBaseColor;
        }

        if (AttackColor.Update()) 
        {
            mRenderer.material.color = mBaseColor;
        }

        if (BeforeDie.Update())
        {
            Destroy(gameObject);
        }
        else if (BeforeDie.IsRunning() == false)
        {
            UpdateMove();
        }
    }

    private void UpdateMove()
    {
        if (NoMove)
            return;

        if (GameManager.Instance.FreezeAllEnemy)
            return;

        if (AttackColor.IsRunning())
            return;

        GameObject player = GameManager.Player;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance > DetectionDistance)
            return;

        mNavMeshAgent.destination = player.transform.position;
    }

    public void TakeDamage(float damage, Vector3 direction, float intensity, Enemy.Type sensibilityType) 
    {
        if (sensibilityType != mType)
            return;

        if (BeforeDie.IsRunning())
            return;

        mHealth -= damage;
        if (mHealth <= 0)
        {
            BeforeDie.Start();
            mNavMeshAgent.enabled = false;
        }

        //#TODO feedback
        mRenderer.material.color = GameManager.Instance.White.color;

        GetComponent<Rigidbody>().AddForce(direction * intensity, ForceMode.Impulse);

        HurtColor.Start();
    }

    public void Attack()
    {
        AttackColor.Start();
        mRenderer.material.color = GameManager.Instance.Red.color;
    }
}
