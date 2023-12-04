using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //访问游戏时间
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float impetuousMultipie;//浮躁条影响倍数

    private bool gameActive;
    public float timer;


    // Start is called before the first frame update
    void Start()
    {
        gameActive = true;

        impetuousMultipie = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameActive == true)
        {
            //浮躁条影响时间流逝
            timer += (Time.deltaTime * ImpetuousBar.instance.currentImpetuousBar * impetuousMultipie);

            ImpetuousBar.instance.TimeLapse(timer);//时间流逝影响浮躁条
        }
    }

    public void EndLevel()
    {
        gameActive = false;
    }
}
