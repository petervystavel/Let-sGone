using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    public float Impulse = 200;
    public float KnockbackDuration = 0.2f;
    public int MaxLife = 3;

    int mLife;

    Timer mLifeOscillationTimer = new Timer(1f);
    Timer mLifeIndicatorTimer = new Timer(0.5f);

    GameObject mProjectileSpawn;

    SkinnedMeshRenderer mSkinnedRenderer;

    Enemy.Type mCurrentProjectileType;

    Color[] colors;

    Force mKnockback = null;

    bool[] mProjectilesPossessed = new bool[3];

    void Start()
    {
        mLife = MaxLife;
        mProjectileSpawn = transform.Find("ProjectileSpawn").gameObject;

        mSkinnedRenderer = transform.Find("RealRender").GetComponentInChildren<SkinnedMeshRenderer>();

        mCurrentProjectileType = Enemy.Type.None;

        for (int i = 0; i < mProjectilesPossessed.Length; ++i) 
        {
            mProjectilesPossessed[i] = false;
        }

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

        UpdateLife();

        if (mKnockback != null) 
        {
            bool end = mKnockback.Update(transform);

            if (end)
                mKnockback = null;
        }

        HandleInput();
    }

    bool mLifeIndicatorMax = false;
    private void UpdateLife()
    {
        if (mLifeOscillationTimer.Update()) 
            mLifeIndicatorTimer.Start();

        if (mLifeOscillationTimer.IsRunning())
            return;

        if (mLifeIndicatorTimer.IsRunning() == false)
            return;

        float ratio = mLifeIndicatorTimer.GetAcomplishedRatio();

        Color color = GameManager.Instance.Red.color;

        for (int i = 0; i < mSkinnedRenderer.materials.Length; ++i) 
        {
            if (i == 2)
                continue;

            Color colorStart = mLifeIndicatorMax ? color : colors[i];
            Color colorEnd = mLifeIndicatorMax ? colors[i] : color;

            mSkinnedRenderer.materials[i].color = Color.Lerp(colorStart, colorEnd, ratio);
        }

        if (mLifeIndicatorTimer.Update())
        {
            if (mLifeIndicatorMax)
            {
                mLifeIndicatorMax = false;
                mLifeOscillationTimer.Start();
                ResetColor();
            }  
            else
            {
                mLifeIndicatorMax = true;
                mLifeIndicatorTimer.Start();
            }
        }
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
                gameObject.GetComponentInChildren<Animator>().SetBool("BoolRun", false);
                moveDirection = Vector3.zero;
            }
            else
            {
                gameObject.GetComponentInChildren<Animator>().SetBool("BoolRun", true);
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

                LoopAudio.Instance.PlaySFXShoot();
            }
        }
        else if (Input.GetButtonUp("LB"))
        {
            NextProjectile();
        }
    }

    private void NextProjectile() 
    {
        if (mCurrentProjectileType == Enemy.Type.None)
            return;

        for( int i = ((int)mCurrentProjectileType + 1) % 3; i < mProjectilesPossessed.Length + 3; ++i)
        {
            int index = i % 3;

            if (mProjectilesPossessed[index])
            {
                SetProjectileType((Enemy.Type)index);
                return;
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

    public void SetProjectilePossessed(Enemy.Type type)
    {
        mProjectilesPossessed[(int)type] = true;

        if (mCurrentProjectileType == Enemy.Type.None)
            SetProjectileType(type);
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

        mKnockback = new Force(new Vector2(direction.x, direction.z), Impulse, KnockbackDuration);

        AttackColor.Start();

        SetColor(GameManager.Instance.Red.color);

        Invincibility.Start();

        TakeDamage();
    }

    public void TakeDamage()
    {
        mLife -= 1;

        if (mLife <= 0)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
        else 
        {
            float duration = 1f;
            if (mLife == 1) 
            {
                duration = 0.1f;
            }

            mLifeOscillationTimer.Start(duration);
            mLifeIndicatorTimer.Start();
        }
    }
}
