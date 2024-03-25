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
        //����һ��Save����洢��ǰ��Ϸ����
        Save save = new Save();

        //���������
        save.currentImpetuousBarValue = ImpetuousBar.instance.currentImpetuousBar;
        save.currentTime = ProgressRound.instance.timer;
        save.currentPoints = LevelManager.instance.timer + GameManger.Instance.enemyKill;

        return save;
    }

    //��������
    public void SaveBySerialization()
    {
        //��ȡ��ǰ����Ϸ���ݴ���Save������
        Save save = CreateSave();

        //����һ����������ʽ
        BinaryFormatter moodblast = new BinaryFormatter();

        //�������ļ���
        FileStream fs = File.Create(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast");

        //��Save����ת��Ϊ�ֽ�
        moodblast.Serialize(fs, save);

        //�ر��ļ���
        fs.Close();
    }

    //��ȡ����
    public void LoadByDeserialization()
    {
        //�ж��ļ��Ƿ񴴽�
        if (File.Exists(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast"))
        {

            //�����л��������ݴ�����save
            BinaryFormatter moodblast = new BinaryFormatter();
            FileStream fs = File.Open(UnityEngine.Application.persistentDataPath + "/Data.MoodBlast", FileMode.Open);//���ļ�
            Save save = moodblast.Deserialize(fs) as Save;

            //���ļ���       
            fs.Close();

            //��ֵ
            ImpetuousBar.instance.currentImpetuousBar = save.currentImpetuousBarValue;
        }
        else
        {
            UnityEngine.Debug.Log("Data Not Found");
        }
    }
}
