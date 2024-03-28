using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PopupText : MonoBehaviour
{
    public static void Create(Vector3 position, int damageAmount, int HurtType)
    {
        GameObject newPopup = ObjectPool.Instance.RequestCacheGameObejct(GameManger.Instance.PopupPrefab);
        newPopup.transform.position = position;
        PopupText popupText = newPopup.GetComponent<PopupText>();
        popupText.Setup(damageAmount, HurtType);
    }

    private TextMeshPro _textMeshPro;
    private Color _textColor;

    [Header("Move Up")]
    public Vector3 moveUpVector = Vector3.up;
    public float moveUpSpeed = 2.0f;

    [Header("Move Down")]
    public Vector3 moveDownVector = Vector3.down;

    [Header("Disapper")]
    public float disappearSpeed = 3.0f;
    private const float DisappearTimeMax = 0.5f;
    private float _disappearTimer;

    [Header("Damage Color")]
    public Color normalColor;  //HurtType == 0
    public Color beHurtColor;  //HurtType == 1
    public Color HealBackColor;     //HurtType == 2
    public Color FieldHurtColor;     //HurtType == 3
    public Color EnemyHealColor;     //HurtType == 4
    private void OnEnable()
    {
        moveUpVector = Vector3.up;
        moveDownVector = Vector3.down;
        transform.localScale = new Vector3(1, 1, 1);
    }
    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshPro>();
    }
    private void Setup(int damageAmount, int HurtType)
    {
        _textMeshPro.SetText(damageAmount.ToString());

        if (HurtType == 0) 
        {
            _textMeshPro.fontSize = 5;
            _textColor = normalColor;
        }
        else if(HurtType == 1)
        {
            _textMeshPro.fontSize = 7;
            _textColor = beHurtColor;
        }
        else if (HurtType == 2)
        {
            _textMeshPro.fontSize = 6;
            _textColor = HealBackColor;
        }
        else if (HurtType == 3)
        {
            _textMeshPro.fontSize = 5;
            _textColor = FieldHurtColor;
        }
        else if (HurtType == 4)
        {
            _textMeshPro.fontSize = 5;
            _textColor = EnemyHealColor;
        }

        _textMeshPro.color = _textColor;
        _disappearTimer = DisappearTimeMax;
    }

    private void Update()
    {
        //move up
        if (_disappearTimer > DisappearTimeMax + 0.5f)
        {
            transform.position += moveUpVector * Time.deltaTime;
            moveUpVector += moveUpVector * (Time.deltaTime * moveUpSpeed);
            transform.localScale += Vector3.one * (Time.deltaTime * 1);
        }
        else
        {
            transform.position -= moveUpVector * Time.deltaTime;
            transform.localScale -= Vector3.one * (Time.deltaTime * 1);
        }

        //disappear
        _disappearTimer -= Time.deltaTime;
        if (_disappearTimer < 0)
        {
            //alpha
            _textColor.a -= disappearSpeed * Time.deltaTime;
            _textMeshPro.color = _textColor;
            if (_textColor.a < 0)
            {
                ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            }
        }
    }
}
