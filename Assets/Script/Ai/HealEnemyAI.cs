using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class HealEnemyBlackboard : BlockBorad
{
    [Header("速度")]
    public float speed;  //速度
    [Header("治疗量")]
    public int heal; //治疗量
    [Header("治疗范围")]
    public float healRange; //治疗范围

    public float createTime = 1.0f; //生成过渡时间
    [Header("治疗间隔")]
    public float initHealInterval; //初始攻击间隔
    [Header("移动无敌时长")]
    public float MoveTime;
    [Header("治疗时长")]
    public float HealTime;
    [Header("距离玩家最大距离")]
    public float maxDistance;
    [NonSerialized] public float lastAttackPlayerTime = 0; //近战敌人上次攻击玩家的时间

    [Header("反弹伤害比例")]
    public float reboundProportion;

    //需要治疗的ai列表
    [NonSerialized]
    public List<AiParent> healAIList = new List<AiParent>();
}

/// <summary>
/// 近战敌人的ai
/// </summary>
public class HealEnemyAI : AiParent
{
    //储存数据
    public HealEnemyBlackboard blackboard;

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
        //更改治疗范围
        gameObject.transform.GetChild(0).transform.localScale = new Vector2(blackboard.healRange, blackboard.healRange);
        GameManger.Instance.GetAi.Add(gameObject, this);
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new HealEnemyAI_Create(fsm));
        fsm.AddState(StateType.Move, new HealEnemyAI_Move(fsm));
        fsm.AddState(StateType.Heal, new HealEnemyAI_Heal(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && !blackboard.healAIList.Contains(collision.gameObject.GetComponent<AiParent>()))
        {
            blackboard.healAIList.Add(collision.gameObject.GetComponent<AiParent>());
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Enemy" && blackboard.healAIList.Contains(collision.gameObject.GetComponent<AiParent>()))
        {
            blackboard.healAIList.Remove(collision.gameObject.GetComponent<AiParent>());
        }
    }
}

//生成状态
public class HealEnemyAI_Create : IState
{
    private HealEnemyBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public HealEnemyAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as HealEnemyBlackboard;
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

//移动无敌状态
public class HealEnemyAI_Move : IState
{
    public HealEnemyBlackboard blackBoard;

    private FSM fsm;
    public float timer;
    public HealEnemyAI_Move(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as HealEnemyBlackboard;
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
        if (timer > blackBoard.MoveTime)
        {
            fsm.SwitchState(StateType.Heal);  //切换状态
        }
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
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed * AiParent.moveSpeedMultiplier; //速度乘以倍率
    }
}

//治疗状态
public class HealEnemyAI_Heal : IState
{
    public HealEnemyBlackboard blackBoard;

    private FSM fsm;
    //切换状态Timer
    public float timer;
    //治疗计时器
    public float healTimer;
    public HealEnemyAI_Heal(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as HealEnemyBlackboard;
    }
    public void OnEnter()
    {
        timer = 0;
        healTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > blackBoard.MoveTime)
        {
            fsm.SwitchState(StateType.Move);  //切换状态
        }

        healTimer += Time.deltaTime;
        if (healTimer > blackBoard.initHealInterval)
        {
            healTimer = 0;
            foreach(AiParent aiParent in blackBoard.healAIList)
            {
                if (aiParent != null)
                {
                    aiParent.TakeHeal(blackBoard.heal);
                }
            }
        }
    }

    public void OnUpdate()
    {
        //该单位与玩家的距离
        Vector2 distance = PlayerControl.Instance.transform.position - blackBoard.self.transform.position;
        Vector2 toward = distance.normalized; //移动的方向
        if (distance.sqrMagnitude > blackBoard.maxDistance * blackBoard.maxDistance * 1.05f)
        {
            toward *= 0;
        }
        else if (distance.sqrMagnitude > blackBoard.maxDistance * blackBoard.maxDistance * 0.95f)
        {
            toward *= 0;
        }
        else { toward *= -1; }
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed * AiParent.moveSpeedMultiplier; //速度乘以倍率
    }
}
