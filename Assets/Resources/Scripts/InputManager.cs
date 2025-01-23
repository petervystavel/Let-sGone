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

    public static Vector2 GetRightStick()
    {
        Vector2 oDirection;

        oDirection.x = Input.GetAxisRaw("RightHorizontal");
        oDirection.y = 0f;
        oDirection.y = -1f * Input.GetAxisRaw("RightVertical");

        return oDirection;
    }
}