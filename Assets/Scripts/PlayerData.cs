using UnityEngine;
using System;

public class PlayerData : MonoBehaviour
{

    public string uid;
    public string username;
    public int bestScore;
    public int quickestTime = int.MaxValue;
    public string bestScoreDate;
    public int totalPlayersInGame;
    public string roomName;
    public PlayerData()
    {
        uid = Guid.NewGuid().ToString();
    }

    void Start()
    {
        
    }

    void Update()
    {
        
    }
}
