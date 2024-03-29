using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Particle : MonoBehaviour
{
    //销毁时间
    public float DestoryTime;
    //启用
    private void OnEnable()
    {
        StartCoroutine(Destory(DestoryTime));
    }


    //定时返回至对象池
    IEnumerator Destory(float Time)
    {
        yield return new WaitForSeconds(Time);
        ObjectPool.Instance.ReturnCacheGameObject(this.gameObject);
    }
}
