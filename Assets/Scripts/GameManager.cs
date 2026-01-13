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
        string serializedData = JSON.Serialize(playerData).CreateString(); //Serialization is the process of converting an object or data structure
                                                                           //into a format that can be easily stored or transmitted
        string encryptedData = AesEncryption.Encrypt(serializedData);   //sends data to be encrypted

        File.WriteAllText(filePath, encryptedData);
    }

    public void LoadPlayerData()
    {
        if (!File.Exists(filePath))
        {
            playerData = new PlayerData();
            SavePlayerData();
            return;
        }

        string encryptedFileContents = File.ReadAllText(filePath);
        string decryptedJson = AesEncryption.Decrypt(encryptedFileContents);

        playerData = JSON.ParseString(decryptedJson).Deserialize<PlayerData>();
    }


}

