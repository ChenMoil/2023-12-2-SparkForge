using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

/// <summary>
/// 对象池，可以访问它的单例
/// </summary>
public class ObjectPool : MonoBehaviour
{
    // 单例
    public static ObjectPool Instance;

    //根据不同对象分配不同的队列
    private Dictionary<string, Queue<GameObject>> pool = new Dictionary<string, Queue<GameObject>>();

    private Dictionary<GameObject, string> tags = new Dictionary<GameObject, string>();

    private void Awake()
    {
        Instance = this;
    }

    /// <summary>
    /// 向对象池返回GameObject
    /// </summary>
    public void ReturnCacheGameObject(GameObject obj)
    {
        if(obj == null) {  return; }

        obj.transform.SetParent(gameObject.transform);
        obj.SetActive(false);

        if (tags.ContainsKey(obj))
        {
            string tag = tags[obj];
            RemoveOutMark(obj); //去除标记

            if (!pool.ContainsKey(tag))
            {
                //生成对应的队列
                pool.Add(tag, new Queue<GameObject>());
            }

            //入列
            pool[tag].Enqueue(obj);
        }
    }

    /// <summary>
    /// 向对象池请求GameObject
    /// </summary>
    public GameObject RequestCacheGameObejct(GameObject prefab)
    {
        string tag = prefab.GetInstanceID().ToString();
        GameObject obj = GetFromPool(tag);
        if(obj == null)
        {
            obj = GameObject.Instantiate(prefab);


            obj.transform.SetParent(gameObject.transform);

            obj.name = prefab.name + Time.time;
        }

        MarkAsOut(obj, tag); 
        return obj;
    }

    /// <summary>
    /// 通过tag直接从对象池中得到已经缓存的物体，如果没有缓存过的，返回null
    /// </summary>
    /// <param name="tag"></param>
    /// <returns></returns>
    private GameObject GetFromPool(string tag)
    {
        if (pool.ContainsKey(tag) && pool[tag].Count > 0)
        {
            GameObject obj = pool[tag].Dequeue();
            obj.SetActive(true);
            return obj;
        }
        else
        {
            return null;
        }
    }

    /// <summary>
    /// 标记物体
    /// </summary>
    /// <param name="obj"></param>
    /// <param name="tag"></param>
    private void MarkAsOut(GameObject obj, string tag) 
    {
        tags.Add(obj, tag);
    }

    /// <summary>
    /// 移除物体的标记
    /// </summary>
    /// <param name="obj"></param>
    private void RemoveOutMark(GameObject obj)
    {
        if (tags.ContainsKey(obj))
        {
            tags.Remove(obj);
        }
        else
        {
            Debug.LogError(obj.name + " 未被标记");
        }
    }
    public void ClearAll()
    {
        foreach (Transform transform in GetComponentsInChildren<Transform>())
        {
            gameObject.gameObject.SetActive(false);
        }
    }
}
