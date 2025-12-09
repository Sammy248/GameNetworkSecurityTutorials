using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Pun.Demo.PunBasics;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.SocialPlatforms.Impl;
using UnityEngine.UI;
using System;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    public int maxKills = 3;
    public GameObject gameOverPopUp;
    public Text winnerText;
    public Text timerText;
    //public GameObject[] players;

    PhotonView _photonView;

    public Transform[] sTransform;
    public int spawnIndex;
    public Vector3[] spawnPositions;

    float timer;


    Vector3 initialSpawnPos;


    void Start()
    {

        _photonView = GetComponent<PhotonView>();

        spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //give each player a number for spawning
        spawnPositions = new Vector3[4];  // create the 4 spawn positions
        spawnIndex = Mathf.Clamp(spawnIndex, 0, spawnPositions.Length - 1); //keep spawnindex at max spawnpoints

        for (int i = 0; i < 4; i++)   //assigns each position to where i have placed 4 gameobjects in the scene
        {
            spawnPositions[i] = sTransform[i].position;
        }
        PhotonNetwork.Instantiate("Multiplayer Player", //create player
            spawnPositions[spawnIndex], Quaternion.identity);

        //set timer
        timer = 100;
    }
    void Update()
    {
        timer -= Time.deltaTime;
        if (timer>= 0)
        {
            timerText.text = timer.ToString();

        }
        else
        {
            timerText.text = "TIME";
            winnerText.text = "NOBODY";
            gameOverPopUp.SetActive(true);
        }
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == maxKills)
        {            
            winnerText.text = targetPlayer.NickName;
            gameOverPopUp.SetActive(true);
            StorePersonalBest();
        }        
    }
    void StorePersonalBest()
    {
        int currentScore = PhotonNetwork.LocalPlayer.GetScore();
        PlayerData playerData = GameManager.instance.playerData;
        if (currentScore > playerData.bestScore)
        {
            playerData.username = PhotonNetwork.LocalPlayer.NickName;
            playerData.bestScore = currentScore;
            playerData.bestScoreDate = DateTime.UtcNow.ToString();
            playerData.totalPlayersInGame = PhotonNetwork.CurrentRoom.PlayerCount;
            playerData.roomName = PhotonNetwork.CurrentRoom.Name;

            GameManager.instance.SavePlayerData();
        }
    }

    public void LeaveGame()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.Disconnect();
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        SceneManager.LoadScene("Lobby");
    }

    [PunRPC]
    public void NewGame()
    {
;
        if (PhotonNetwork.PlayerList.Length >= 2)
        {
            //photonView.RPC("ResetPlayer", RpcTarget.AllViaServer);
            ResetPlayer();
            timer = 100;
            if (!photonView.IsMine)
            {
                return;
            }
            photonView.RPC("ResetEndscreen", RpcTarget.All);
            //ResetEndscreen();
        }
    }

    [PunRPC]

    public void ResetPlayer()
    {
        //reset playerposition
        foreach(GameObject p in GameObject.FindGameObjectsWithTag("Player"))
        {
            p.transform.position = spawnPositions[PhotonNetwork.LocalPlayer.ActorNumber-1];
            p.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
        }

        // Reset player score
        foreach (var player in PhotonNetwork.PlayerList)
        {
            player.SetScore(0);
        }
    }

    [PunRPC]
    public void ResetEndscreen()
    {
        gameOverPopUp.SetActive(false);

    }
}
