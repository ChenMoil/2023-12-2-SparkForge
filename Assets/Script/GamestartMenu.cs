using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GamestartMenu : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void GameStartButton()
    {
        Debug.Log("1");
        //转到下一个场景
        //SceneManager.loadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 

    public void ExitButton()
    {
        Debug.Log("2");
        //退出游戏
        Application.Quit();
    }
}
