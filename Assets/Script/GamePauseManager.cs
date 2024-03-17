using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Threading;
using System.Diagnostics;

public class GamePauseManager : MonoBehaviour
{

    public GameObject gamePauseCanvas;
    public static bool gameisPause = false;

    // Start is called before the first frame update
    void Start()
    {
        gamePauseCanvas.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            if (gameisPause)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }

    //回到游戏
    public void Resume()
    {
        gamePauseCanvas.SetActive(false);
        Time.timeScale = 1.0f;
        gameisPause = false;
    }

    //暂停游戏
    private void Pause()
    {
        gamePauseCanvas.SetActive(true);
        Time.timeScale = 0.0f;
        gameisPause = true;
    }


    /// <summary>
    /// 游戏存档函数
    /// </summary>
    public void GameSaveButton()
    {

    }


    //退出游戏
    public void GameExitButton()
    {
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);

        //退出游戏
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
            Application.Quit();
        #endif
    }
}
