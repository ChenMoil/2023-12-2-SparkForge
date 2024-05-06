using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Timers;
using UnityEngine;
using UnityEngine.UI;

public class ImpetuousBar : MonoBehaviour
{
    public Image fixImgae;

    public Text killText;

    //访问玩家的浮躁条
    public static ImpetuousBar instance;

    private void Awake()
    {
        instance = this;
    }

    //当前浮躁条与最大浮躁条
    [Header("当前浮躁条")]
    public float currentImpetuousBar;
    [Header("最高浮躁条")]
    public float maxImpetuousBar;
    
    //时间对于浮躁条影响倍数
    public float impetuousMultipie;
    //浮躁阶段
    public int impetuousLevel;

    //浮躁条UI,通过滑条制作
    public Slider impetuousSlider;

    //阶段变化间隙时长
    public float timer;

    private float fieldTimer;
    private float AllAttackTimer;
    private bool isCalmDown = false;
    void Start()
    {
        maxImpetuousBar = maxImpetuousBar * (1 + Gameover.instance.impetuousLevel * 0.2f);

        //数值初始化
        currentImpetuousBar = 0f;
        impetuousMultipie = 8f;
        impetuousLevel = 0;

        impetuousSlider.maxValue = maxImpetuousBar;
        impetuousSlider.value = currentImpetuousBar;
    }

    // Update is called once per frame
    void Update()
    {
        ChangeFixAndKillText();
        ImpetuousMultipieChange();

        AllAttackTimer += Time.deltaTime;
        fieldTimer += Time.deltaTime;
        timer += Time.deltaTime;
    }

    //浮躁条的阶段性变化
    public void ImpetuousMultipieChange()
    {      
        if (currentImpetuousBar < maxImpetuousBar / 6f && timer > 3f && impetuousLevel != 0)
        {
            SignUI.instance.DisplayText("你感觉心如止水", 2f, SignUI.instance.colors[0]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 4f;
            impetuousLevel = 0;//阶段一
        }
        else if (currentImpetuousBar >= maxImpetuousBar / 6f && currentImpetuousBar < maxImpetuousBar * 1 / 3f && timer > 3f && impetuousLevel != 1)
        {
            SignUI.instance.DisplayText("你感觉平静", 2f, SignUI.instance.colors[1]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 2f;
            impetuousLevel = 1;//阶段二
        }
        else if (currentImpetuousBar >= maxImpetuousBar * 1 / 3f && currentImpetuousBar < maxImpetuousBar * 1 /2f && timer > 3f && impetuousLevel != 2)
        {
            SignUI.instance.DisplayText("你感觉不安", 2f, SignUI.instance.colors[2]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 1f;
            impetuousLevel = 2;//阶段三
        }
        else if (currentImpetuousBar >= maxImpetuousBar * 1 / 2f && currentImpetuousBar < maxImpetuousBar * 2 / 3f && timer > 3f && impetuousLevel != 3)
        {
            SignUI.instance.DisplayText("你感觉烦躁", 2f, SignUI.instance.colors[3]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 0.5f;
            impetuousLevel = 3;//阶段四
        }
        else if (currentImpetuousBar >= maxImpetuousBar * 2 / 3f && currentImpetuousBar < maxImpetuousBar * 5 / 6f && timer > 3f && impetuousLevel != 4)
        {
            SignUI.instance.DisplayText("你感觉愤怒", 2f, SignUI.instance.colors[4]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 0.25f;
            impetuousLevel = 4;//阶段五
        }
        else if (currentImpetuousBar >= maxImpetuousBar * 5 / 6f && currentImpetuousBar < maxImpetuousBar && timer > 3f && impetuousLevel != 5)
        {
            SignUI.instance.DisplayText("你感觉暴怒", 2f, SignUI.instance.colors[5]);
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[7], 1f, 0, 1f);
            timer = 0f;//重置计时

            impetuousMultipie = 0.125f;
            impetuousLevel = 5;//阶段六
        }
        //阶段七游戏结束

    }

    //玩家受击
    public void TakeDamage(float damageToTake)
    {
        //显示伤害数字
        PopupText.Create(PlayerControl.Instance.transform.position, damageToTake < 0 ? (int)-damageToTake : (int)damageToTake, damageToTake < 0 ? 2 : 1);
        //播放受伤音效
        if (damageToTake > 0) 
        {
            AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[4], 0.3f, 0, 1);
        }

        currentImpetuousBar += damageToTake;

        if(currentImpetuousBar>=maxImpetuousBar)
        {
            LevelManager.instance.EndLevel();//游戏结束
        }

        impetuousSlider.value = currentImpetuousBar;//UI

        //让玩家进入受伤状态
        if (damageToTake > 0)
        {
            PlayerControl.Instance.EnterBeHurtState(0.3f);
        }

        //判断是否发动强制冷静
        CalmDown();
    }


    //时间流逝
    public void TimeLapse(float timeToLapse)
    {
        currentImpetuousBar += timeToLapse;

        if (currentImpetuousBar >= maxImpetuousBar)
        {
            LevelManager.instance.EndLevel();//游戏结束
        }

       impetuousSlider.value = currentImpetuousBar;//UI
    }

    //消灭敌人
    public void DestroyEnemy(float enemyToBeDestroy)
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
        PopupText.Create(PlayerControl.Instance.transform.position, (int)meditation, 2);
        if(currentImpetuousBar >= 0)
            currentImpetuousBar -= meditation;

        impetuousSlider.value = currentImpetuousBar;//UI
        FieldSkill();
        AllAttack();
    }
    public void KillEnemy(float meditation)
    {
        PopupText.Create(PlayerControl.Instance.transform.position, (int)meditation, 2);
        if (currentImpetuousBar >= 0)
            currentImpetuousBar -= meditation;

        impetuousSlider.value = currentImpetuousBar;//UI
        AllAttack();
    }

    public void ChangeFixAndKillText()
    {
        float currentLevel = impetuousLevel;
        float level = 5f;
        fixImgae.color = new Color(impetuousLevel / level, 1 - impetuousLevel / level, 0, 1);
        killText.text = GameManger.Instance.enemyKill.ToString();
    }

    private void FieldSkill()
    {
        int skill = PlayerPrefs.GetInt("shieldLevel");
        if (fieldTimer >= 3 - 0.2f * skill)
        {
            SignUI.instance.DisplayText("额外回血技能触发", 1f, Color.green);
            fieldTimer = 0;
            PopupText.Create(PlayerControl.Instance.transform.position, 20 * skill, 2);
            currentImpetuousBar -= 20 * skill;
        }
    }
    /// <summary>
    /// 全屏伤害技能
    /// </summary>
    private void AllAttack()
    {
        if (currentImpetuousBar <= 0 && AllAttackTimer >= (6 - PlayerPrefs.GetInt("fullScreenDamageLevel") / 2))
        {
            AllAttackTimer = 0;
            SignUI.instance.DisplayText("你触发了全屏伤害技能", 1f, Color.red);
            foreach (AiParent enemy in GameManger.Instance.GetAi.Values)
            {
                if (enemy != null && enemy.enabled == true)
                {
                    enemy.HP -= PlayerPrefs.GetInt("fullScreenDamageLevel") * 50;
                }
            }
        }
    }

    /// <summary>
    /// 强制冷静技能
    /// </summary>
    private void CalmDown()
    {
        if(isCalmDown == false && PlayerPrefs.GetInt("calmdownSkillLevel") == 1 && currentImpetuousBar >= maxImpetuousBar / 2)
        {
            SignUI.instance.DisplayText("你触发了强制冷静效果", 1f, Color.green);
            isCalmDown = true;
            currentImpetuousBar = 0;
        }
    }
}
