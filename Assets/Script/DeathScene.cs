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

    void Start()
    {
        obtainPointsText.text = DataStorage.instance.obtainPoints.ToString();
        survivalTime.text = DataStorage.instance.survivalTime.ToString();
    }

    public void Exit()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        //SceneManager.LoadScene(0);
        PlayerPrefs.SetInt("TransitionButtonIsPush", 1);
    }
    public void Restart()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        //SceneManager.LoadScene(1);
        PlayerPrefs.SetInt("FirstNextButtonIsPush", 1);

        StartCoroutine(LoadLevel());

    }
    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);

        //调出排行榜面板
        RankingsCanvas.instance.rankingsCanvasObject.SetActive(true);
    }
}
