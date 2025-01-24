using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 12;
    public Timer ProjectileIntervalShoot = new Timer(0.1f);

    GameObject mCAC;
    GameObject mAOE;
    GameObject mLaser;
    GameObject mProjectileSpawn;

    MeshRenderer mMeshRenderer;

    Enemy.Type mCurrentProjectileType;
    int mMaxProjectileType;

    // Start is called before the first frame update
    void Start()
    {
        mCAC = transform.Find("CAC").gameObject;
        mAOE = transform.Find("AOE").gameObject;
        mLaser = transform.Find("Laser").gameObject;
        mProjectileSpawn = transform.Find("ProjectileSpawn").gameObject;

        mMeshRenderer = transform.Find("RealRender").GetComponent<MeshRenderer>();

        mCAC.SetActive(false);
        mAOE.SetActive(false);
        mLaser.SetActive(false);

        mCurrentProjectileType = Enemy.Type.None;
        mMaxProjectileType = 3;
    }

    void Update()
    {
        ProjectileIntervalShoot.Update();
        HandleInput();
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
            if ((int)mCurrentProjectileType != -1 && ProjectileIntervalShoot.IsRunning() == false)
            {
                GameObject proj = Instantiate(GameManager.Instance.ProjectilePrefab, mProjectileSpawn.transform.position, Quaternion.identity);

                proj.GetComponent<Projectile>().Initialize(transform.forward, mCurrentProjectileType);

                ProjectileIntervalShoot.Start();
            }
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
        if(Utils.AreAquals(oDirection, Vector3.zero))
            return false;

        if (Utils.IsNaN(ref oDirection))
            return false;

        transform.LookAt(transform.position + oDirection);

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
