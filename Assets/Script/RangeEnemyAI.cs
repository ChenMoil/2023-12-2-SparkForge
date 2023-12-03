using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class RangeEnemyBlackboard : BlockBorad
{
    [NonSerialized] public GameObject self; //物体自己

    public float speed;  //速度

    public float damage; //伤害

    public float createTime = 1.0f; //生成过渡时间

    public int maxDistance;  //与玩家的最大距离(超过这个距离会逃跑)
}

/// <summary>
/// 远程躲闪型敌人的ai
/// </summary>
public class RangeEnemyAI : MonoBehaviour
{
    public FSM fsm;

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
        AddState();
        InitState();
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
        float timer = 0;
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
        blackBoard.self.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, timer / blackBoard.createTime);
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
        Vector2 toward = (GameManger.Instance.playerGameObject.transform.position - blackBoard.self.transform.position).normalized;
        if ((GameManger.Instance.playerGameObject.transform.position - blackBoard.self.transform.position).magnitude < blackBoard.maxDistance)
        {
            toward *= -1;
        }
        blackBoard.self.GetComponent<Rigidbody2D>().velocity = toward * blackBoard.speed;
    }
}