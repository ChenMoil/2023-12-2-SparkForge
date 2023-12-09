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
    //����
    public static Gameover instance;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public GameObject gameoverScreen;

    public TextMeshProUGUI pointsRemainText;
    //����ǿ���ȼ��ı�
    public TextMeshProUGUI impetuousLevelText;
    public TextMeshProUGUI attackspeedLevelText;
    public TextMeshProUGUI movespeedLevelText;
    public TextMeshProUGUI meditationLevelText;

    public float pointsRemain;

    //public TextMeshProUGUI pointsSpendText;

    //����ǿ������
    private float impetuousButtonSpend = 100f;
    private float attackspeedButtonSpend = 100f;
    private float movespeedButtonSpend = 100f;
    private float meditationButtonSpend = 100f;
    //����ǿ���ȼ�
    public int impetuousLevel = 1;
    public int attackspeedLevel = 1;
    public int movespeedLevel = 1;
    public int meditationLevel = 1;

    // Start is called before the first frame update
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

    //���¿�ʼ
    public void Replay()
    {
        //���ø�����
        ImpetuousBar.instance.gameObject.SetActive(true);
        ImpetuousBar.instance.currentImpetuousBar = 0f;

        //���ü�ʱ��
        LevelManager.instance.gameActive = true;
        LevelManager.instance.timer = 0f;

        //�رյ������
        gameoverScreen.SetActive(false);

        //ˢ�ֿ�ʼ
        //GetComponent<EnemySpawn>().enabled = true;
        //GetComponent<EnemySpawn>().enableTime = Time.time;

        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //退出游戏
    public void Exit(string sceneName)
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(sceneName);
    }

    //点数结算
    public void PointsRemain()
    {
        pointsRemain = LevelManager.instance.timer*100 + GameManger.Instance.enemyKill;

        pointsRemainText.text = ":" + pointsRemain.ToString("00");
    }

    //点数刷新
    public void PointsNew()
    {
        pointsRemainText.text = ":" + pointsRemain.ToString("00");
    }

    //ǿ������������
    public void ImpetuousButton()
    {

        UnityEngine.Debug.Log("1111");
        if (pointsRemain >= impetuousButtonSpend && impetuousButtonSpend<=1600f)
        {
            pointsRemain -= impetuousButtonSpend;

            ImpetuousBar.instance.maxImpetuousBar *= 1.2f;

            impetuousButtonSpend *= 2;
            impetuousLevel += 1;

            //�����ı�
            PointsNew();
            impetuousLevelText.text = "LEVEL:" + impetuousLevel.ToString("0");
        }
    }

    //ǿ��������
    public void AttackspeedButton()
    {
        UnityEngine.Debug.Log("1111");
        if (pointsRemain >= attackspeedButtonSpend && attackspeedButtonSpend<=1600f)
        {
            pointsRemain -= attackspeedButtonSpend;

            for(int i=0 ; i<6 ; i++)
            {
                PlayerControl.Instance.attackSpeed[i] *= 1.4f;
            }

            attackspeedButtonSpend *= 2;
            attackspeedLevel += 1;

            //�����ı�
            PointsNew();
            attackspeedLevelText.text = "LEVEL:" + attackspeedLevel.ToString("0");
        }
    }

    //ǿ���ƶ��ٶ�
    public void MovespeedButton()
    {
        UnityEngine.Debug.Log("1111");
        if (pointsRemain >= movespeedButtonSpend && movespeedButtonSpend<=1600f)
        {
            pointsRemain -= movespeedButtonSpend;

            PlayerControl.Instance.playerSpeed *= 1.1f;

            movespeedButtonSpend *= 2;
            movespeedLevel += 1;
            
            //�����ı�
            PointsNew();
            movespeedLevelText.text = "LEVEL:" + movespeedLevel.ToString("0");
        }
    }

    //ǿ��ڤ������
    public void MeditationButton()
    {
        UnityEngine.Debug.Log("1111");
        if (pointsRemain >= meditationButtonSpend && meditationButtonSpend<=1600f)
        {
            pointsRemain -= meditationButtonSpend;

            PlayerControl.Instance.meditationSpeed *= 1.25f;

            meditationButtonSpend *= 2;
            meditationLevel += 1;

            //�����ı�
            PointsNew();
            meditationLevelText.text = "LEVEL:" + meditationLevel.ToString("0");
        }
    }
}
 