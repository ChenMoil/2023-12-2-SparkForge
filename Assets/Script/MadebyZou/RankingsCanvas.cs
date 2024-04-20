using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RankingsCanvas : MonoBehaviour
{
    //历史积分
    public Text[] rankText = new Text[6];
    //当前积分
    public Text currentPoints;
    public float points;

    //游戏对象
    public GameObject rankingsCanvasObject;

    //单例访问
    public static RankingsCanvas instance;
    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        rankingsCanvasObject.SetActive(false);

        //赋值
        //历史值
        points = DataStorage.instance.obtainPoints;
        ProcessingDate();
        //当前值
        currentPoints.text = "CurrentPoints:  " + DataStorage.instance.obtainPoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //转到商店面板
    public void ShopButton()
    {
        PlayerPrefs.SetInt("SecondNextButtonIsPush", 1);
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);

        //调出排行榜面板
        Shop.instance.shopCanvasObject.SetActive(true);
    }

    //生成新的排行榜
    public void ProcessingDate()
    {
        //历史数据
        float[] pointsTable = new float[7];
        pointsTable[0] = PlayerPrefs.GetFloat("pointsRank0");
        pointsTable[1] = PlayerPrefs.GetFloat("pointsRank1");
        pointsTable[2] = PlayerPrefs.GetFloat("pointsRank2");
        pointsTable[3] = PlayerPrefs.GetFloat("pointsRank3");
        pointsTable[4] = PlayerPrefs.GetFloat("pointsRank4");
        pointsTable[5] = PlayerPrefs.GetFloat("pointsRank5");
        pointsTable[6] = points;

        //排序处理(升序)
        Array.Sort(pointsTable);

        //记录新的排行榜
        PlayerPrefs.SetFloat("pointsRank0", pointsTable[6]);
        PlayerPrefs.SetFloat("pointsRank1", pointsTable[5]);
        PlayerPrefs.SetFloat("pointsRank2", pointsTable[4]);
        PlayerPrefs.SetFloat("pointsRank3", pointsTable[3]);
        PlayerPrefs.SetFloat("pointsRank4", pointsTable[2]);
        PlayerPrefs.SetFloat("pointsRank5", pointsTable[1]);

        //给榜赋值
        for(int i = 0; i < 6; i++)
        {
            rankText[i].text = $"TOP{i+1}:  " + pointsTable[6 - i].ToString();
        }
    }
}
