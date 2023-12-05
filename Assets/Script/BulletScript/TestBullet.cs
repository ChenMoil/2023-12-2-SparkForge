using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestBullet : MonoBehaviour
{
    //子弹的伤害
    [SerializeField] private int damage;

    private void OnEnable()
    {
        StartCoroutine(RegularDestory(5f));
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

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("击中敌人");
        //如果击中的是敌人
        if (collision.gameObject.tag == "Enemy")
        {
            GameManger.Instance.GetAi[collision.gameObject].TakeDamege(damage);
        }
    }
}
