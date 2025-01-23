using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mSpeed = 5f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = InputManager.GetLeftStick();
        if (direction.magnitude < 0.5f)
            direction = Vector3.zero;

        direction.Normalize();

        Vector3 velocity = direction * mSpeed;

        Debug.Log(velocity.magnitude);

        GetComponent<Rigidbody>().velocity = velocity;
    }
}
