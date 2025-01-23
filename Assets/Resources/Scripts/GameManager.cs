using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameObject Player;
    public static GameManager Instance;
    public Material White;

    private void Awake()
    {
        Player = GameObject.Find("Player");
        Instance = this;
    }
}
