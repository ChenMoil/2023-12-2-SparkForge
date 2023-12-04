using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour
{
    public static GameManger Instance;
    [NonSerialized]public GameObject playerGameObject; //玩家物体

    public List<GameObject> TestEnemyGameObjects; //测试用
    public List<GameObject> TestGameObjects; //测试用
    public List<GameObject> TestGameObjects1; //测试用
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerGameObject = GameObject.Find("Player");
    }
    private void Start() //测试
    {
        //TestGameObjects = new List<GameObject>();
        //for (int i = 0; i < 10; i++)
        //{
        //    TestGameObjects.Add(ObjectPool.Instance.RequestCacheGameObejct(TestEnemyGameObjects[0]));
        //}
        //foreach(GameObject gameObject in TestGameObjects)
        //{
        //    ObjectPool.Instance.ReturnCacheGameObject(gameObject);
        //}
        //for (int i = 0; i < 5; i++)
        //{
        //    TestGameObjects.Add(ObjectPool.Instance.RequestCacheGameObejct(TestEnemyGameObjects[0]));
        //}

        //for (int i = 0; i < 10; i++)
        //{
        //    TestGameObjects1.Add(ObjectPool.Instance.RequestCacheGameObejct(TestEnemyGameObjects[1]));
        //}
        //foreach (GameObject gameObject in TestGameObjects1)
        //{
        //    ObjectPool.Instance.ReturnCacheGameObject(gameObject);
        //}
        //for (int i = 0; i < 11; i++)
        //{
        //    TestGameObjects1.Add(ObjectPool.Instance.RequestCacheGameObejct(TestEnemyGameObjects[1]));
        //}
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
