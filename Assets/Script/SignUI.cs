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
    public void DisplayText(string newText, float time, Color color)  //������ҵ�UI��չʾ(��ʱ��չʾ)
    {
        StopAllCoroutines();  //�Ƚ�������Э��
        StartCoroutine(FadeCoroutine(0.3f, time));
        instance.text.text = newText;
        instance.text.color = color;
    }
    public void DisplayText(string newText, bool permanent, Color color)  //������ҵ�UI��չʾ(����չʾ)
    {
        if (permanent)
        {
            StopAllCoroutines();  //�Ƚ�������Э��
            StartCoroutine(FadeCoroutine(0.2f, 10000000f));
            instance.text.text = newText;
            instance.text.color = color;
        }
    }
    IEnumerator FadeCoroutine(float fadeTime, float sumTime) //��Э���������뵭��Ч��
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
