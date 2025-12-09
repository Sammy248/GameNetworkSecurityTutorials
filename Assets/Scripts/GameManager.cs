using UnityEngine;
using System.IO;
using Leguar.TotalJSON;
using PlayFab;
using PlayFab.ClientModels;

public class GameManager : MonoBehaviour
{
        public static GameManager instance;
    public PlayerData playerData;
    public string filePath;
    private void Start()
    {
        LoadPlayerData();
        LoginToPlayFab();
    }
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(this);
        }
    }
    void LoginToPlayFab()
    {
        PlayFabAddonAPI.LoginWithCustomID();
    }
    public void SavePlayerData()
    {
        string serialisedDataString = JSON.Serialize(playerData).CreateString();
        //JSON.Serialize(playerData);
        File.WriteAllText(filePath, serialisedDataString);
    }
    public void LoadPlayerData()
    {
        if (!File.Exists(filePath))
        {
            playerData = new PlayerData();
            SavePlayerData();
        }
        string fileContents = File.ReadAllText(filePath);
        playerData = JSON.ParseString(fileContents).Deserialize<PlayerData>();
    }
}
