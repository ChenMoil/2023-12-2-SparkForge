using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

//可被序列化
[System.Serializable]
public class Save
{

    //需要存储的数据

    //浮躁条的值
    public float currentImpetuousBarValue;
    //当前关卡已进行时间
    public float currentTime;
    //已获取分数
    public float currentPoints;
    //当前boss血量
    public float currentBossBloodBar;
    //杀敌数
    public int enemyKill;
    //游戏时间
    public float gameTime;
    //已获得增益效果

}

