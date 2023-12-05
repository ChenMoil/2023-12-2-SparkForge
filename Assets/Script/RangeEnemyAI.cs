using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RangeEnemyBlackboard : BlockBorad
{
    public int initHp;   //初始血量

    public float speed;  //速度

    public float damage; //伤害

    public float createTime = 1.0f; //生成过渡时间

    public int maxDistance;  //与玩家的最大距离(超过这个距离会逃跑)
}

/// <summary>
/// 远程躲闪型敌人的ai
/// </summary>
public class RangeEnemyAI : AiParent
{
    //储存数据
    public RangeEnemyBlackboard blackboard;

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
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new RangeAI_Create(fsm));
        fsm.AddState(StateType.Attack, new RangeAI_Attack(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }

    //发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            ImpetuousBar.instance.TakeDamage(blackboard.damage);
        }
    }
}

//生成状态
public class RangeAI_Create : IState
{
    private RangeEnemyBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public RangeAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as RangeEnemyBlackboard;
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
public class RangeAI_Attack : IState
{
    public RangeEnemyBlackboard blackBoard;

    private FSM fsm;
    public RangeAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as RangeEnemyBlackboard;
    }
    public void OnEnter()
    {

    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {

    }

    public void OnUpdate()
    {
        //该单位与玩家的距离
        Vector2 distance = PlayerControl.Instance.transform.position - blackBoard.self.transform.position;
        Vector2 toward = distance.normalized; //移动的方向
        if (distance.sqrMagnitude > blackBoard.maxDistance * blackBoard.maxDistance * 1.05f)
        {
            toward *= 1;
        }
        else if (distance.sqrMagnitude > blackBoard.maxDistance * blackBoard.maxDistance * 0.95f)
        {
            toward *= 0;
        }
        else { toward *= -1; }
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed;
    }
}