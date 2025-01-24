using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float Damage = 1f;
    public float Intensity = 10f;
    public float Speed = 20f;

    Enemy.Type mType;

    public void Initialize(Vector3 direction, Enemy.Type type) 
    {
        transform.forward = direction;
        mType = type;

        GameObject[] types = new GameObject[3];
        types[0] = transform.Find("1").gameObject;
        types[1] = transform.Find("2").gameObject;
        types[2] = transform.Find("3").gameObject;

        for(int i = 0; i < types.Length; i++)
            types[i].SetActive(false);

        types[(int)type].SetActive(true);
    }

    void Update()
    {
        GetComponent<Rigidbody>().velocity = transform.forward * Speed;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
            return;

        if (other.CompareTag("Enemy")) 
        {
            Enemy enemy = other.GetComponent<Enemy>();

            if (enemy != null) 
            {
                Vector3 direction = (other.transform.position - transform.position).normalized;
                enemy.TakeDamage(Damage, direction, Intensity, mType);
            }
        }

        Destroy(gameObject);
    }
}
