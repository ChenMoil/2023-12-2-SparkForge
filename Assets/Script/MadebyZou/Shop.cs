using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System;

public class Shop : MonoBehaviour
{

    public GameObject shopCanvasObject;
    
    //����ǿ���ȼ�
    public Text decreaseImpetuousLevel;
    public Text shieldLevel;//���ܵȼ�
    public Text moveSpeedLevel;
    public Text fullScreenDamageLevel;
    //����Ч��
    public Text calmdownSkillLevel;
    
    //ǿ���������
    public float decreaseImpetuousCost;
    public float shieldCost;
    public float moveSpeedCost;
    public float fullScreenDamageCost;
    public float calmdownCost;
    
    //��ǰʣ�����
    public Text PointsRemainText;
    public float PointsRemain;

    //��������
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


        //����ǿ�����ѵ�����ʼ����
        decreaseImpetuousCost = 150f * (PlayerPrefs.GetInt("decreaseImpetuousLevel") + 1);
        shieldCost = 100f * (PlayerPrefs.GetInt("shieldLevel") + 1);
        moveSpeedCost = 125f * (PlayerPrefs.GetInt("moveSpeedLevel") + 1);
        fullScreenDamageCost = 200 * (PlayerPrefs.GetInt("fullScreenDamageLevel") + 1);
        calmdownCost = 2500f;

        //��ʾ�ȼ�
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

        //��ǰ����
        PointsRemain = DataStorage.instance.obtainPoints;
    }

    // Update is called once per frame
    void Update()
    {
        PointsRemainText.text = "PointsRemain:" + PointsRemain.ToString();
    }

    //���¿�ʼ��Ϸ
    public void Restart()
    {
        PlayerPrefs.SetInt("RestartButtonIsPush", 1);
        PlayerPrefs.SetInt("TransitionButtonIsPush", 1);
        //SceneManager.LoadScene(1);
    }

    //���ز˵�
    public void BackButton()
    {
        PlayerPrefs.SetInt("TransitionButtonIsPush", 1);
        //SceneManager.LoadScene(0);
    }

    //���ܵ��˽�����������ֵ����
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

    //ڤ���û�������
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

    //�����ƶ��ٶ�����
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

    //ȫ���˺�����
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

    //ǿ���侲����
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
