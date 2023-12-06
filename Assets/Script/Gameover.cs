using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    //访问
    public static Gameover instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject gameoverScreen;

    public TextMeshProUGUI pointsRemainText;
    public float pointsRemain;

    //public TextMeshProUGUI pointsSpendText;

    //各项强化次数
    private float impetuousButtonSpend = 1;

    // Start is called before the first frame update
    void Start()
    {
        gameoverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        PointsRemain();
    }

    //重新开始
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //退出游戏
    public void Exit()
    {

    }

    //点数结算
    public void PointsRemain()
    {
        pointsRemain = LevelManager.instance.timer;

        pointsRemainText.text = "Points:" + pointsRemain.ToString("00");
    }

    //强化
    public void ImpetuousButton()
    {
        impetuousButtonSpend *= 2;


    }
}
 