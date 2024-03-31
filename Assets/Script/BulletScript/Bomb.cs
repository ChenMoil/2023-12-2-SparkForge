using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{

    public enum BombType
    {
        PlayerBombBullet, //玩家的炸弹
        ArrogantBoss,     //傲慢BOSS的震荡
    }
    //伤害
    [NonSerialized]public int damage;
    //大小
    public float scale;
    public BombType bombType;
    //爆炸扩散时间
    public float bombTime;
    //扩散完毕后销毁时间
    public float destoryTime;
    void Start()
    {
        //开始协程
        StartCoroutine(Bombing(bombTime, destoryTime));
    }

    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(bombType == BombType.PlayerBombBullet)
        {
            //如果击中的是敌人
            if (collision.gameObject.tag == "Enemy")
            {
                AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[6], 1f, 0, 1f);
                GameManger.Instance.GetAi[collision.gameObject].TakeDamege(damage);
                Vector2 towards = (collision.gameObject.transform.position - gameObject.transform.position).normalized;

                collision.gameObject.GetComponent<Rigidbody2D>().AddForce(towards * 500f);
            }
            if (collision.gameObject.tag == "EnemyField")
            {
                //AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[6], 1f, 0, 1f);
                collision.gameObject.GetComponent<EnemyField>().TakeDamegeToField(damage);
            }
        }
        if (bombType == BombType.ArrogantBoss)
        {
            if (collision.gameObject.tag == "Player")
            {
                ImpetuousBar.instance.TakeDamage(damage);

            }
        }
    }
    IEnumerator Bombing(float bombTime, float destoryTime)
    {
        float Timer = 0;
        while (Timer < bombTime)
        {
            Timer += Time.deltaTime;
            transform.localScale = new Vector3(scale * Timer / bombTime, scale * Timer / bombTime, scale * Timer / bombTime);
            yield return null;
        }
        yield return new WaitForSeconds(destoryTime);
       
        Destroy(gameObject);
    }
}
