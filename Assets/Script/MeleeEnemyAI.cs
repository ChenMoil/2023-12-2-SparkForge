﻿using System;
using System.Collections;
using UnityEngine;

[Serializable]
public class MeleeEnemyBlackboard : BlockBorad
{
    [NonSerialized]public GameObject self; //物体自己

    public float speed;  //速度

    public float damage; //伤害

    public float createTime = 1.0f; //生成过渡时间
}

/// <summary>
/// 近战敌人的ai
/// </summary>
public class MeleeEnemyAI : MonoBehaviour
{
    public FSM fsm;
  
    //储存数据
    public MeleeEnemyBlackboard blackboard;
    
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
        fsm.AddState(StateType.Create, new MeeleAI_Create(fsm));
        fsm.AddState(StateType.Attack, new MeeleAI_Attack(fsm));
    }

    //切换初始状态
    private void InitState()
    {
        fsm.SwitchState(StateType.Create);
    }
}

//生成状态
public class MeeleAI_Create : IState
{
    private MeleeEnemyBlackboard blackBoard;

    private FSM fsm;

    private float timer; //计时器
    public MeeleAI_Create(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeEnemyBlackboard;
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
public class MeeleAI_Attack : IState
{
    public MeleeEnemyBlackboard blackBoard;

    private FSM fsm;
    public MeeleAI_Attack(FSM fsm)
    {
        this.fsm = fsm;
        this.blackBoard = fsm.blockBorad as MeleeEnemyBlackboard;
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
        blackBoard.self.GetComponent<Rigidbody2D>().velocity = toward * blackBoard.speed;
    }
}