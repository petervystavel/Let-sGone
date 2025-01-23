using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Enemy : MonoBehaviour
{
    public enum Type 
    {
        Enemy1,
        Enemy2,
        Enemy3
    }

    protected Type mType;

    public float MaxHealth = 3;
    public float Speed = 10;

    float mHealth;

    public float HitDuration = 0.1f;
    float mHitProgress = -1f;

    NavMeshAgent mNavMeshAgent;

    Color mBaseColor;

    GameObject mRealRender;
    Renderer mRenderer;

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

        mRealRender = transform.Find("RealRender").gameObject;
        mRenderer = mRealRender.GetComponent<Renderer>();
        mBaseColor = mRenderer.material.color;

        OnStart();
    }

    protected virtual void OnStart() { }

    // Update is called once per frame
    void Update()
    {
        GameObject player = GameManager.Player;

        if(GameManager.Instance.FreezeAllEnemy == false)
        {
            GetComponent<NavMeshAgent>().destination = player.transform.position;
        }

        if (mHitProgress >= 0)
        {
            mHitProgress += Time.deltaTime;
            if (mHitProgress >= HitDuration)
            {
                mHitProgress = -1;
                mRenderer.material.color = mBaseColor;
            }
        }
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
