using System.Collections;
using UnityEngine;
using Text = UnityEngine.UI.Text;

public class SignUI : MonoBehaviour
{
    private Text text;
    public static SignUI instance;
    // Start is called before the first frame update
    void Start()
    {
        if (instance == null)
        {
            instance = this;
        }
        instance.text = instance.GetComponent<Text>();
    }
    public void DisplayText(string newText, float time, Color color)  //提醒玩家的UI的展示(短时间展示)
    {
        StopAllCoroutines();  //先结束其他协程
        StartCoroutine(FadeCoroutine(0.3f, time));
        instance.text.text = newText;
        instance.text.color = color;
    }
    public void DisplayText(string newText, bool permanent, Color color)  //提醒玩家的UI的展示(长期展示)
    {
        if (permanent)
        {
            StopAllCoroutines();  //先结束其他协程
            StartCoroutine(FadeCoroutine(0.2f, 10000000f));
            instance.text.text = newText;
            instance.text.color = color;
        }
    }
    IEnumerator FadeCoroutine(float fadeTime, float sumTime) //用协程做出淡入淡出效果
    {
        float waitTime = 0;
        float allTime = sumTime / fadeTime;
        while (waitTime < 1)
        {
            instance.text.color = new Color(instance.text.color.r, instance.text.color.g, instance.text.color.b, waitTime);
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
        while (waitTime < allTime - 1)
        {
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
        while (waitTime < allTime)
        {
            instance.text.color = new Color(instance.text.color.r, instance.text.color.g, instance.text.color.b, allTime - waitTime);
            yield return null;
            waitTime += Time.deltaTime / fadeTime;
        }
    }
}
