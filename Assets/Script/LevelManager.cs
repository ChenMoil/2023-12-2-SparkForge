using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    //������Ϸʱ��
    public static LevelManager instance;

    private void Awake()
    {
        instance = this;
    }

    public float impetuousMultipie;//������Ӱ�챶��

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
            //������Ӱ��ʱ������
            timer += (Time.deltaTime * ImpetuousBar.instance.currentImpetuousBar * impetuousMultipie);

            ImpetuousBar.instance.TimeLapse(timer);//ʱ������Ӱ�측����
        }
    }

    public void EndLevel()
    {
        gameActive = false;
    }
}
