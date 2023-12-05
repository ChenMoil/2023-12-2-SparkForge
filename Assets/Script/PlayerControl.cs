using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerControl : MonoBehaviour
{
    public static PlayerControl Instance;

    //Player物体控制脚本
    private PlayerInput playerInputControl;

    //玩家的速度
    public float playerSpeed;
    //冥想影响速度的大小， 最终速度 = playerSpeed * meditationSpeed
    public float meditationSpeed;

    //刚体组件
    private Rigidbody2D playerRigidbody;

    //冥想时间计时器
    private float meditationTimer = 0;
    //攻击时间计时器
    private float attackTimer = 0;

    //玩家当前的浮躁阶段(0 - 6)
    private int curState = 0;
    //各阶段子弹的列表
    public List<GameObject> bulletList = new List<GameObject>();
    //子弹的速度
    public int[] bulletSpeed;
    //各阶段的攻速(每次发射子弹的时间间隔)
    public float[] attackSpeed;
    void Start()
    {
        //玩家刚体初始化
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //玩家的冥想函数
        bool isMeditation = Meditation();
        //玩家的移动函数
        PlayerMove(isMeditation);
        //玩家的攻击函数
        Attack();
    }

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        playerInputControl = new PlayerInput();
    }
    private void OnEnable()
    {
        playerInputControl.PlayerControl.Enable();
    }
    private void OnDisable()
    {
        playerInputControl.PlayerControl.Disable();
    }

    private void PlayerMove(bool isMeditation)
    {
        //读取输入的数据
        Vector2 playMove = playerInputControl.PlayerControl.Move.ReadValue<Vector2>();

        if (isMeditation) { playMove *= meditationSpeed; }

        //将速度赋值给刚体
        playerRigidbody.velocity = playMove * playerSpeed;
    }

    /// <summary>
    /// 冥想函数
    /// </summary>
    /// <returns>是否正在冥想</returns>
    private bool Meditation()
    {
        //检测到玩家正在按空格
        if (playerInputControl.PlayerControl.Meditation.IsPressed() == true)
        {
            meditationTimer += Time.deltaTime;

            //每0.1s减少一次浮躁值
            if (meditationTimer >= 0.1f)
            {
                ImpetuousBar.instance.Meditation(1);
                meditationTimer = 0;
            }

            return true;
        }
        return false;
    }

    private void Attack()
    {
        if (playerInputControl.PlayerControl.Attack.IsPressed() == true)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer >= attackSpeed[curState])
            {
                //重置计时器
                attackTimer = 0;

                //子弹的初始速度
                float initialVelocity = bulletSpeed[curState];

                //子弹的方向(朝向鼠标)
                Vector2 towards = ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector2)gameObject.transform.position).normalized;

                //创造子弹
                GameObject newBullet = ObjectPool.Instance.RequestCacheGameObejct(bulletList[curState]);

                newBullet.transform.position = (Vector2)gameObject.transform.position + (Vector2)towards;
                newBullet.GetComponent<Rigidbody2D>().velocity = initialVelocity * towards;
            }
        }
    }
}
