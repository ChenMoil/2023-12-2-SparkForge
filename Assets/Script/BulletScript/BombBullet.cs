using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombBullet : MonoBehaviour
{
    //爆炸的伤害
    public int damage;
    //自动销毁的时间
    [SerializeField] private float destoryTime;


    //爆炸子物体
    public GameObject bombGameObject;
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
        Debug.Log("击中敌人");
        //如果击中的是敌人
        if (collision.gameObject.tag == "Enemy" || collision.gameObject.tag == "EnemyField")
        {
            //生成爆炸
            GameObject bomb = Instantiate(bombGameObject);
            bomb.transform.position = gameObject.transform.position;
            bomb.GetComponent<Bomb>().damage = damage;
            //停止协程，返回导弹
            StopAllCoroutines();
            ObjectPool.Instance.ReturnCacheGameObject(gameObject);
        }
    }

    //IEnumerator Bomb(float time)
    //{
    //    float Timer = 0;
    //    while (Timer < time)
    //    {
    //        Timer += Time.deltaTime;
    //        bombGameObject.transform.localScale = new Vector3(3 * Timer / time, 3 * Timer / time, 3 * Timer / time);
    //        yield return null;
    //    }
    //    //完全变大后等0.1s
    //    yield return new WaitForSeconds(0.1f);
    //    bombGameObject.transform.localScale = Vector3.zero;
    //    bombGameObject.SetActive(false);
    //    ObjectPool.Instance.ReturnCacheGameObject(gameObject);
    //    //结束所有协程
    //    StopAllCoroutines();
    //}
}
