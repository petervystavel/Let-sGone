using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject Player;
    public static GameManager Instance;

    public GameObject Enemy1;
    public GameObject Enemy2;
    public GameObject Enemy3;

    public bool FreezeAllEnemy = false;
    public Material White;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        Instance = this;
    }
}
