using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    [Header("攻击速度倍率，怪物攻击间隔会除与该数值(每个浮躁阶段都会除一次)")]
    public float attackSpeedMultiplier;
    [Header("怪物移动倍率，怪物速度会乘该数值(每个浮躁阶段都会乘一次)")]
    public float moveSpeedMultiplier;
    [Header("敌人生成速度倍率（大于1生成速度加快）")]
    public float enemySpawnSpeed;

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
        if(Gameover.instance != null)
            Gameover.instance.gameoverScreen.SetActive(false);

        PlayerPrefs.SetInt("decreaseImpetuousLevel", 5);
        PlayerPrefs.SetInt("shieldLevel", 5);
        PlayerPrefs.SetInt("moveSpeedLevel", 5);
        PlayerPrefs.SetInt("fullScreenDamageLevel", 5);
        PlayerPrefs.SetInt("calmdownSkillLevel", 1);
    }
    // Update is called once per frame
    void Update()
    {
        AiParent.attackSpeedMultiplier = math.pow(1 / attackSpeedMultiplier, ImpetuousBar.instance.impetuousLevel);
        AiParent.moveSpeedMultiplier = math.pow(moveSpeedMultiplier, ImpetuousBar.instance.impetuousLevel);
        EnemySpawn.instance.enemySpawnSpeed = math.pow(enemySpawnSpeed, ImpetuousBar.instance.impetuousLevel);
    }
}
