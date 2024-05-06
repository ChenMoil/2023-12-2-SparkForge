using System;
using UnityEngine;

[Serializable]
public class ArrogantBossBlackboard : BlockBorad
{
    public float createTime = 1.0f; //生成过渡时间
    [Header("速度")]
    public int speed;  //速度
    [Header("技能一和技能二切换距离")]
    public int maxDistance;  

    [Header("技能一")]
    [Header("伤害1伤害")]
    public int skillOneDamage; //伤害
    [Header("攻击间隔")]
    public float attackInterval; //攻击间隔
    [Header("发射的子弹速度")]
    public float BulletSpeed; //发射的子弹速度
    public GameObject[] Bullet; //发射的子弹列表

    [Header("技能二")]
    [Header("伤害2伤害")]
    public int skillTwoDamage; //伤害
    [Header("技能二冷却时间")]
    public float skillTwoCooldown; //攻击间隔
    [Header("震荡时长")]
    public float concussionTime;
    [Header("震荡速度比例( <1 )")]
    public float concussionSpeed;
    [Header("施法时间")]
    public float castTime;
    [Header("施法时的速度")]
    public float castSpeed;
    [Header("技能范围")]
    public float skillTwoScale;

    //爆炸子物体
    public GameObject bombGameObject;
    [NonSerialized]public GameObject handParent;
    [NonSerialized]public bool isSkillTwo; //是否可以释放第二个技能
    [NonSerialized] public Animator animator;
    [NonSerialized] public int BossID;
}

/// <summary>
/// 傲慢BOSS
/// </summary>
public class ArrogantBoss : AiParent
{
    //储存数据
    public ArrogantBossBlackboard blackboard;

    void Start()
    {
        Init();
        blackboard.handParent = transform.GetChild(0).gameObject;
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
        blackboard.animator = GetComponent<Animator>();

        //初始化血量
        HP = initHp;
        GameManger.Instance.GetAi.Add(gameObject, this);
    }

    //向FSM中添加状态
    private void AddState()
    {
        fsm.AddState(StateType.Create, new ArrogantBossAI_Create(fsm));
        fsm.AddState(StateType.Attack, new ArrogantBossAI_Attack(fsm));
        fsm.AddState(StateType.SkillTwo, new ArrogantBossAI_SkillTwo(fsm));
        fsm.AddState(StateType.Dead, new ArrogantBossAI_Dead(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }

    public override void TakeDamege(int damege)
    {
        //生成粒子效果
        ParticleManger.instance.ShowParticle(0, this.gameObject);
        HP -= damege;
        //怪物受伤 颜色0
        PopupText.Create(transform.position, damege, 0);
        if (HP <= 0)
        {
            fsm.SwitchState(StateType.Dead);
        }
    }
}

//生成状态
public class ArrogantBossAI_Create : IState
{
    private ArrogantBossBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public ArrogantBossAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as ArrogantBossBlackboard;
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

//技能一攻击状态
public class ArrogantBossAI_Attack : IState
{
    public ArrogantBossBlackboard blackBoard;
    public bool isIdle = true;
    private FSM fsm;

    public float skillTwoTimer; //技能二冷却时间计时器
    public float attackTimer; //发射子弹计时器
    public float idleTimer;
    public float idleTime = 2;
    public float moveTimer;
    public float moveTime = 2;
    private Vector2 toward;
    public ArrogantBossAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as ArrogantBossBlackboard;
    }
    public void OnEnter()
    {
        blackBoard.animator.SetInteger("CurState", 1);
        blackBoard.isSkillTwo = false;
        skillTwoTimer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        //计算技能二冷却
        skillTwoTimer += Time.deltaTime;
        if (skillTwoTimer > blackBoard.skillTwoCooldown)
        {
            blackBoard.isSkillTwo = true;
            skillTwoTimer = 0;
        }
    }

    public void OnUpdate()
    {
        if (isIdle)
        {
            toward = new Vector2(0, 0);
            idleTimer += Time.deltaTime;
            if (idleTimer > idleTime)
            {
                idleTimer = 0;
                isIdle = false;
                float x = UnityEngine.Random.Range(-1f, 1f);
                float y = UnityEngine.Random.Range(-1f, 1f);
                toward = new Vector2(x, y).normalized;
            }
        }
        else
        {
            moveTimer += Time.deltaTime;
            if (moveTimer > moveTime)
            {
                moveTimer = 0;
                isIdle = true;
                toward = Vector2.zero;
            }
        }

        //该单位与玩家的距离
        Vector2 distance = PlayerControl.Instance.transform.position - blackBoard.self.transform.position;
        if (distance.sqrMagnitude < blackBoard.maxDistance * blackBoard.maxDistance) //进入释放技能范围
        {
            if (blackBoard.isSkillTwo == true) //可以释放技能
            {
                //释放技能2
                fsm.SwitchState(StateType.SkillTwo);
            }
        }
        blackBoard.rigidbody2D.velocity = toward * blackBoard.speed * AiParent.moveSpeedMultiplier; //速度乘以倍率


        //攻击部分
        //让手对着玩家
        PlayerControl.LookAt(PlayerControl.Instance.gameObject.transform.position, blackBoard.handParent);
        attackTimer += Time.deltaTime;
        if (attackTimer > blackBoard.attackInterval * AiParent.attackSpeedMultiplier)
        {
            attackTimer = 0;

            //生成子弹
            int temp = UnityEngine.Random.Range(0, blackBoard.Bullet.Length);
            GameObject newBullet = ObjectPool.Instance.RequestCacheGameObejct(blackBoard.Bullet[temp]);
            //改变位置
            newBullet.transform.position = blackBoard.handParent.transform.GetChild(0).position;
            //改变子弹伤害
            newBullet.GetComponent<EnemyBullet>().damage = blackBoard.skillOneDamage;
            //改变速度
            Vector2 towards = ((Vector2)PlayerControl.Instance.gameObject.transform.position - (Vector2)blackBoard.handParent.transform.position).normalized;
            newBullet.GetComponent<Rigidbody2D>().velocity = blackBoard.BulletSpeed * towards;

        }
    }
}

//技能二状态
public class ArrogantBossAI_SkillTwo : IState
{
    public ArrogantBossBlackboard blackBoard;

    private FSM fsm;

    public float timer; //技能二计时器
    public ArrogantBossAI_SkillTwo(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as ArrogantBossBlackboard;
    }
    public void OnEnter()
    {
        blackBoard.animator.SetInteger("CurState", 2);
        timer = 0;
    }

    public void OnExit()
    {

    }

    public void OnFixedUpdate()
    {
        timer += Time.deltaTime;
        if (timer > blackBoard.castTime) //施法完毕
        {
            //生成爆炸
            GameObject bomb = GameObject.Instantiate(blackBoard.bombGameObject);
            bomb.transform.position = blackBoard.self.transform.position;
            bomb.GetComponent<Bomb>().damage = blackBoard.skillTwoDamage;
            bomb.GetComponent<Bomb>().scale = blackBoard.skillTwoScale;


            fsm.SwitchState(StateType.Attack);
        }
    }

    public void OnUpdate()
    {
        //该单位与玩家的距离
        Vector2 distance = PlayerControl.Instance.transform.position - blackBoard.self.transform.position;
        Vector2 toward = distance.normalized; //移动的方向
        blackBoard.rigidbody2D.velocity = toward * blackBoard.castSpeed * AiParent.moveSpeedMultiplier; //速度乘以倍率

    }
}

public class ArrogantBossAI_Dead : IState
{
    private ArrogantBossBlackboard blackBoard;

    private FSM fsm;
    private float timer; //计时器
    private float deadTime = 0.75f;
    public ArrogantBossAI_Dead(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as ArrogantBossBlackboard;
    }
    public void OnEnter()
    {
        dialogue.instance.BossDeadSign(2f, 7.5f);
        blackBoard.animator.SetInteger("CurState", 3);
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
        this.blackBoard.rigidbody2D.velocity = Vector2.zero;
        timer += Time.deltaTime;
        if (timer > deadTime)
        {
            fsm.SwitchState(StateType.Create);  //切换状态
            ObjectPool.Instance.ReturnCacheGameObject(blackBoard.self);
        }
    }
}
