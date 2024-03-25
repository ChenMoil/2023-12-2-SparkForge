using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

[Serializable]
public class FixedRangeEnemyBlackboard : BlockBorad
{
    public int initHp;   //初始血量

    public float damage; //伤害

    public float createTime = 1.0f; //生成过渡时间

    [NonSerialized]public GameObject handParent; //手

    public GameObject Bullet; //发射的子弹
    public float BulletSpeed; //发射子弹的初始速度

    public float idleTime;   //休息时间
    public float attackBombNumber;   //攻击状态子弹发射数量
    public float attackInterval;   //攻击间隔
}

/// <summary>
/// 远程躲闪型敌人的ai
/// </summary>
public class FixedRangeEnemyAI : AiParent
{
    //储存数据
    public FixedRangeEnemyBlackboard blackboard;

    void Start()
    {
        Init();
    }

    //禁用回到对象池时
    private void OnEnable()
    {

    }

    //从对象池启用时
    private void OnDisable()
    {
        //血量回归初始
        HP = blackboard.initHp;
    }

    // Update is called once per frame
    void Update()
    {
        fsm.OnUpdate();
    }

    private void FixedUpdate()
    {
        fsm.OnFixUpdate();
    }

    //第一次生成初始化时
    private void Init()
    {
        fsm = new FSM(blackboard);
        blackboard.self = gameObject;
        blackboard.rigidbody2D = gameObject.GetComponent<Rigidbody2D>();
        blackboard.spriteRenderer = gameObject.GetComponent<SpriteRenderer>();
        AddState();
        InitState();

        //初始化血量
        HP = blackboard.initHp;
        GameManger.Instance.GetAi.Add(gameObject, this);
        blackboard.handParent = gameObject.transform.GetChild(0).gameObject;
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new FixedRangeAI_Create(fsm));
        fsm.AddState(StateType.Attack, new FixedRangeAI_Attack(fsm));
        fsm.AddState(StateType.Idle, new FixedRangeAI_Idle(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }
}

//生成状态
public class FixedRangeAI_Create : IState
{
    private FixedRangeEnemyBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public FixedRangeAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as FixedRangeEnemyBlackboard;
    }
    public void OnEnter()
    {
        timer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        timer += Time.deltaTime;
        blackBoard.spriteRenderer.color = new Color(1, 1, 1, timer / blackBoard.createTime);
        if (timer > blackBoard.createTime)
        {
            fsm.SwitchState(StateType.Attack);  //切换状态
        }
    }
}

//攻击状态
public class FixedRangeAI_Attack : IState
{
    public FixedRangeEnemyBlackboard blackBoard;

    private FSM fsm;

    private int bulletNumber; //子弹发射数量
    private float timer; //攻击计时器
    public FixedRangeAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as FixedRangeEnemyBlackboard;
    }
    public void OnEnter()
    {
        bulletNumber = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        timer += Time.deltaTime;
    }

    public void OnUpdate()
    {
        blackBoard.rigidbody2D.velocity = Vector3.zero;

        //让手对着玩家
        PlayerControl.LookAt(PlayerControl.Instance.gameObject.transform.position, blackBoard.handParent);

        if (timer > blackBoard.attackInterval)
        {
            timer = 0;


            bulletNumber++;
            //生成子弹
            GameObject newBullet = ObjectPool.Instance.RequestCacheGameObejct(blackBoard.Bullet);
            //改变位置
            newBullet.transform.position = blackBoard.handParent.transform.GetChild(0).position;
            //改变速度
            Vector2 towards = ((Vector2)PlayerControl.Instance.gameObject.transform.position - (Vector2)blackBoard.handParent.transform.position).normalized;
            newBullet.GetComponent<Rigidbody2D>().velocity = blackBoard.BulletSpeed * towards;

        }
        //子弹发射完毕，进入站立状态
        if (bulletNumber >= blackBoard.attackBombNumber) 
        {
            fsm.SwitchState(StateType.Idle);
        }
    }
}

//站立休息状态状态
public class FixedRangeAI_Idle : IState
{
    public FixedRangeEnemyBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public FixedRangeAI_Idle(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as FixedRangeEnemyBlackboard;
    }
    public void OnEnter()
    {
        timer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        timer += Time.deltaTime;
        //站立时间到，切换至攻击状态
        if (timer > blackBoard.idleTime)
        {
            fsm.SwitchState(StateType.Attack);
        }
    }

    public void OnUpdate()
    {
        
    }
}