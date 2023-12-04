using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ImpetuousBar : MonoBehaviour
{

    //访问玩家的浮躁条
    public static ImpetuousBar instance;

    private void Awake()
    {
        instance = this;
    }


    public float currentImpetuousBar, maxImpetuousBar;//当前浮躁条与最大浮躁条

    public float timeMultipie;//时间流逝影响倍数

    public Slider impetuousSlider;//浮躁条UI,通过滑条制作

    // Start is called before the first frame update
    void Start()
    {
        timeMultipie = 1f;

        currentImpetuousBar = 0f;

        impetuousSlider.maxValue = maxImpetuousBar;
        impetuousSlider.value = currentImpetuousBar;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //玩家受击
    public void TakeDamage(float damageToTake)
    {
        currentImpetuousBar += damageToTake;

        if(currentImpetuousBar>=maxImpetuousBar)
        {
            gameObject.SetActive(false);

            LevelManager.instance.EndLevel();//游戏结束
        }

        impetuousSlider.value = currentImpetuousBar;//UI
    }

    //时间流逝
    public void TimeLapse(float timeToLapse)
    {
        currentImpetuousBar = currentImpetuousBar * timeToLapse * timeMultipie;

        if (currentImpetuousBar >= maxImpetuousBar)
        {
            gameObject.SetActive(false);

            LevelManager.instance.EndLevel();//游戏结束
        }

       impetuousSlider.value = currentImpetuousBar;//UI
    }

    //消灭敌人
    public void  DestroyEnemy(float enemyToBeDestroy)
    {
        currentImpetuousBar -= enemyToBeDestroy;

        impetuousSlider.value = currentImpetuousBar;//UI
    }

    //冥想
    public void Meditation(float meditation)
    {
        currentImpetuousBar -= meditation;

        impetuousSlider.value = currentImpetuousBar;//UI
    }
}
