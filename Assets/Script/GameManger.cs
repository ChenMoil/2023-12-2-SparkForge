using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    public int enemyKill = 0; //玩家杀敌数
    
    public Dictionary<GameObject, AiParent> GetAi;

    public GameObject PopupPrefab; 
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        GetAi = new Dictionary<GameObject, AiParent>();
    }
    private void Start() 
    {
        
    }
    // Update is called once per frame
    void Update()
    {
        AiParent.attackSpeedMultiplier = math.pow(1 / 1.2f, ImpetuousBar.instance.impetuousLevel);
        AiParent.moveSpeedMultiplier = math.pow(1.2f, ImpetuousBar.instance.impetuousLevel);
    }
}
