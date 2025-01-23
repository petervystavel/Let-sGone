using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : MonoBehaviour
{
    public float Duration = 1f;
    public float Progress = 0f;
    public float Damage = 1f;
    public float Intensity = 1f;

    List<GameObject> mAlreadyHitEnemy = new List<GameObject>();

    void Update()
    {
        Progress += Time.deltaTime;
        if (Progress >= Duration) 
        {
            gameObject.SetActive(false);
            Progress = 0;
            mAlreadyHitEnemy.Clear();

            return;
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy") == false)
            return;

        if(mAlreadyHitEnemy.Contains(other.gameObject))
            return;

        Vector3 direction = other.transform.position - transform.position;
        other.GetComponent<Enemy>().TakeDamage(Damage, direction.normalized, Intensity);

        mAlreadyHitEnemy.Add(other.gameObject);
    }
}
