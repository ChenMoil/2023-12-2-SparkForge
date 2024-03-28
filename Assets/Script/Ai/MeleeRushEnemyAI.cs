using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class MeleeRushBlackboard : BlockBorad
{
    [Header("速度")]
    public float speed;  //速度
    [Header("冲锋速度")]
    public float rushSpeed;  //冲锋速度
    [Header("冲锋距离(距离玩家多远时发起冲锋)")]
    public float rushDistance;  //冲锋距离(距离玩家多远时发起冲锋)
    [Header("冲锋的蓄力时间")]
    public float rushWaitTime;  //冲锋的蓄力时间
    [Header("冲锋持续时间")]
    public float rushTime;      //冲锋持续时间
    [Header("伤害")]
    public float damage; //伤害
    [Header("初始攻击间隔")]
    public float initAttackInterval; //初始攻击间隔

    [NonSerialized]public float lastAttackPlayerTime = 0; //近战敌人上次攻击玩家的时间

    public float createTime = 1.0f; //生成过渡时间
}

/// <summary>
/// 会冲锋的近战敌人的ai
/// </summary>
public class MeleeRushEnemyAI : AiParent
{
    //储存数据
    public MeleeRushBlackboard blackboard;
    
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
        HP = initHp;
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
        HP = initHp;
        GameManger.Instance.GetAi.Add(gameObject, this);
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new MeleeRushAI_Create(fsm));
        fsm.AddState(StateType.Move, new MeleeRushAI_Move(fsm));
        fsm.AddState(StateType.Attack, new MeleeRushAI_Attack(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }

    //发生碰撞
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player" && Time.time - blackboard.lastAttackPlayerTime > attackSpeedMultiplier * blackboard.initAttackInterval) //攻速乘以倍率
        {
            ImpetuousBar.instance.TakeDamage(blackboard.damage * attackDamageMultiplier); //攻击乘以倍率

            blackboard.lastAttackPlayerTime = Time.time;
        }
    }
}

//生成状态
public class MeleeRushAI_Create : IState
{
    private MeleeRushBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public MeleeRushAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeRushBlackboard;
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
            fsm.SwitchState(StateType.Move);  //切换状态
        }
    }
}

//移动状态(朝着玩家走)
public class MeleeRushAI_Move : IState
{
    public MeleeRushBlackboard blackBoard;

    private FSM fsm;
    public MeleeRushAI_Move(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeRushBlackboard;
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
        Vector2 toward = distance.normalized;
        if (distance.sqrMagnitude <= blackBoard.rushDistance * blackBoard.rushDistance)  //与玩家的距离小于冲锋距离
        {
            fsm.SwitchState(StateType.Attack); //切换为冲锋状态
            return;
        }
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed * AiParent.moveSpeedMultiplier; //乘以速度倍率
    }
}

public class MeleeRushAI_Attack : IState
{
    private bool isRush;  //是否进入冲锋状态
    public MeleeRushBlackboard blackBoard;

    private FSM fsm;

    public float timer; //进入Attack状态的时间
    public MeleeRushAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeRushBlackboard;
    }
    public void OnEnter()
    {
        //速度改为0，进入蓄力阶段
        blackBoard.rigidbody2D.velocity = Vector2.zero;

        isRush = false;
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
        if (timer <= blackBoard.rushWaitTime)  //蓄力阶段
        {
            blackBoard.rigidbody2D.velocity = Vector2.zero;
        }
        if (timer > blackBoard.rushWaitTime && timer < blackBoard.rushWaitTime + blackBoard.rushTime) //冲锋阶段
        {
            if (isRush == false) //整个冲锋状态只改变一次速度
            {
                isRush = true;
                Vector2 toward = (PlayerControl.Instance.transform.position - blackBoard.self.transform.position).normalized;
                blackBoard.rigidbody2D.velocity = toward * blackBoard.rushSpeed;
            }
        }
        if (timer >= blackBoard.rushWaitTime + blackBoard.rushTime) //冲锋结束，重新切换状态
        {
            //该单位与玩家的距离
            Vector2 distance = PlayerControl.Instance.transform.position - blackBoard.self.transform.position;

            if (distance.sqrMagnitude <= blackBoard.rushDistance * blackBoard.rushDistance)  //与玩家的距离小于冲锋距离
            {
                fsm.SwitchState(StateType.Attack); //重新进入冲锋状态
            }
            if (distance.sqrMagnitude > blackBoard.rushDistance * blackBoard.rushDistance)   //与玩家的距离大于冲锋距离
            {
                fsm.SwitchState(StateType.Move);   //回到移动状态
            }
        }
    }
}
