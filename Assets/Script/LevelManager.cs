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

    public TextMeshProUGUI timeText;//时间UI


    // Start is called before the first frame update
    void Start()
    {
        gameActive = true;

        timerMultipie = 5f;

        timer = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameActive == true)
        {
            //浮躁条影响时间流逝
            timer += (Time.deltaTime * ImpetuousBar.instance.impetuousMultipie);
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

        timeText.text = "Time:" + minutes + ":" + seconds.ToString("00");
    }

    //Gameover
    public void EndLevel()
    {
        //计时暂停
        gameActive = false;
        //浮躁条停用
        ImpetuousBar.instance.gameObject.SetActive(false);
        
        EnemySpawn.instance.gameObject.SetActive(false);
        ObjectPool.Instance.ClearAll();

        //调用点数面板
        Gameover.instance.gameoverScreen.SetActive(true);
        Gameover.instance.PointsRemain();
    }
}
