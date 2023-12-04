using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;
    [NonSerialized]public GameObject playerGameObject; //玩家物体
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerGameObject = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
