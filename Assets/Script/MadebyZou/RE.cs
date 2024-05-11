using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RE : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        PlayerPrefs.SetInt("decreaseImpetuousLevel", 0);
        PlayerPrefs.SetInt("shieldLevel", 0);
        PlayerPrefs.SetInt("moveSpeedLevel", 0);
        PlayerPrefs.SetInt("fullScreenDamageLevel", 0);
        PlayerPrefs.SetInt("calmdownSkillLevel", 0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
