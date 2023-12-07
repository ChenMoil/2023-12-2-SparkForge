using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelManager : MonoBehaviour
{

    //������Ϸʱ��
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float timerMultipie;//ʱ������Ӱ�챶��

    public bool gameActive;
    public float timer;

    public TextMeshProUGUI timeText;//ʱ��UI


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
            //������Ӱ��ʱ������
            timer += (Time.deltaTime * ImpetuousBar.instance.impetuousMultipie);
            UpdateTimer(timer);


            //ʱ������Ӱ�측����
            ImpetuousBar.instance.TimeLapse(Time.deltaTime * timerMultipie);
        }
    }

    //ʱ����ʾ
    public void UpdateTimer(float time)
    {
        float minutes = Mathf.FloorToInt(time / 60f);
        float seconds = Mathf.FloorToInt(time % 60);

        timeText.text = "Time:" + minutes + ":" + seconds.ToString("00");
    }

    //Gameover
    public void EndLevel()
    {
        //���������Ч��
        gameActive = false;
        ImpetuousBar.instance.gameObject.SetActive(false);

        //���õ������
        Gameover.instance.gameoverScreen.SetActive(true);
        Gameover.instance.PointsRemain();
    }
}
