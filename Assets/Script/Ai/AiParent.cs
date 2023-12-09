using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiParent : MonoBehaviour
{
    //怪物攻速倍率（根据浮躁条变化）(与黑板中的攻击间隔相乘) / 1.2
    public static float attackSpeedMultiplier = 1; 
    //怪物攻击倍率（根据浮躁条变化）* 1.2
    public static float attackDamageMultiplier = 1;
    //怪物速度倍率（根据浮躁条变化）* 1.2
    public static float moveSpeedMultiplier = 1;

    //当前血量
    [NonSerialized] public int HP;
    //怪物死亡降低的浮躁条
    public int reduceImpetuousBar = 2;
    public FSM fsm;
    public void TakeDamege(int damege)
    {
        HP -= damege;
        //怪物受伤 0
        PopupText.Create(transform.position, damege, 0);
        if (HP <= 0)
        {
            ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            GameManger.Instance.enemyKill++;

            //浮躁条数值减少2
            ImpetuousBar.instance.Meditation(2);
        }
    }
}
