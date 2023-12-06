using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiParent : MonoBehaviour
{
    //血量
    [NonSerialized] public int HP;
    //怪物死亡降低的浮躁条
    public int reduceImpetuousBar = 2;
    public FSM fsm;
    public void TakeDamege(int damege)
    {
        HP -= damege;
        if (HP <= 0)
        {
            ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            GameManger.Instance.enemyKill++;

            //浮躁条数值减少2
            ImpetuousBar.instance.TakeDamage(-2);
        }
    }
}
