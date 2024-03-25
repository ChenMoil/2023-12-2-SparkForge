using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using static System.Net.Mime.MediaTypeNames;

public class SaveAndLoad : MonoBehaviour
{
    public GameObject canvas;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public Save CreateSave()
    {
        //创建一个Save对象存储当前游戏数据
        Save save = new Save();

        //储存的数据
        save.currentImpetuousBarValue = ImpetuousBar.instance.currentImpetuousBar;
        save.currentTime = ProgressRound.instance.timer;
        save.currentPoints = LevelManager.instance.timer + GameManger.Instance.enemyKill;

        return save;
    }

    //储存数据
    public void SaveBySerialization()
    {
        //获取当前的游戏数据存在Save对象里
        Save save = CreateSave();

        //创建一个二进制形式
        BinaryFormatter moodblast = new BinaryFormatter();

        //储存在文件中
        FileStream fs = File.Create(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast");

        //将Save对象转化为字节
        moodblast.Serialize(fs, save);

        //关闭文件流
        fs.Close();
    }

    //读取数据
    public void LoadByDeserialization()
    {
        //判断文件是否创建
        if (File.Exists(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast"))
        {

            //反序列化并将数据储存至save
            BinaryFormatter moodblast = new BinaryFormatter();
            FileStream fs = File.Open(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast", FileMode.Open);//打开文件
            Save save = moodblast.Deserialize(fs) as Save;

            //关文件流       
            fs.Close();

            //赋值
            ImpetuousBar.instance.currentImpetuousBar = save.currentImpetuousBarValue;
        }
        else
        {
            UnityEngine.Debug.Log("Data Not Found");
        }
    }
}
