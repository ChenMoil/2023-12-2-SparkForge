
using UnityEngine;


public class GamePauseManager : MonoBehaviour
{

    public GameObject gamePauseCanvas;
    //游戏进行状态
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

    //保存游戏
    public void GameSaveButton()
    {

    }

}
