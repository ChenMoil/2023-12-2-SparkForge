using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameLoad : MonoBehaviour
{
    public GameObject gameLoadCanvas;

    //ทรฮส
    public static GameLoad instance;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        gameLoadCanvas.SetActive(false);
        if (PlayerPrefs.GetInt("isLoad") == 1)
        {
            gameLoadCanvas.SetActive(true);
        }
        PlayerPrefs.SetInt("isLoad", 0);
    }

    // Update is called once per frame
    void Update()
    {

    }
}
