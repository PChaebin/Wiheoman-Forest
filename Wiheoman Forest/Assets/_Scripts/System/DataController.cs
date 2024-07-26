using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

[System.Serializable]
public class PlayerData
{
    public bool isDead;
    public string name;
    public int level;
    public int gold;
    public string[] items;
}

public class DataController : MonoBehaviour
{
    [SerializeField] private PlayerData playerData;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
        LoadData();
    }

    public void SaveData()
    {
        string json = JsonUtility.ToJson(playerData, true);
        string path = Application.dataPath + "/Resources/Json Files/PlayerData.json";
        File.WriteAllText(path, json);
    }

    public void LoadData()
    {
        string path = Application.dataPath + "/Resources/Json Files/PlayerData.json";

        if (File.Exists(path))
        {
            string data = File.ReadAllText(path);
            playerData = JsonUtility.FromJson<PlayerData>(data);
        }
        else
        {
            Debug.LogError("Not Found 'PlayerData.json' File.");
        }
    }
}