using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{
    //访问游戏时间
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float timerMultipie;//时间流逝影响倍数

    public bool gameActive;

    public float timer; //游戏时间
    public float realTimer; //现实时间
    public float secondTimer60; //60s计数器

    public TextMeshProUGUI timeText;//时间UI

    public int curLevel = 0;
    public event Action<int> levelChange; //刷怪阶段切换时会发生的时间

    // Start is called before the first frame update
    void Start()
    {
        gameActive = true;

        timerMultipie = 5f;

        timer = 0f;


        levelChange += EnemySpawn.instance.RefreshWaveStateAndSpawnBoss;
    }
    private void FixedUpdate()
    {
        secondTimer60 += Time.deltaTime;
        if (secondTimer60 >= 20)
        {
            secondTimer60 = 0;
            curLevel += 1;
            curLevel %= 5;
            levelChange(curLevel);
        }
    }
    void Update()
    {
        if(gameActive == true)
        {
            //浮躁条影响时间流逝
            //timer += (Time.deltaTime * ImpetuousBar.instance.impetuousMultipie);
            realTimer += Time.deltaTime;
            UpdateTimer(timer);


            //时间流逝影响浮躁条
            ImpetuousBar.instance.TimeLapse(Time.deltaTime * timerMultipie);
        }
    }

    //时间显示
    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60);

        //timeText.text = "Time:" + minutes + ":" + seconds.ToString("00");
    }

    //Gameover
    public void EndLevel()
    {
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[8], 0.7f, 0, 1);

        //计时暂停
        gameActive = false;
        //浮躁条停用
        ImpetuousBar.instance.gameObject.SetActive(false);
        //刷怪停止
        EnemySpawn.instance.gameObject.SetActive(false);
        ObjectPool.Instance.ClearAll();

        //调用点数面板
        Gameover.instance.gameoverScreen.SetActive(true);
        //Gameover.instance.PointsRemain();
    }
}
