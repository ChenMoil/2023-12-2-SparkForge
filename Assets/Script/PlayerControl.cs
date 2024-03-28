using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerControl : MonoBehaviour
{
    private GameObject handParent;

    public static PlayerControl Instance;

    //Player物体控制脚本
    private PlayerInput playerInputControl;

    //玩家的速度
    [Header("玩家的速度")]
    public float playerSpeed;
    //冥想影响速度的大小， 最终速度 = playerSpeed * meditationSpeed
    [Header("冥想影响速度的大小， 最终速度 = playerSpeed * meditationSpeed")]
    public float meditationSpeed;
    //冥想每秒减少的数值
    [Header("冥想每秒减少的数值")]
    public float meditation;
    //每秒冥想触发几次
    [Header("每秒冥想触发几次")]
    public int meditationTimes;

    //刚体组件
    private Rigidbody2D playerRigidbody;
    //动画器
    [NonSerialized]public Animator animator;

    //冥想时间计时器
    private float meditationTimer = 0;
    //攻击时间计时器
    private float attackTimer = 0;

    //各阶段子弹的列表
    [Header("各阶段子弹的列表")]
    public List<GameObject> bulletList = new List<GameObject>();
    //子弹的速度
    [Header("各阶段子弹的速度")]
    public int[] bulletSpeed;
    //各阶段的攻速(每次发射子弹的时间间隔)
    [Header("各阶段的攻速(每次发射子弹的时间间隔)")]
    public float[] attackSpeed;

    //手的x旋转
    public int handXRotation;
    void Start()
    {
        //玩家刚体初始化
        playerRigidbody = GetComponent<Rigidbody2D>();
        //寻找手
        handParent = gameObject.transform.GetChild(0).gameObject;
        animator = GetComponent<Animator>();
        playerRigidbody.freezeRotation = true;   //冻结旋转
    }

    // Update is called once per frame
    void Update()
    {
        //玩家的冥想函数
        bool isMeditation = Meditation();
        //玩家的移动函数
        PlayerMove(isMeditation);
        //玩家的攻击函数
        Attack(isMeditation);

        //调整手的方向
        if (handParent != null /*&& playerInputControl.PlayerControl.JoyStickAttack.ReadValue<Vector2>() != Vector2.zero*/)
            LookAt(Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) + new Vector3(0, 0, 10), handParent);

        //给动画机传入浮躁等级
        animator.SetInteger("CurState", ImpetuousBar.instance.impetuousLevel);
        //给动画机传入是否在冥想
        animator.SetBool("IsMeditation", isMeditation);
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
        playerRigidbody.velocity = playMove * playerSpeed * (1 + 0.1f * Gameover.instance.movespeedLevel);

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

            //每0.5s减少一次浮躁值
            if (meditationTimer >= 1f / meditationTimes)
            {
                AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[5], 0.6f, 0, 2.5f);
                //ImpetuousBar.instance.Meditation(meditation * (1 + Gameover.instance.meditationLevel * 0.25f) / 2);
                ImpetuousBar.instance.Meditation(meditation / meditationTimes * (float)Math.Pow(1.2f, ImpetuousBar.instance.impetuousLevel));
                meditationTimer = 0;
            }

            return true;
        }
        return false;
    }

    private void Attack(bool isMeditation)
    {
        if (!isMeditation && playerInputControl.PlayerControl.Attack.IsPressed() == true)
        {
            attackTimer += Time.deltaTime;

            if (attackTimer * (1 + 0.1f * Gameover.instance.attackspeedLevel) >= attackSpeed[ImpetuousBar.instance.impetuousLevel])
            {
                //播放攻击音效
                AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 0.5f, 0, 1);

                //重置计时器
                attackTimer = 0;

                //子弹的初始速度
                float initialVelocity = bulletSpeed[ImpetuousBar.instance.impetuousLevel];

                //人物朝向鼠标的方向
                Vector2 towards = ((Vector2)Camera.main.ScreenToWorldPoint(Mouse.current.position.ReadValue()) - (Vector2)gameObject.transform.position).normalized;

                //创造子弹
                GameObject newBullet = ObjectPool.Instance.RequestCacheGameObejct(bulletList[ImpetuousBar.instance.impetuousLevel]);

                //改变子弹的初始参数
                newBullet.transform.localEulerAngles = handParent.transform.localEulerAngles - new Vector3(handXRotation, 0, 0);
                newBullet.transform.position = handParent.transform.GetChild(0).position;
                newBullet.GetComponent<Rigidbody2D>().velocity = initialVelocity * towards;
            }
        }

        //读取输入的数据(手机端)
        Vector2 joyStickAttack = playerInputControl.PlayerControl.JoyStickAttack.ReadValue<Vector2>();
        if (joyStickAttack != Vector2.zero && !isMeditation)
        {
            //改变手的位置
            float angle = Mathf.Atan2(joyStickAttack.x, joyStickAttack.y) * Mathf.Rad2Deg;
            handParent.transform.localEulerAngles = new Vector3(handXRotation, 0, -1 * angle + 90);

            attackTimer += Time.deltaTime;
            if (attackTimer * (1 + 0.1f * Gameover.instance.attackspeedLevel) >= attackSpeed[ImpetuousBar.instance.impetuousLevel])
            {
                //播放攻击音效
                AudioManager.instance.PlayOneShot(AudioManager.instance.AudioClip[2], 0.5f, 0, 1);

                //重置计时器
                attackTimer = 0;

                //子弹的初始速度
                float initialVelocity = bulletSpeed[ImpetuousBar.instance.impetuousLevel];

                //创造子弹
                GameObject newBullet = ObjectPool.Instance.RequestCacheGameObejct(bulletList[ImpetuousBar.instance.impetuousLevel]);

                //改变子弹的初始参数
                newBullet.transform.localEulerAngles = handParent.transform.localEulerAngles - new Vector3(handXRotation, 0, 0); 
                newBullet.transform.position = handParent.transform.GetChild(0).position;
                newBullet.GetComponent<Rigidbody2D>().velocity = initialVelocity * joyStickAttack;
            }
        }
    }

    public static void LookAt(Vector3 target, GameObject self)    //朝向其他物体
    {
        Vector3 dir = target - self.transform.position;
        dir.z = 0;
        float angle =
            Vector3.SignedAngle(Vector3.right, dir, Vector3.forward);
        Quaternion rotation = Quaternion.Euler(PlayerControl.Instance.handXRotation, 0, angle);                     //利用角度得到rotation
        self.transform.localRotation = rotation;
        //self.transform.eulerAngles =
        //    Vector3.Lerp(self.transform.eulerAngles, new Vector3(0, 0, angle), 0.1f);
    }

    IEnumerator HurtPlayerTimer(float time)
    {
        PlayerControl.Instance.animator.SetBool("IsHurt", true);
        yield return new WaitForSeconds(time);
        PlayerControl.Instance.animator.SetBool("IsHurt", false);
    }

    public void EnterBeHurtState(float time)
    {
        StopAllCoroutines();
        StartCoroutine(HurtPlayerTimer(time));
    }
}
