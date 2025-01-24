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

    public Material Projectile1;
    public Material Projectile2;
    public Material Projectile3;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        Instance = this;
    }
}
