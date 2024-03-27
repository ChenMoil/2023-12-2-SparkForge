using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;


/// <summary>
/// 控制敌人生成的脚本
/// </summary>
public class EnemySpawn : MonoBehaviour
{
    public static EnemySpawn instance;
    //敌人预制体列表
    [Header("敌人预制体列表")]
    public List<GameObject> enemyList = new List<GameObject>();

    //每种怪物生成的权重
    [Header("每种怪物生成的权重")]
    public int[] spawnWeight;
    //根据权重得到的怪物对应的随机数区间(最大值)
    private int[] weightRange;

    //各阶段怪物生成速度 个/s
    [Header("各阶段怪物生成速度 个/s")]
    public int[] spawnSpeed;
    //各阶段对应的时间区间（区间的最大值）
    [Header("各阶段对应的时间区间（区间的最大值）")]
    public int[] spawnRange;
    //当前阶段
    public int curStage;

    //怪物会距离玩家多远刷新（X轴）
    public float distanceX;
    //怪物会距离玩家多远刷新（Y轴）
    public float distanceY;

    //地图边界
    public float up, down, right, left;
    //计时器
    private float timer;
    //启用怪物刷新的时间
    public float enableTime = 0;

    //是否开始怪物刷新
    [Header("是否开始怪物刷新")]
    public bool isStartEnemySpawn;
    private void Awake()
    {
        instance = this;
    }
    void Start()
    {
        enableTime = Time.time;
        //生成 怪物对应的随机数区间 数组
        weightRange = new int[spawnWeight.Length];
        for (int i = 0; i < spawnWeight.Length; i++)
        {
            weightRange[i] += spawnWeight[i] + (i == 0 ? 0 : weightRange[i - 1]);
        }
    }

    
    void Update()
    {
        
        timer += Time.deltaTime;
        if(timer >= 1 && isStartEnemySpawn == true) //计时器每次到达1s生成一次怪物
        {
            for (int i = 0;i < spawnRange.Length; i++)
            {
                if (Time.time - enableTime <= spawnRange[i])
                {
                    curStage = i;
                    break;
                }
            }

            StartCoroutine(SpawnEnemy(spawnSpeed[curStage]));
            timer = 0;
        }
    }


    /// <summary>
    /// 生成怪物的协程
    /// </summary>
    /// <param name="number">生成怪物数量</param>
    /// <returns></returns>    
    IEnumerator SpawnEnemy(int number)
    {
        float x; //(-1 , 1)
        float y; //(-1 , 1)
        int direction;
        Vector2 spawnPosition; //生成的坐标
        for (int i = 0; i < number; i++)
        {
            //随机出 生成坐标
            x = UnityEngine.Random.Range(-1f, 1f);
            y = math.sqrt(1 - x * x);
            direction = UnityEngine.Random.Range(0, 2);
            if (direction == 0) { y *= -1; };
            spawnPosition.x = PlayerControl.Instance.transform.position.x + distanceX * x;
            spawnPosition.y = PlayerControl.Instance.transform.position.y + distanceY * y;
            //生成在地图边界之外->重新生成
            if (spawnPosition.x < left || spawnPosition.x > right || spawnPosition.y > up || spawnPosition.y < down)
            {
                i--;
                continue;
            }

            //随机出生成敌人类型
            int random = UnityEngine.Random.Range(1, weightRange[weightRange.Length - 1] + 1);
            for (int j = 0; j < weightRange.Length; j++)
            {
                if (random <= weightRange[j])
                {
                    //通过对象池生成新敌人
                    GameObject newEnemy = ObjectPool.Instance.RequestCacheGameObejct(enemyList[j]);
                    newEnemy.transform.position = spawnPosition;

                    //退出随机生成敌人循环，保证只生成一个敌人
                    break;
                }
            }
            yield return 0;
        }
    }
}
