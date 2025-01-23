using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public float Speed = 12;

    Vector3 mLookDirection;

    GameObject mCAC;
    GameObject mAOE;
    GameObject mLaser;
    GameObject mEye;

    // Start is called before the first frame update
    void Start()
    {
        mCAC = transform.Find("CAC").gameObject;
        mAOE = transform.Find("AOE").gameObject;
        mLaser = transform.Find("Laser").gameObject;
        mEye = transform.Find("Eye").gameObject;

        mCAC.SetActive(false);
        mAOE.SetActive(false);
        mLaser.SetActive(false);
    }

    void Update()
    {
        Vector3 moveDirection = InputManager.GetLeftStick();
        Vector3 lookDirection = InputManager.GetRightStick();
        if (moveDirection.magnitude < 0.5f)
            moveDirection = Vector3.zero;

        if(lookDirection.magnitude < 0.1f)
            lookDirection = moveDirection;

        moveDirection.Normalize();
        lookDirection.Normalize();

        LookAt(lookDirection);

        Vector3 velocity = moveDirection * Speed;

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
            mLaser.SetActive(true);
        }
        else if (Input.GetButtonUp("RB"))
        {
            mLaser.SetActive(false);
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
}
