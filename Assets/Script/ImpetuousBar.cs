using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;
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

    public float impetuousMultipie;//浮躁条影响倍数
    public int impetuousLevel;//浮躁条等级

    public Slider impetuousSlider;//浮躁条UI,通过滑条制作

    // Start is called before the first frame update
    void Start()
    {
        //数值初始化
        currentImpetuousBar = 0f;
        impetuousMultipie = 8f;
        impetuousLevel = 1;

        impetuousSlider.maxValue = maxImpetuousBar;
        impetuousSlider.value = currentImpetuousBar;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //浮躁条的阶段性变化
    public void ImpetuousMultipieChange()
    {
        StartCoroutine(ImpetuousMultipie_Change());
    }
    IEnumerator ImpetuousMultipie_Change()
    {
        if (currentImpetuousBar < maxImpetuousBar / 8f)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 4f;
            impetuousLevel = 1;

        }
        else if (currentImpetuousBar >= maxImpetuousBar / 8f && currentImpetuousBar < maxImpetuousBar * 5 / 16f)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 2f;
            impetuousLevel = 2;

        }
        else if (currentImpetuousBar >= maxImpetuousBar * 5 / 16f && currentImpetuousBar < maxImpetuousBar * 3 / 8f)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 1f;
            impetuousLevel = 3;

        }
        else if (currentImpetuousBar >= maxImpetuousBar * 3 / 8f && currentImpetuousBar < maxImpetuousBar * 13 / 16f)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 0.5f;
            impetuousLevel = 4;

        }
        else if (currentImpetuousBar >= maxImpetuousBar * 13 / 16f && currentImpetuousBar < maxImpetuousBar * 15 / 16f)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 0.25f;
            impetuousLevel = 5;

        }
        else if (currentImpetuousBar >= maxImpetuousBar * 15 / 16f && currentImpetuousBar < maxImpetuousBar)
        {

            yield return new WaitForSeconds(3f);

            impetuousMultipie = 0.125f;
            impetuousLevel = 6;

        }
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
        currentImpetuousBar += timeToLapse;

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

    /// <summary>
    /// 冥想
    /// </summary>
    /// <param name="meditation">浮躁条减少的大小</param>
    public void Meditation(float meditation)
    {
        currentImpetuousBar -= meditation;

        impetuousSlider.value = currentImpetuousBar;//UI
    }
}
