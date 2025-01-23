using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float mSpeed = 5f;

    Vector3 mFaceDirection;

    GameObject mCAC;
    GameObject mAOE;

    // Start is called before the first frame update
    void Start()
    {
        mCAC = GameObject.Find("CAC");
        mAOE = GameObject.Find("AOE");

        mCAC.SetActive(false);
        mAOE.SetActive(false);
    }

    void Update()
    {
        Vector3 direction = InputManager.GetLeftStick();
        if (direction.magnitude < 0.5f)
            direction = Vector3.zero;

        direction.Normalize();

        Debug.Log(direction);

        LookAt(direction);

        Vector3 velocity = direction * mSpeed;

        GetComponent<Rigidbody>().velocity = velocity;

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
            //#TODO
        }
    }

    public bool LookAt(Vector3 oDirection)
    {
        if (Utils.IsNaN(ref oDirection))
            return false;

        transform.LookAt(transform.position + oDirection);

        mFaceDirection = oDirection;

        return true;
    }
}
