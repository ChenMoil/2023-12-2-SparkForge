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
        //设置Content的大小
        //RectTransform content = scrollRect.content;
        //content.sizeDelta = new Vector2(0, 1000);

        // 设置滚动视图的滚动范围
        scrollRect.verticalNormalizedPosition = 1;
        scrollRect.movementType = ScrollRect.MovementType.Clamped;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
