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
    public void BossSpawnSign(float signTime, float dialogueTime)
    {
        StartCoroutine(ShowPic(Boss0TopImage[0], top, 0, signTime));
        List<Sprite> list = new List<Sprite>();
        list.Add(Boss0BottomLeftImage[0]);
        list.Add(Boss0BottomLeftImage[1]);
        StartCoroutine(ShowBottomLeftImage(list, signTime, dialogueTime));
    }
    public void BossDeadSign(float signTime, float dialogueTime)
    {
        StartCoroutine(ShowPic(Boss0TopImage[1], top, 0, signTime));
        List<Sprite> list = new List<Sprite>();
        list.Add(Boss0BottomLeftImage[2]);
        list.Add(Boss0BottomLeftImage[3]);
        list.Add(Boss0BottomLeftImage[4]);
        StartCoroutine(ShowBottomLeftImage(list, signTime, dialogueTime));
    }
    IEnumerator ShowPic(Sprite newSprite,Image image ,float waitTime,float time)
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
        //显示
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
