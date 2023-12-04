﻿using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    //Player物体控制脚本
    private PlayerInput playerInputControl;

    //玩家的速度
    public float playerSpeed;

    //刚体组件
    private Rigidbody2D playerRigidbody;
    void Start()
    {
        //玩家刚体初始化
        playerRigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        //玩家的移动函数
        PlayerMove();
    }

    private void Awake()
    {
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
    private void PlayerMove()
    {
        //读取输入的数据
        Vector2 playMove = playerInputControl.PlayerControl.Move.ReadValue<Vector2>();
        //将速度赋值给刚体
        playerRigidbody.velocity = playMove * playerSpeed;
    }
}