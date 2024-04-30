using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class dialogue : MonoBehaviour
{
    public static dialogue instance;

    public Image top;
    public Image BottomLeft;


    public List<Sprite> Boss0TopImage;
    public List<Sprite> Boss0BottomLeftImage;
    public void BossSpawnSign(int boss, float signTime, float dialogueTime)
    {
        List<List<Sprite>> allLists = GetSpriteList(boss);
        List<Sprite> topImage = allLists[0];
        StartCoroutine(ShowTopPic(topImage[0], top, 0, signTime));
        ShowBottomLeftImage(boss, signTime, dialogueTime);
    }
    public void BossDeadSign(int boss, float signTime)
    {
        List<List<Sprite>> allLists = GetSpriteList(boss);
        List<Sprite> topImage = allLists[0];
        StartCoroutine(ShowTopPic(topImage[1], top, 0, signTime));
    }
    private void ShowBottomLeftImage(int boss, float waitTime, float time)
    {
        List<List<Sprite>> allLists = GetSpriteList(boss);
        List<Sprite> BottomLeftImage = allLists[1];
        StartCoroutine(ShowBottomLeftImage(BottomLeftImage, waitTime, time));
    }
    private List<List<Sprite>> GetSpriteList(int boss)
    {
        List<List<Sprite>> res = new List<List<Sprite>>();
        if (boss == 0)
        {
            res.Add(Boss0TopImage);
            res.Add(Boss0BottomLeftImage);
        }
        return res;
    }
    IEnumerator ShowTopPic(Sprite newSprite,Image image ,float waitTime,float time)
    {
        yield return new WaitForSeconds(waitTime);
        //显示
        image.color = Color.white;
        image.sprite = newSprite;
        yield return new WaitForSeconds(time);
        //不显示
        image.color = new Color(1, 1, 1, 0);
    }
    IEnumerator ShowBottomLeftImage(List<Sprite> BottomLeftImage,float waitTime, float time)
    {
        yield return new WaitForSeconds(waitTime);
        BottomLeft.color = Color.white;
        int length = BottomLeftImage.Count;
        float divideTime = time / length;
        for (int i = 0; i < length; i++)
        {
            BottomLeft.sprite = BottomLeftImage[i];
            yield return new WaitForSeconds(divideTime);
        }
        //不显示
        BottomLeft.color = new Color(1, 1, 1, 0);
    }

    public void Awake()
    {
        instance = this;
    }
}
