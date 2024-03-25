using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    //伤害
    private int damage;

    void Start()
    {
        damage = BombBullet.Damage;
        StartCoroutine(Bombing(0.3f));
    }

    private void OnEnable()
    {
        
    }
    private void OnTriggerEnter2D(Collider2D collision)
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
    IEnumerator Bombing(float time)
    {
        float Timer = 0;
        while (Timer < time)
        {
            Timer += Time.deltaTime;
            transform.localScale = new Vector3(3 * Timer / time, 3 * Timer / time, 3 * Timer / time);
            yield return null;
        }
        yield return new WaitForSeconds(0.2f);
       
        Destroy(gameObject);
    }
}
