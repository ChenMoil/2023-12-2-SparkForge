using System.Collections;
using System.Collections.Generic;
using System.Net.Sockets;
using UnityEngine;
using UnityEngine.UI;

public class ImpetuousBar : MonoBehaviour
{

    //������ҵĸ�����
    public static ImpetuousBar instance;

    private void Awake()
    {
        instance = this;
    }


    public float currentImpetuousBar, maxImpetuousBar;//��ǰ���������������

    public float timeMultipie;//ʱ������Ӱ�챶��

    public Slider impetuousSlider;//������UI,ͨ����������

    // Start is called before the first frame update
    void Start()
    {
        timeMultipie = 1f;

        currentImpetuousBar = 0f;

        impetuousSlider.maxValue = maxImpetuousBar;
        impetuousSlider.value = currentImpetuousBar;
    }

    // Update is called once per frame
    void Update()
    {

    }

    //����ܻ�
    public void TakeDamage(float damageToTake)
    {
        currentImpetuousBar += damageToTake;

        if(currentImpetuousBar>=maxImpetuousBar)
        {
            gameObject.SetActive(false);

            LevelManager.instance.EndLevel();//��Ϸ����
        }

        impetuousSlider.value = currentImpetuousBar;//UI
    }

    //ʱ������
    public void TimeLapse(float timeToLapse)
    {
        currentImpetuousBar = currentImpetuousBar * timeToLapse * timeMultipie;

        if (currentImpetuousBar >= maxImpetuousBar)
        {
            gameObject.SetActive(false);

            LevelManager.instance.EndLevel();//��Ϸ����
        }

       impetuousSlider.value = currentImpetuousBar;//UI
    }

    //�������
    public void  DestroyEnemy(float enemyToBeDestroy)
    {
        currentImpetuousBar -= enemyToBeDestroy;

        impetuousSlider.value = currentImpetuousBar;//UI
    }

    //ڤ��
    public void Meditation(float meditation)
    {
        currentImpetuousBar -= meditation;

        impetuousSlider.value = currentImpetuousBar;//UI
    }
}
