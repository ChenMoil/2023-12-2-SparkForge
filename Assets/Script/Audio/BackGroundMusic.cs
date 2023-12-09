using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundMusic : MonoBehaviour
{
    //背景音乐对应下标
    public int backGroundMusic;
    //声音大小
    public float volume;
    void Start()
    {
        //进入新场景 关闭所有音乐
        AudioManager.instance.StopAll();
        AudioManager.instance.PlayLoop(AudioManager.instance.AudioClip[backGroundMusic], volume, 0, 1);
    }
}
