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
    //public

    void Start()
    {
        int spawnIndex = PhotonNetwork.LocalPlayer.ActorNumber - 1; //give each player a number for spawning
        Vector3[] spawnPositions = new Vector3[4];  // create the 4 spawn positions
        spawnIndex = Mathf.Clamp(spawnIndex, 0, spawnPositions.Length - 1); //keep spawnindex at max spawnpoints
        
        for (int i = 0; i<4; i++)   //assigns each position to where i have placed 4 gameobjects in the scene
        {
            spawnPositions[i] = sTransform[i].position;
        }
        PhotonNetwork.Instantiate("Multiplayer Player", //create player
            spawnPositions[spawnIndex], Quaternion.identity);
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
