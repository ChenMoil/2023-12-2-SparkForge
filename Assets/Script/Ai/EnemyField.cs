using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人盾牌
public class EnemyField : MonoBehaviour
{
    public int HP;
    
    public void TakeDamegeToField(int damage)
    {
        PopupText.Create(transform.position, damage, 3);
        HP -= damage;
        if (HP <= 0)
        {
            gameObject.SetActive(false);
        }
    }
}
