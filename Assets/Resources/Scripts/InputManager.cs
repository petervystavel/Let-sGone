using Unity;
using UnityEngine;
using System;

class InputManager
{ 
    public static Vector3 GetLeftStick()
    {
        Vector3 oDirection;

        oDirection.x = Input.GetAxisRaw("LeftHorizontal");
        oDirection.y = 0f;
        oDirection.z = -Input.GetAxisRaw("LeftVertical");

        return oDirection;
    }

    public static Vector3 GetRightStick()
    {
        Vector3 oDirection;

        oDirection.x = Input.GetAxisRaw("RightHorizontal");
        oDirection.y = 0f;
        oDirection.z = -Input.GetAxisRaw("RightVertical");

        return oDirection;
    }
}