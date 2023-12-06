using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiParent : MonoBehaviour
{
    //血量
    [NonSerialized] public int HP;

    public FSM fsm;
    public void TakeDamege(int damege)
    {
        HP -= damege;
        if (HP <= 0)
        {
            ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            GameManger.Instance.enemyKill++;
        }
    }
}
