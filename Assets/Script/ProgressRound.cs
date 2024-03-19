using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressRound : MonoBehaviour
{
    //��ʱ��
    public float timer;
    //�������ɽ׶�
    public int enemyProgress;
    //�������ɽ׶���ʾָ��
    public Transform progressPointer;
    //�׶ε���ʱָ��
    public Transform countdownPointer;
    //�׶ε���ʱ��������Ӱ�챶��
    public float timepassMultiple = 1f;
    //��ʱ��
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

    //�׶ε���ʱָ�뺯��
    public void Countdown(float timer)
    {
        //ָ����ת
        float timerIn = timer % 180;
        float pointerAngle = 180 * (timerIn * timepassMultiple) / totalTime;
        countdownPointer.localRotation = Quaternion.Euler(0, 0, pointerAngle);
      
    }

    //�������ɽ׶���ʾָ�뺯��
    public void Progress(float timer)
    {
        //�������ɽ׶�
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

        //ָ����ת
        float pointerAngle = 180 * (timer * timepassMultiple) / totalTime;
        progressPointer.localRotation = Quaternion.Euler(0, 0, pointerAngle/3);
    }
}
