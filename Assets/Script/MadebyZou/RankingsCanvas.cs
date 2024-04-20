using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using UnityEngine.SceneManagement;

public class RankingsCanvas : MonoBehaviour
{
    //��ʷ����
    public Text[] rankText = new Text[6];
    //��ǰ����
    public Text currentPoints;
    public float points;

    //��Ϸ����
    public GameObject rankingsCanvasObject;

    //��������
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

        //��ֵ
        //��ʷֵ
        points = DataStorage.instance.obtainPoints;
        ProcessingDate();
        //��ǰֵ
        currentPoints.text = "CurrentPoints:  " + DataStorage.instance.obtainPoints.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //ת���̵����
    public void ShopButton()
    {
        PlayerPrefs.SetInt("SecondNextButtonIsPush", 1);
        StartCoroutine(LoadLevel());
    }

    IEnumerator LoadLevel()
    {
        yield return new WaitForSeconds(2f);

        //�������а����
        Shop.instance.shopCanvasObject.SetActive(true);
    }

    //�����µ����а�
    public void ProcessingDate()
    {
        //��ʷ����
        float[] pointsTable = new float[7];
        pointsTable[0] = PlayerPrefs.GetFloat("pointsRank0");
        pointsTable[1] = PlayerPrefs.GetFloat("pointsRank1");
        pointsTable[2] = PlayerPrefs.GetFloat("pointsRank2");
        pointsTable[3] = PlayerPrefs.GetFloat("pointsRank3");
        pointsTable[4] = PlayerPrefs.GetFloat("pointsRank4");
        pointsTable[5] = PlayerPrefs.GetFloat("pointsRank5");
        pointsTable[6] = points;

        //������(����)
        Array.Sort(pointsTable);

        //��¼�µ����а�
        PlayerPrefs.SetFloat("pointsRank0", pointsTable[6]);
        PlayerPrefs.SetFloat("pointsRank1", pointsTable[5]);
        PlayerPrefs.SetFloat("pointsRank2", pointsTable[4]);
        PlayerPrefs.SetFloat("pointsRank3", pointsTable[3]);
        PlayerPrefs.SetFloat("pointsRank4", pointsTable[2]);
        PlayerPrefs.SetFloat("pointsRank5", pointsTable[1]);

        //����ֵ
        for(int i = 0; i < 6; i++)
        {
            rankText[i].text = $"TOP{i+1}:  " + pointsTable[6 - i].ToString();
        }
    }
}
