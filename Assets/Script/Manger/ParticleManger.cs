using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleManger : MonoBehaviour
{
    public static ParticleManger instance;
    private void Awake()
    {
        instance = this;
    }
    public List<GameObject> ParticleList;

    /// <summary>
    /// 请求粒子效果
    /// </summary>
    /// <param name="index">粒子效果对应索引</param>
    /// <param name="target">生成在那个物体上</param>
    /// <returns></returns>
    public GameObject ShowParticle(int index, GameObject target)
    {
        //请求
        GameObject newParticle = ObjectPool.Instance.RequestCacheGameObejct(ParticleList[index]);
        //改变位置
        newParticle.transform.position = target.transform.position;

        return newParticle;
    }
}
