using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Pun.UtilityScripts;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;

public class MultiplayerLevelManager : MonoBehaviourPunCallbacks
{
    public int maxKills = 3;
    public GameObject gameOverPopUp;
    public Text winnerText;
    public GameObject[] players;


    public Transform[] sTransform;
    public Vector3[] spawnPositions;

    void Start()
    {
        spawnPositions[0] = sTransform[0].position;
        spawnPositions[1] = sTransform[1].position;
        spawnPositions[2] = sTransform[2].position;
        spawnPositions[3] = sTransform[3].position;
        PhotonNetwork.Instantiate("Multiplayer Player", spawnPositions[0],Quaternion.identity);
    }
    public override void OnPlayerPropertiesUpdate(Photon.Realtime.Player targetPlayer, Hashtable changedProps)
    {
        if (targetPlayer.GetScore() == maxKills)
        {
            winnerText.text = targetPlayer.NickName;
            gameOverPopUp.SetActive(true);
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

    void getPlayers()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
    }
    public void NewGame()
    {
        
        int playerCount = PhotonNetwork.CountOfPlayers;
        Debug.Log("There are" + playerCount + " players");
        if (playerCount >= 2)
        {
            foreach(GameObject p in players)
            {
                if (p.GetComponent<PhotonView>().IsMine)
                {
                  if (p!= null)
                  {
              //          p.GetComponent<Transform>() = spawnPositions[0];
                  }
                }
                  
            }
            //Photon.Realtime.Player targetPlayer = ;
           // targetPlayer.SetScore(0);
            //targetPlayer.
        }
        
    }
}
