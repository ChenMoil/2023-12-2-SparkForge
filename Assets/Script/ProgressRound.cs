using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressRound : MonoBehaviour
{
    //计时器
    public float timer;
    //敌人生成阶段
    public int enemyProgress;
    //敌人生成阶段显示指针
    public Transform progressPointer;
    //阶段倒计时指针
    public Transform countdownPointer;
    //阶段倒计时受情绪条影响倍率
    public float timepassMultiple = 1f;
    //总时长
    public float totalTime=180f;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
        Countdown(timer);
        Progress(timer);
    }

    //阶段倒计时指针函数
    public void Countdown(float timer)
    {
        //指针旋转
        float timerIn = timer % 180;
        float pointerAngle = 180 * (timerIn * timepassMultiple) / totalTime;
        countdownPointer.localRotation = Quaternion.Euler(0, 0, pointerAngle);
      
    }

    //敌人生成阶段显示指针函数
    public void Progress(float timer)
    {
        //敌人生成阶段
        if (timer <= totalTime*1/3)
        {
            enemyProgress = 1;
        }
        else if (timer > totalTime*1/3 && timer <= totalTime*2/3)
        {
            enemyProgress = 2;
        }
        else if (timer > totalTime*2/3)
        {
            enemyProgress = 3;
        }

        //指针旋转
        float pointerAngle = 180 * (timer * timepassMultiple) / totalTime;
        progressPointer.localRotation = Quaternion.Euler(0, 0, pointerAngle/3);
    }
}
