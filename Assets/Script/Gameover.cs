using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Gameover : MonoBehaviour
{
    //����
    public static Gameover instance;

    private void Awake()
    {
        instance = this;
    }

    public GameObject gameoverScreen;

    public TextMeshProUGUI pointsRemainText;
    public float pointsRemain;

    //public TextMeshProUGUI pointsSpendText;

    //����ǿ������
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

    //���¿�ʼ
    public void Replay()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    //�˳���Ϸ
    public void Exit()
    {

    }

    //��������
    public void PointsRemain()
    {
        pointsRemain = LevelManager.instance.timer;

        pointsRemainText.text = "Points:" + pointsRemain.ToString("00");
    }

    //ǿ��
    public void ImpetuousButton()
    {
        impetuousButtonSpend *= 2;


    }
}
 