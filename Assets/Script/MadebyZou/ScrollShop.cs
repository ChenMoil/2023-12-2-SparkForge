using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollShop : MonoBehaviour
{
    public ScrollRect scrollRect;
    // Start is called before the first frame update
    void Start()
    {
        //����Content�Ĵ�С
        //RectTransform content = scrollRect.content;
        //content.sizeDelta = new Vector2(0, 1000);

        // ���ù�����ͼ�Ĺ�����Χ
        scrollRect.verticalNormalizedPosition = 1;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
