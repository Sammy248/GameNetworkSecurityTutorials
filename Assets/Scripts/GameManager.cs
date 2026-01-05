using Leguar.TotalJSON;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.IO;
using System.Text;
using System.Security;
using System.Security.Cryptography;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public PlayerData playerData;
    public string filePath;
    public GlobalLeaderboard globalLeaderboard;


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
        LoginWithCustomIDRequest request = new LoginWithCustomIDRequest()
        {
            CreateAccount = true,
            CustomId = playerData.uid,
        };
        PlayFabClientAPI.LoginWithCustomID(request, PlayFabLoginResult, PlayFabLoginError);
    }
    void PlayFabLoginResult(LoginResult loginResult)
    {
        Debug.Log("PlayFab - Login Succeeded: " + loginResult.ToJson());
    }
    void PlayFabLoginError(PlayFabError loginError)
    {
        Debug.Log("PlayFab - Login failed: " + loginError.ErrorMessage);

    }

    public void SavePlayerData()
    {
        string serialisedDataString = JSON.Serialize(playerData).CreateString();

        File.WriteAllText(filePath, Base64Encode(serialisedDataString));  //encode data to base 64 
    }
    public void LoadPlayerData()
    {
        if (!File.Exists(filePath))
        {
            playerData = new PlayerData();
            SavePlayerData();
        }
        string fileContents = File.ReadAllText(filePath);
        string decodedFileContents = Base64Decode(fileContents);
        playerData = JSON.ParseString(decodedFileContents).Deserialize<PlayerData>();
    }

    public static string Base64Encode(string plainText) //encode data
    {
        var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
        return Convert.ToBase64String(plainTextBytes);
    }

    public static string Base64Decode(string base64EncodedData) //decode data
    {
        var base64EncodedBytes = Convert.FromBase64String(base64EncodedData);
        return Encoding.UTF8.GetString(base64EncodedBytes);
    }

}