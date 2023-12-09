using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class DeathScene : MonoBehaviour
{
    public Text obtainPointsText;
    public Text survivalTime;
    // Start is called before the first frame update

    public void Start()
    {
        obtainPointsText.text = DataStorage.instance.obtainPoints.ToString();
        survivalTime.text = DataStorage.instance.survivalTime.ToString();
    }
    public void Exit()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        SceneManager.LoadScene(0);
    }
    public void Restart()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        SceneManager.LoadScene(1);
    }
}
