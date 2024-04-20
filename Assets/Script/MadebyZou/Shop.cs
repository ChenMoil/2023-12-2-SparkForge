using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{

    public GameObject shopCanvasObject;
    
    //各项强化等级
    public Text decreaseImpetuousLevel;
    public Text shieldLevel;//护盾等级
    public Text moveSpeedLevel;
    public Text fullScreenDamageLevel;
    //特殊效果
    public Text calmdownSkillLevel;
    
    //强化所需点数
    public float decreaseImpetuousCost;
    public float shieldCost;
    public float moveSpeedCost;
    public float fullScreenDamageCost;
    public float calmdownCost;
    
    //当前剩余点数
    public Text PointsRemainText;
    public float PointsRemain;

    //单例访问
    public static Shop instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        shopCanvasObject.SetActive(false);


        //各项强化花费点数初始花费
        decreaseImpetuousCost = 150f * (PlayerPrefs.GetInt("decreaseImpetuousLevel") + 1);
        shieldCost = 100f * (PlayerPrefs.GetInt("shieldLevel") + 1);
        moveSpeedCost = 125f * (PlayerPrefs.GetInt("moveSpeedLevel") + 1);
        fullScreenDamageCost = 200 * (PlayerPrefs.GetInt("fullScreenDamageLevel") + 1);
        calmdownCost = 2500f;

        //显示等级
        decreaseImpetuousLevel.text = PlayerPrefs.GetInt("decreaseImpetuousLevel").ToString();
        shieldLevel.text = PlayerPrefs.GetInt("shieldLevel").ToString();
        moveSpeedLevel.text = PlayerPrefs.GetInt("moveSpeedLevel").ToString();
        fullScreenDamageLevel.text = PlayerPrefs.GetInt("fullScreenDamageLevel").ToString();
        if (PlayerPrefs.GetInt("calmdownSkillLevel") == 1)
        {
            calmdownSkillLevel.text = "UNLOCKED";
        }
        else
        {
            calmdownSkillLevel.text = "LOCKED";
        }

        //当前分数
        PointsRemain = DataStorage.instance.obtainPoints;
    }

    // Update is called once per frame
    void Update()
    {
        PointsRemainText.text = "PointsRemain:" + PointsRemain.ToString();
    }

    //重新开始游戏
    public void Restart()
    {
        PlayerPrefs.SetInt("RestartButtonIsPush", 1);
        PlayerPrefs.SetInt("TransitionButtonIsPush", 1);
        //SceneManager.LoadScene(1);
    }

    //返回菜单
    public void BackButton()
    {
        PlayerPrefs.SetInt("TransitionButtonIsPush", 1);
        //SceneManager.LoadScene(0);
    }

    //击败敌人降低少量情绪值能力
    public void DecreaseImpetuousButton()
    {
        int currentLevel = PlayerPrefs.GetInt("decreaseImpetuousLevel");
        if (currentLevel < 5&&PointsRemain>=decreaseImpetuousCost)
        {
            PointsRemain -= decreaseImpetuousCost;
            currentLevel++;
            PlayerPrefs.SetInt("decreaseImpetuousLevel", currentLevel);
        }
        decreaseImpetuousLevel.text = currentLevel.ToString();
    }

    //冥想获得护盾能力
    public void ShieldButton()
    {
        int currentLevel = PlayerPrefs.GetInt("shieldLevel");
        if (currentLevel < 5&&PointsRemain>=shieldCost)
        {
            PointsRemain -= shieldCost;
            currentLevel++;
            PlayerPrefs.SetInt("shieldLevel", currentLevel);
        }
        shieldLevel.text = currentLevel.ToString();
    }

    //增加移动速度能力
    public void MoveSpeedButton()
    {
        int currentLevel = PlayerPrefs.GetInt("moveSpeedLevel");
        if (currentLevel < 5&&PointsRemain>=moveSpeedCost)
        {
            PointsRemain -= moveSpeedCost;
            currentLevel++;
            PlayerPrefs.SetInt("moveSpeedLevel", currentLevel);
        }
        moveSpeedLevel.text = currentLevel.ToString();
    }

    //全屏伤害能力
    public void FullScreenDamageButton()
    {
        int currentLevel = PlayerPrefs.GetInt("fullScreenDamageLevel");
        if (currentLevel < 5&&PointsRemain>=fullScreenDamageCost)
        {
            PointsRemain -= fullScreenDamageCost;
            currentLevel++;
            PlayerPrefs.SetInt("fullScreenDamageLevel", currentLevel);
        }
        fullScreenDamageLevel.text = currentLevel.ToString();
    }

    //强制冷静能力
    public void CalmDownButton()
    {
        int currentLevel = PlayerPrefs.GetInt("calmdownSkillLevel");

        if (currentLevel != 1&&PointsRemain>=calmdownCost)
        {
            PointsRemain -= calmdownCost;
            currentLevel = 1;
            PlayerPrefs.SetInt("calmdownSkillLevel", currentLevel);
        }

        if (PlayerPrefs.GetInt("calmdownSkillLevel") == 1)
        {
            calmdownSkillLevel.text = "UNLOCKED";
        }
        else
        {
            calmdownSkillLevel.text = "LOCKED";
        }
    }
}
