using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;

    public int enemyKill = 0; //玩家杀敌数

    public List<GameObject> TestEnemyGameObjects; //测试用
    public List<GameObject> TestGameObjects; //测试用
    public List<GameObject> TestGameObjects1; //测试用
    
    public Dictionary<GameObject, AiParent> GetAi;
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
        
    }
}
