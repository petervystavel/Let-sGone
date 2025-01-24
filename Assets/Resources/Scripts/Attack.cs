using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public Timer Duration = new Timer(0.2f);

    public float Damage = 1f;
    public float Intensity = 1f;
    public bool ContinueDamage = false;

    public Timer DamageInterval = new Timer(0.1f);

    List<GameObject> mAlreadyHitEnemy = new List<GameObject>();

    public Enemy.Type SensibilityType;

    protected void OnEnable()
    {
        Duration.Start();
    }

    void Update()
    {
        DamageInterval.Update();

        if (Duration.Update()) 
        {
            gameObject.SetActive(false);
            mAlreadyHitEnemy.Clear();

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == false)
            return;

        if (ContinueDamage == false && mAlreadyHitEnemy.Contains(other.gameObject))
            return;

        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Enemy>().TakeDamage(Damage, direction.normalized, Intensity, SensibilityType);

        mAlreadyHitEnemy.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (ContinueDamage == false)
            return;

        if (other.CompareTag("Enemy") == false)
            return;

        if (DamageInterval.IsRunning())
            return;

        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Enemy>().TakeDamage(Damage, direction.normalized, Intensity, SensibilityType);
        DamageInterval.Start();
    }
}
