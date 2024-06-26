﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    //子弹的伤害
    [NonSerialized] public int damage;
    //自动销毁的时间
    [SerializeField] private float destoryTime;
    //是否可以穿透敌人
    [SerializeField] private bool isPenetrate;
    private void OnEnable()
    {
        StartCoroutine(RegularDestory(destoryTime));
    }

    void Update()
    {

    }

    /// <summary>
    /// 定时销毁该物体
    /// </summary>
    /// <param name="time">销毁时间</param>
    /// <returns></returns>
    IEnumerator RegularDestory(float time)
    {
        yield return new WaitForSeconds(time);
        ObjectPool.Instance.ReturnCacheGameObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //如果击中的是敌人
        if (collision.gameObject.tag == "Player")
        {
            ImpetuousBar.instance.TakeDamage(damage * AiParent.attackDamageMultiplier); //攻击乘以倍率
            Vector2 towards = (collision.gameObject.transform.position - gameObject.transform.position).normalized;
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(towards * 200f);

            if (!isPenetrate)
            {
                //结束所有协程
                StopAllCoroutines();
                ObjectPool.Instance.ReturnCacheGameObject(gameObject);
            }
        }
    }

}
