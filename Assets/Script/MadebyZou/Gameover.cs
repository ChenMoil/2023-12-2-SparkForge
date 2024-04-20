using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Diagnostics;
using System.Threading;

public class Gameover : MonoBehaviour
{
    //访问
    public static Gameover instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject gameoverScreen;

    public TextMeshProUGUI pointsRemainText;
    //各项强化等级文本
    public TextMeshProUGUI impetuousLevelText;
    public TextMeshProUGUI attackspeedLevelText;
    public TextMeshProUGUI movespeedLevelText;
    public TextMeshProUGUI meditationLevelText;

    public float pointsRemain;

    //public TextMeshProUGUI pointsSpendText;

    //各项强化花费
    private float impetuousButtonSpend = 100f;
    private float attackspeedButtonSpend = 100f;
    private float movespeedButtonSpend = 100f;
    private float meditationButtonSpend = 100f;
    //各项强化等级
    public int impetuousLevel = 1;
    public int attackspeedLevel = 1;
    public int movespeedLevel = 1;
    public int meditationLevel = 1;

    // Start is called before the first frame update
    private void OnEnable()
    {
        
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
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
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);

        //重置浮躁条
        ImpetuousBar.instance.gameObject.SetActive(true);
        ImpetuousBar.instance.currentImpetuousBar = 0f;

        //重置计时器
        LevelManager.instance.gameActive = true;
        LevelManager.instance.timer = 0f;

        //关闭点数面板
        gameoverScreen.SetActive(false);

        //刷怪开始
        //GetComponent<EnemySpawn>().enabled = true;
        //GetComponent<EnemySpawn>().enableTime = Time.time;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void Exit(string sceneName)
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);

        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //点数结算
    //角色死亡场景跳转
    public void PointsRemain()
    {
        pointsRemain = LevelManager.instance.timer + GameManger.Instance.enemyKill;

        //跳转场景
        SceneManager.LoadScene(2);

        //pointsRemainText.text = ":" + pointsRemain.ToString("00");
        DataStorage.instance.obtainPoints = pointsRemain;
    }

    //点数刷新
    public void PointsNew()
    {
        pointsRemainText.text = ":" + pointsRemain.ToString("00");
    }

    //强化浮躁条上限
    public void ImpetuousButton()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        if (pointsRemain >= impetuousButtonSpend && impetuousButtonSpend<=1600f)
        {
            pointsRemain -= impetuousButtonSpend;

            ImpetuousBar.instance.maxImpetuousBar *= 1.2f;

            impetuousButtonSpend *= 2;
            impetuousLevel += 1;

            //更新文本
            PointsNew();
            impetuousLevelText.text = "LEVEL：" + impetuousLevel.ToString("0");
        }
    }

    //强化攻击力
    public void AttackspeedButton()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        if (pointsRemain >= attackspeedButtonSpend && attackspeedButtonSpend<=1600f)
        {
            pointsRemain -= attackspeedButtonSpend;

            for(int i=0 ; i<6 ; i++)
            {
                PlayerControl.Instance.attackSpeed[i] *= 1.4f;
            }

            attackspeedButtonSpend *= 2;
            attackspeedLevel += 1;

            //更新文本
            PointsNew();
            attackspeedLevelText.text = "LEVEL：" + attackspeedLevel.ToString("0");
        }
    }

    //强化移动速度
    public void MovespeedButton()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        if (pointsRemain >= movespeedButtonSpend && movespeedButtonSpend<=1600f)
        {
            pointsRemain -= movespeedButtonSpend;

            PlayerControl.Instance.playerSpeed *= 1.1f;

            movespeedButtonSpend *= 2;
            movespeedLevel += 1;
            
            //更新文本
            PointsNew();
            movespeedLevelText.text = "LEVEL：" + movespeedLevel.ToString("0");
        }
    }

    //强化冥想收益
    public void MeditationButton()
    {
        //播放点击按钮音效
        AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 1f, 0, 1);
        if (pointsRemain >= meditationButtonSpend && meditationButtonSpend<=1600f)
        {
            pointsRemain -= meditationButtonSpend;

            PlayerControl.Instance.meditationSpeed *= 1.25f;

            meditationButtonSpend *= 2;
            meditationLevel += 1;

            //更新文本
            PointsNew();
            meditationLevelText.text = "LEVEL：" + meditationLevel.ToString("0");
        }
    }
}
 