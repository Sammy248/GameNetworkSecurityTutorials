using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using System.Collections.Generic;

public class MultiplayerLobby : MonoBehaviourPunCallbacks
{
    public Transform LoginPanel;
    public Transform SelectionPanel;
    public Transform CreateRoomPanel;
    public Transform InsideRoomPanel;
    public Transform ListRoomsPanel;

    public Transform ListRoomPanel;
    public Transform roomEntryPrefab;
    public Transform listRoomPanelContent;

    public InputField roomNameInput;

    public GameObject startGameButton;

    public InputField playerNameInput;

    string playerName;

    public GameObject textPrefab;
    public Transform insideRoomPlayerList;

    Dictionary<string, RoomInfo> cachedRoomList;

    private void Start()
    {
        playerNameInput.text = playerName = string.Format("Player {0}", Random.Range(1, 1000000));

        cachedRoomList = new Dictionary<string, RoomInfo>();

        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreateARoom()
    {
        Debug.Log("Created Room?");
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 4;
        roomOptions.IsVisible = true;

        PhotonNetwork.CreateRoom(roomNameInput.text, roomOptions);
    }
    public override void OnCreatedRoom()
    {
        Debug.Log("Room has been created");
    }
    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("RoomCreateFailed" + message);
    }
    public override void OnJoinedRoom()
    {
        Debug.Log("Room has been Joined");
        ActivatePanel("InsideRoom");

        startGameButton.SetActive(PhotonNetwork.IsMasterClient);

        foreach (var player in PhotonNetwork.PlayerList)
        {
            var playerListEntry = Instantiate(textPrefab, insideRoomPlayerList);
            playerListEntry.GetComponent<Text>().text = player.NickName;
            playerListEntry.name = player.NickName;
        }
        PhotonNetwork.Instantiate("Idiot Player", Vector3.zero, Quaternion.identity);
    }
    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }
    public override void OnLeftRoom()
    {
        Debug.Log("Room has been left");
        ActivatePanel("CreateRoom");

        DestroyChildren(InsideRoomPanel);

    }
    public void LoginButtonClicked()
    {
        PhotonNetwork.LocalPlayer.NickName = playerName = playerNameInput.text;
        PhotonNetwork.ConnectUsingSettings();
        ActivatePanel("Selection");
    }

    public void StartGameClicked()
    {
        PhotonNetwork.CurrentRoom.IsOpen = false;
        PhotonNetwork.CurrentRoom.IsVisible = false;
        PhotonNetwork.LoadLevel("Multiplayer");
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("We have connected to the master server");
    }

    public void ActivatePanel(string panelName)
    {
        LoginPanel.gameObject.SetActive(false);
        SelectionPanel.gameObject.SetActive(false);
        CreateRoomPanel.gameObject.SetActive(false);
        InsideRoomPanel.gameObject.SetActive(false);
        ListRoomsPanel.gameObject.SetActive(false);

        if(panelName == LoginPanel.gameObject.name)
        {
            LoginPanel.gameObject.SetActive(true);
        }
        else if (panelName == SelectionPanel.gameObject.name)
        {
            SelectionPanel.gameObject.SetActive(true);
        }
        else if (panelName == CreateRoomPanel.gameObject.name)
        {
            CreateRoomPanel.gameObject.SetActive(true);
        }
        else if (panelName == InsideRoomPanel.gameObject.name)
        {
            InsideRoomPanel.gameObject.SetActive(true);
        }
        else if (panelName == ListRoomsPanel.gameObject.name)
        {
            ListRoomsPanel.gameObject.SetActive(true);
        }
    }

    public void DisconnectButtonClicked()
    {
        Debug.Log("Disconnected from Master Server");
        PhotonNetwork.Disconnect();
    }

    public void DestroyChildren(Transform parent)
    {
        foreach (Transform child in parent)
        {
            Destroy(child.gameObject);
        }
    }

    public void ListRoomsClicked()
    {
        PhotonNetwork.JoinLobby();
    }
    public void OnJoinRandomRoomClicked()
    {
        PhotonNetwork.JoinRandomRoom();
    }
    public override void OnJoinedLobby()
    {
        Debug.Log("Joined Lobby");
        ActivatePanel("ListRooms");
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        Debug.Log("Room Update: " + roomList.Count);

        DestroyChildren(listRoomPanelContent);

        UpdateCachedRoomList(roomList);

        foreach (var room in cachedRoomList)
        {
            var newRoomEntry = Instantiate(roomEntryPrefab, listRoomPanelContent);
            var newRoomEntryScript = newRoomEntry.GetComponent<RoomEntry>();
            newRoomEntryScript.roomName = room.Key;
            newRoomEntryScript.roomText.text = string.Format("[{0} - ({1}/{2})]", room.Key, room.Value.PlayerCount, room.Value.MaxPlayers);
        }
    }

    public void LeaveLobbyClicked()
    {
        PhotonNetwork.LeaveLobby();
    }

    public override void OnLeftLobby()
    {
        Debug.Log("LeftLobby!");
        DestroyChildren(listRoomPanelContent);
        cachedRoomList.Clear();
        ActivatePanel("Selection");
    }

    public void UpdateCachedRoomList(List<RoomInfo> roomList)
    {
        foreach(var room in roomList)
        {
            if (!room.IsOpen || !room.IsVisible || room.RemovedFromList)
            {
                cachedRoomList.Remove(room.Name);
            }
            else
            {
                cachedRoomList[room.Name] = room;
            }
        }
    }

    public override void OnPlayerEnteredRoom(Photon.Realtime.Player newPlayer)
    {
        Debug.Log("Player Joined Room");
        var playerListEntry = Instantiate(textPrefab, insideRoomPlayerList);
        playerListEntry.GetComponent<Text>().text = newPlayer.NickName;
        playerListEntry.name = newPlayer.NickName;
    }

    public override void OnPlayerLeftRoom(Photon.Realtime.Player otherPlayer)
    {
        Debug.Log("Player Left Room");
        foreach (Transform child in insideRoomPlayerList)
        {
            if (child.name == otherPlayer.NickName)
            {
                Destroy(child.gameObject);
                break;
            }
        }
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join Random Room. " + message);
    }
    public override void OnJoinRoomFailed(short returnCode, string message)
    {
        Debug.Log("Failed to join Room. " + message);
    }

    public override void OnMasterClientSwitched(Photon.Realtime.Player newMasterClient)
    {
        startGameButton.SetActive(PhotonNetwork.IsMasterClient);
    }
}
