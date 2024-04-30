using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Transition : MonoBehaviour
{
    public Animator transition;

    public float transitionTime = 1f;

    // Update is called once per frame
    void Update()
    {

        //场景间转场
        if (PlayerPrefs.GetInt("TransitionButtonIsPush")==1)
        {
            PlayerPrefs.SetInt("TransitionButtonIsPush", 0);
            LoadNextLevel();
        }
    }

    /// <summary>
    /// 场景间转场方法
    /// </summary>
    public void LoadNextLevel()
    {
        if(SceneManager.GetActiveScene().buildIndex<=1)
            StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
        else if (PlayerPrefs.GetInt("RestartButtonIsPush") == 1)
        {
            PlayerPrefs.SetInt("RestartButtonIsPush", 0);
            StartCoroutine(LoadLevel(1));
        }
        else
            StartCoroutine(LoadLevel(0));
    }
    IEnumerator LoadLevel(int LevelIndex)
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadScene(LevelIndex);
    }
}
