using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// 控制敌人生成的脚本
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    //敌人预制体列表
    public List<GameObject> enemyList = new List<GameObject>();
    
    //怪物生成速度 个/s
    public int spawnSpeed;

    //怪物会距离玩家多远刷新（X轴）
    public float distanceX;
    //怪物会距离玩家多远刷新（Y轴）
    public float distanceY;

    //地图边界
    public float up, down, right, left;
    //计时器
    private float timer;
    void Start()
    {
        
    }

    
    void Update()
    {
        timer += Time.deltaTime;
        if(timer >= 1) //计时器每次到达1s生成一次怪物
        {
            StartCoroutine(SpawnEnemy(spawnSpeed, enemyList[0]));
            timer = 0;
        }
    }

    /// <summary>
    /// 生成怪物的协程
    /// </summary>
    /// <param name="number">生成怪物数量</param>
    /// <returns></returns>    
    IEnumerator SpawnEnemy(int number, GameObject enemy)
    {
        float x; //(-1 , 1)
        float y; //(-1 , 1)
        int direction;
        Vector2 spawnPosition; //生成的坐标
        for (int i = 0; i < number; i++)
        {
            x = UnityEngine.Random.Range(-1f, 1f);
            y = math.sqrt(1 - x * x);
            direction = UnityEngine.Random.Range(0, 2);
            if (direction == 0) { y *= -1; };

            spawnPosition.x = PlayerControl.Instance.transform.position.x + distanceX * x;
            spawnPosition.y = PlayerControl.Instance.transform.position.y + distanceY * y;

            //通过对象池生成新敌人
            GameObject newEnemy = ObjectPool.Instance.RequestCacheGameObejct(enemy);
            newEnemy.transform.position = spawnPosition;

            yield return 0;
        }
    }
}
