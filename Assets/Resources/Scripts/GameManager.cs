using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject Player;

    private void Awake()
    {
        Player = GameObject.Find("Player");
    }
}
