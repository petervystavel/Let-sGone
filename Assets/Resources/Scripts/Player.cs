using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 12;
    public float SpawnRadius = 20;
    public float SpawnCountMin = 1;
    public float SpawnCountMax = 1;
    public float SpawnInterval = 1;
    float SpawnProgress = 0;

    Vector3 mLookDirection;

    GameObject mCAC;
    GameObject mAOE;
    GameObject mLaser;
    GameObject mEye;

    List<Type> mTypesToWarp = new List<Type>();

    // Start is called before the first frame update
    void Start()
    {
        mCAC = transform.Find("CAC").gameObject;
        mAOE = transform.Find("AOE").gameObject;
        mLaser = transform.Find("Laser").gameObject;
        mEye = transform.Find("Eye").gameObject;

        mCAC.SetActive(false);
        mAOE.SetActive(false);
        mLaser.SetActive(false);
    }

    void Update()
    {
        HandleInput();
        TrySpawnEnemy();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
    }

    private void TrySpawnEnemy() 
    {
        /*
        SpawnProgress += Time.deltaTime;
        if (SpawnProgress >= SpawnInterval)
        {
            SpawnProgress = 0;
            int count = UnityEngine.Random.Range((int)SpawnCountMin, (int)SpawnCountMax);
            for (int i = 0; i < count; i++)
            {
                Vector2 v2 = UnityEngine.Random.insideUnitCircle * SpawnRadius;
                Vector3 v3 = new Vector3(v2.x, 0, v2.y);
                Vector3 spawnPosition = transform.position + v3;
                spawnPosition.y = 0;
                GameObject enemy = Instantiate(GameManager.Instance.Enemy1);

                enemy.GetComponent<Enemy>().Initialize(spawnPosition);
            }
        }
        */
    }

    private void HandleInput()
    {
        //Sticks
        Vector3 moveDirection = InputManager.GetLeftStick();
        Vector3 lookDirection = InputManager.GetRightStick();
        if (moveDirection.magnitude < 0.5f)
            moveDirection = Vector3.zero;

        if (lookDirection.magnitude < 0.1f)
            lookDirection = moveDirection;

        moveDirection.Normalize();
        lookDirection.Normalize();

        LookAt(lookDirection);

        Vector3 velocity = moveDirection * Speed;

        GetComponent<Rigidbody>().velocity = velocity;

        //Buttons
        if (Input.GetButtonDown("X"))
        {
            mCAC.SetActive(true);
        }
        else if (Input.GetButtonDown("Y"))
        {
            mAOE.SetActive(true);
        }
        else if (Input.GetButtonDown("RB"))
        {
            mLaser.SetActive(true);
        }
        else if (Input.GetButtonUp("RB"))
        {
            mLaser.SetActive(false);
        }
    }

    public bool LookAt(Vector3 oDirection)
    {
        if (Utils.IsNaN(ref oDirection))
            return false;

        transform.LookAt(transform.position + oDirection);

        mLookDirection = oDirection;

        return true;
    }
}
