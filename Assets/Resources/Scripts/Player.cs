using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class Force 
{
    Vector2 mDirection;
    float mMaxIntensity;
    float mIntensity;
    Timer mDuration;

    public Force(Vector2 direction, float intensity, float duration)
    {
        mDirection = direction;
        mMaxIntensity = intensity;
        mIntensity = mMaxIntensity;
        mDuration = new Timer(duration);
        mDuration.Start();
    }

    public bool Update(Transform transform) 
    {
        if (mDuration.IsRunning() == false)
            return true;

        mDuration.Update();

        float ratio = mDuration.GetAcomplishedRatio();

        mIntensity = Mathf.Lerp(mMaxIntensity, 0, ratio);

        Vector2 impulse = mDirection * mIntensity;

        Vector3 velocity = new Vector3(impulse.x, 0, impulse.y);

        transform.position = transform.position + velocity * Time.deltaTime;

        return false;
    }
}

public class Player : MonoBehaviour
{
    public float Speed = 12;
    public Timer ProjectileIntervalShoot = new Timer(0.1f);
    public Timer Invincibility = new Timer(0.5f);
    public Timer AttackColor = new Timer(0.1f);
    public float Knockback = 500;
    public float KnockbackDuration = 0.2f;

    GameObject mCAC;
    GameObject mAOE;
    GameObject mLaser;
    GameObject mProjectileSpawn;

    SkinnedMeshRenderer mSkinnedRenderer;

    Enemy.Type mCurrentProjectileType;
    int mMaxProjectileType;

    Color[] colors;

    Force mKnockback = null;

    void Start()
    {
        mCAC = transform.Find("CAC").gameObject;
        mAOE = transform.Find("AOE").gameObject;
        mLaser = transform.Find("Laser").gameObject;
        mProjectileSpawn = transform.Find("ProjectileSpawn").gameObject;

        mSkinnedRenderer = transform.Find("RealRender").GetComponentInChildren<SkinnedMeshRenderer>();

        mCAC.SetActive(false);
        mAOE.SetActive(false);
        mLaser.SetActive(false);

        mCurrentProjectileType = Enemy.Type.None;
        mMaxProjectileType = 3;

        colors = new Color[mSkinnedRenderer.materials.Length];

        for (int i = 0; i < mSkinnedRenderer.materials.Length; ++i) 
        {
            colors[i] = mSkinnedRenderer.materials[i].color;
        }
    }

    void Update()
    {
        Invincibility.Update();
        ProjectileIntervalShoot.Update();

        if(AttackColor.Update())
        {
            ResetColor();
        }

        if (mKnockback != null) 
        {
            bool end = mKnockback.Update(transform);

            if (end)
                mKnockback = null;
        }

        HandleInput();
    }

    private void HandleInput()
    {
        if (mKnockback == null)
        {        
            //Sticks
            Vector3 moveDirection = InputManager.GetLeftStick();
            Vector3 lookDirection = InputManager.GetRightStick();
            if (moveDirection.magnitude < 0.5f)
            {
                GetComponentInChildren<Animator>().SetTrigger("TriggerIdle");
                moveDirection = Vector3.zero;
            }
            else 
            {
                GetComponentInChildren<Animator>().SetTrigger("TriggerRun");
            }

            if (lookDirection.magnitude < 0.1f)
                lookDirection = moveDirection;

            moveDirection.Normalize();
            lookDirection.Normalize();

            LookAt(lookDirection);

            Vector3 velocity = moveDirection * Speed;

            GetComponent<Rigidbody>().velocity = velocity;
        }

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

        colors[2] = materials[(int)type].color;
        mSkinnedRenderer.materials[2].color = materials[(int)type].color;
    }

    private void SetColor(Color color) 
    {
        for (int i = 0; i < mSkinnedRenderer.materials.Length; ++i) 
        {
            mSkinnedRenderer.materials[i].color = color;
        }
    }

    private void ResetColor() 
    {
        for (int i = 0; i < mSkinnedRenderer.materials.Length; ++i)
        {
            mSkinnedRenderer.materials[i].color = colors[i];
        }
    }

    private void IncrementMaxProjectileType() 
    {
       mMaxProjectileType += 1;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.CompareTag("Enemy") == false)
            return;

        if (Invincibility.IsRunning())
            return;

        Enemy enemy = collision.gameObject.GetComponent<Enemy>();
        if (enemy == null)
            return;

        enemy.Attack();

        Vector3 direction = (transform.position - collision.transform.position).normalized;

        //GetComponent<Rigidbody>().AddForce(direction * Knockback, ForceMode.Impulse);

        mKnockback = new Force(new Vector2(direction.x, direction.z), Knockback, KnockbackDuration);

        AttackColor.Start();

        SetColor(GameManager.Instance.Red.color);

        Invincibility.Start();
    }
}
