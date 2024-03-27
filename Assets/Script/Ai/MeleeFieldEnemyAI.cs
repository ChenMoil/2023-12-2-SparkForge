using System;
using System.Collections;
using System.Runtime.CompilerServices;
using UnityEngine;

[Serializable]
public class MeleeFieldEnemyBlockBorad : BlockBorad
{
    [Header("盾牌初始血量")]
    public int initFieldHp; //盾牌初始血量
    [Header("初始血量")]
    public int initHp;   //初始血量
    [Header("速度")]
    public float speed;  //速度
    [Header("伤害")]
    public float damage; //伤害

    public float createTime = 1.0f; //生成过渡时间
    [Header("初始攻击间隔")]
    public float initAttackInterval; //初始攻击间隔

    [NonSerialized] public float lastAttackPlayerTime = 0; //近战敌人上次攻击玩家的时间

    [NonSerialized] public GameObject hand;
}

/// <summary>
/// 近战敌人的ai
/// </summary>
public class MeleeFieldEnemyAI : AiParent
{
    //储存数据
    public MeleeFieldEnemyBlockBorad blackboard;
    void Start()
    {
        Init();
    }

    //禁用回到对象池时
    private void OnDisable()
    {
        
    }

    //从对象池启用时
    private void OnEnable()
    {
        //血量回归初始
        HP = blackboard.initHp;

        //初始化盾牌
        blackboard.hand = gameObject.transform.GetChild(0).gameObject;
        blackboard.hand.transform.GetChild(0).gameObject.SetActive(true);
        blackboard.hand.transform.GetChild(0).gameObject.GetComponent<EnemyField>().HP = blackboard.initFieldHp;
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
        Debug.Log(blackboard.hand.name);
        AddState();
        InitState();

        //初始化血量
        HP = blackboard.initHp;
        GameManger.Instance.GetAi.Add(gameObject, this);
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new MeleeFieldEnemyAI_Create(fsm));
        fsm.AddState(StateType.Attack, new MeleeFieldEnemyAI_Attack(fsm));
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
            ImpetuousBar.instance.TakeDamage(blackboard.damage * AiParent.attackDamageMultiplier); //攻击乘以倍率

            blackboard.lastAttackPlayerTime = Time.time;
        }
    }
}

//生成状态
public class MeleeFieldEnemyAI_Create : IState
{
    private MeleeFieldEnemyBlockBorad blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public MeleeFieldEnemyAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeFieldEnemyBlockBorad;
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
public class MeleeFieldEnemyAI_Attack : IState
{
    public MeleeFieldEnemyBlockBorad blackBoard;

    private FSM fsm;
    public MeleeFieldEnemyAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeFieldEnemyBlockBorad;
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
        Vector2 toward = (PlayerControl.Instance.transform.position - blackBoard.self.transform.position).normalized;
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed * AiParent.moveSpeedMultiplier; //乘以速度倍率

        //盾牌朝向玩家
        PlayerControl.LookAt(PlayerControl.Instance.transform.position, blackBoard.hand);
    }
}
