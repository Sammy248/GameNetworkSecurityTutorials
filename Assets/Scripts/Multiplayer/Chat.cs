using UnityEngine;
using Photon.Chat;
using ExitGames.Client.Photon;
using Photon.Pun;
using UnityEngine.UI;
public class Chat : MonoBehaviour, IChatClientListener
{
    public InputField inputField;
    public Text chatContent;
    public GameObject chatPanel;

    private ChatClient chatClient;
    private string channelName;

    void Start()
    {
        channelName = PhotonNetwork.CurrentRoom.Name;

        var auth = new AuthenticationValues(PhotonNetwork.LocalPlayer.NickName);

        chatClient = new ChatClient(this);
        chatClient.Connect(
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat,
            "1.0",
            auth
        );

        chatPanel.SetActive(false);
    }
    void Update()
    {
        chatClient?.Service();

        if (Input.GetKeyDown(KeyCode.T))
        {
            chatPanel.SetActive(!chatPanel.activeSelf);
            if (chatPanel.activeSelf)
                inputField.ActivateInputField();
        }
    }
    public void SendMessage()
    {
        if (string.IsNullOrWhiteSpace(inputField.text)) return;

        chatClient.PublishMessage(channelName, inputField.text);
        inputField.text = "";
    }
    public void OnConnected()
    {
        Debug.Log("Chat connected");
        chatClient.Subscribe(new string[] { channelName });
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnGetMessages(string channel, string[] senders, object[] messages)
    {
        for (int i = 0; i < messages.Length; i++)
        {
            chatContent.text += $"\n{senders[i]}: {messages[i]}";
        }
    }

    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnDisconnected()
    {
        Debug.Log("Chat disconnected");
    }

    void OnDestroy()
    {
        if (chatClient != null && chatClient.CanChat)
        {
            chatClient.Disconnect();
        }
    }

    
    public void OnPrivateMessage(string sender, object message, string channelName)
    {
    }

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
    }

    public void OnSubscribed(string[] channels, bool[] results)
    {
    }

    public void OnUnsubscribed(string[] channels)
    {
    }

    public void OnUserSubscribed(string channel, string user)
    {
    }

    public void OnUserUnsubscribed(string channel, string user)
    {
    }
}
