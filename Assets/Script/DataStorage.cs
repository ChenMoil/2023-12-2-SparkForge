using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DataStorage : MonoBehaviour
{
    public static DataStorage instance;

    public float obtainPoints;
    public float survivalTime;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        } 
    }
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }
    
    // Update is called once per frame
    void Update()
    {
        if (LevelManager.instance != null)
        {
            survivalTime = LevelManager.instance.realTimer;
            obtainPoints = LevelManager.instance.timer + GameManger.Instance.enemyKill;
        }
    }
}
