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

    public float HitDuration = 0.1f;
    float mHitProgress = -1f;

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
        UpdateMove();
        UpdateHit();
    }

    private void UpdateMove()
    {
        if (NoMove)
            return;

        if (GameManager.Instance.FreezeAllEnemy)
            return;

        GameObject player = GameManager.Player;

        float distance = Vector3.Distance(player.transform.position, transform.position);

        if(distance > DetectionDistance)
            return;

        GetComponent<NavMeshAgent>().destination = player.transform.position;
    }

    private void UpdateHit()
    {
        if(mHitProgress < 0)
            return;

        mHitProgress += Time.deltaTime;

        if (mHitProgress < HitDuration)
            return;

        mHitProgress = -1;
        mRenderer.material.color = mBaseColor;
    }

    public void TakeDamage(float damage, Vector3 direction, float intensity, Enemy.Type sensibilityType) 
    {
        if (sensibilityType != mType)
            return;

        mHealth -= damage;
        if (mHealth <= 0)
        {
            Destroy(gameObject);
        }

        mHitProgress = 0;
        mRenderer.material.color = GameManager.Instance.White.color;

        GetComponent<Rigidbody>().AddForce(direction * intensity, ForceMode.Impulse);
    }
}
