using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Duration = 1f;
    float mProgress = 0f;
    public float Damage = 1f;
    public float Intensity = 1f;
    public bool ContinueDamage = false;

    public float DamageInterval = 0.1f;
    private float mDamageIntervalProgress = -1f;

    List<GameObject> mAlreadyHitEnemy = new List<GameObject>();

    void Update()
    {
        mProgress += Time.deltaTime;

        if (mDamageIntervalProgress >= 0)
        {
            mDamageIntervalProgress += Time.deltaTime;
            if (mDamageIntervalProgress >= DamageInterval)
            {
                mDamageIntervalProgress = -1;
            }
        }

        if (mProgress >= Duration) 
        {
            gameObject.SetActive(false);
            mProgress = 0;
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
        other.GetComponent<Enemy>().TakeDamage(Damage, direction.normalized, Intensity);

        mAlreadyHitEnemy.Add(other.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if (ContinueDamage == false)
            return;

        if (other.CompareTag("Enemy") == false)
            return;

        if (mDamageIntervalProgress >= 0)
            return;

        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Enemy>().TakeDamage(Damage, direction.normalized, Intensity);
        mDamageIntervalProgress = 0;
    }
}
