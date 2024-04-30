using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AiParent : MonoBehaviour
{
    //怪物攻速倍率（根据浮躁条变化）(与黑板中的攻击间隔相乘) / 1.2
    public static float attackSpeedMultiplier = 1; 
    //怪物速度倍率（根据浮躁条变化）* 1.2
    public static float moveSpeedMultiplier = 1;
    //怪物攻击倍率（根据浮躁条变化）* 1.2
    public static float attackDamageMultiplier = 1;

    //当前血量
    [NonSerialized] public int HP;
    [Header("初始血量")]
    public int initHp; //初始血量
    //怪物死亡降低的浮躁条
    public int reduceImpetuousBar = 0;
    public FSM fsm;
    public virtual void TakeDamege(int damege)
    {
        //生成粒子效果
        ParticleManger.instance.ShowParticle(0, this.gameObject);
        HP -= damege;
        //怪物受伤 颜色0
        PopupText.Create(transform.position, damege, 0);
        if (HP <= 0)
        {
            ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            GameManger.Instance.enemyKill++;

            //浮躁条数值减少
            ImpetuousBar.instance.Meditation(reduceImpetuousBar);
        }
    }

    /// <summary>
    /// 怪物回血
    /// </summary>
    /// <param name="heal"></param>
    public void TakeHeal(int heal)
    {
        if (HP < initHp)
        {
            HP += heal;
            //怪物回血 颜色4
            PopupText.Create(transform.position, heal, 4);
        }
    }
}
