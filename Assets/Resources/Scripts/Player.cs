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

    MeshRenderer mMeshRenderer;

    Enemy.Type mCurrentProjectileType;
    int mMaxProjectileType;

    // Start is called before the first frame update
    void Start()
    {
        mCAC = transform.Find("CAC").gameObject;
        mAOE = transform.Find("AOE").gameObject;
        mLaser = transform.Find("Laser").gameObject;
        mEye = transform.Find("Eye").gameObject;

        mMeshRenderer = transform.Find("RealRender").GetComponent<MeshRenderer>();

        mCAC.SetActive(false);
        mAOE.SetActive(false);
        mLaser.SetActive(false);

        mCurrentProjectileType = Enemy.Type.None;
        mMaxProjectileType = 3;
    }

    void Update()
    {
        HandleInput();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, SpawnRadius);
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

        if (Input.GetButtonDown("RB"))
        {
            mLaser.SetActive(true);
        }
        else if (Input.GetButtonUp("RB"))
        {
            mLaser.SetActive(false);
        }
        else if (Input.GetButtonUp("LB"))
        {
            if (mMaxProjectileType != 0) 
            {
                Enemy.Type nextProjectileType = (Enemy.Type)((((int)mCurrentProjectileType) + 1) % mMaxProjectileType);

                SetProjectileType(nextProjectileType);
            }
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

    private void SetProjectileType(Enemy.Type type)
    {
        mCurrentProjectileType = type;

        Material[] materials = new Material[3];
        materials[0] = GameManager.Instance.Projectile1;
        materials[1] = GameManager.Instance.Projectile2;
        materials[2] = GameManager.Instance.Projectile3;

        Debug.Log(materials[(int)type].ToString());

        mMeshRenderer.materials[2].color = materials[(int)type].color;
    }

    private void IncrementMaxProjectileType() 
    {
       mMaxProjectileType += 1;
    }
}
