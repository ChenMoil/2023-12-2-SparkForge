using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;

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

    //各项强化花费
    private float impetuousButtonSpend = 100f;
    private float attackspeedButtonSpend = 100f;
    private float movespeedButtonSpend = 100f;
    private float meditationButtonSpend = 100f;

    // Start is called before the first frame update
    void Start()
    {
        gameoverScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        //PointsRemain();
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
        pointsRemain = LevelManager.instance.timer + GameManger.Instance.enemyKill;

        pointsRemainText.text = "Points:" + pointsRemain.ToString("00");
    }

    //点数刷新
    public void PointsNew()
    {
        pointsRemainText.text = "Points:" + pointsRemain.ToString("00");
    }

    //强化浮躁条上限
    public void ImpetuousButton()
    {
        if (pointsRemain >= impetuousButtonSpend && impetuousButtonSpend<=1600f)
        {
            pointsRemain -= impetuousButtonSpend;

            ImpetuousBar.instance.maxImpetuousBar *= 1.2f;

            impetuousButtonSpend *= 2;

            PointsNew();//更新文本
        }
    }

    //强化攻击力
    public void AttackspeedButton()
    {
        if (pointsRemain >= attackspeedButtonSpend && attackspeedButtonSpend<=1600f)
        {
            pointsRemain -= attackspeedButtonSpend;

            for(int i=0 ; i<6 ; i++)
            {
                PlayerControl.Instance.attackSpeed[i] *= 1.4f;
            }

            attackspeedButtonSpend *= 2;

            PointsNew();//更新文本
        }
    }

    //强化移动速度
    public void MovespeedButton()
    {
        if (pointsRemain >= movespeedButtonSpend && movespeedButtonSpend<=1600f)
        {
            pointsRemain -= movespeedButtonSpend;

            PlayerControl.Instance.playerSpeed *= 1.1f;

            movespeedButtonSpend *= 2;

            PointsNew();//更新文本
        }
    }

    //强化冥想收益
    public void MeditationButton()
    {
        if (pointsRemain >= meditationButtonSpend && meditationButtonSpend<=1600f)
        {
            pointsRemain -= meditationButtonSpend;

            PlayerControl.Instance.meditationSpeed *= 1.25f;

           meditationButtonSpend *= 2;

            PointsNew();//更新文本
        }
    }
}
 