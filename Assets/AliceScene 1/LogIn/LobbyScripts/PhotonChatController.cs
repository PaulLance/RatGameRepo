using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Chat;
using Photon.Pun;
using ExitGames.Client.Photon;
using System;

public class PhotonChatController : MonoBehaviour, IChatClientListener
{

    [SerializeField] string userID;
    ChatClient chatClient;
    [SerializeField] GameObject notification;
    [SerializeField] GameObject lobby;
    public static Action<string, string> getMassage;
    public static Action<PhotonStatus> OnStatusUpdated;
    public static Action<ChatClient> OnChatConnected;



    void Awake()
    {
        ActionsArea.SendInvintation += SendInvintation;

    }

    void OnDestroy()
    {
        ActionsArea.SendInvintation -= SendInvintation;

    }

    private void SendInvintation(string friendName)
    {
        Debug.Log(PhotonNetwork.CurrentRoom.Name);
        chatClient.SendPrivateMessage(friendName, PhotonNetwork.CurrentRoom.Name);
    }

    void Start()
    {
        chatClient = new ChatClient(this);
        userID = PlayerPrefs.GetString("user");
        //Debug.Log(nickName);
        ConnectToPhotonChat();
      
    }

    void Update()
    {
        chatClient.Service();
    }

    private void ConnectToPhotonChat()
    {
        chatClient.Connect(PhotonNetwork.PhotonServerSettings.AppSettings.AppIdChat, PhotonNetwork.AppVersion,
        new Photon.Chat.AuthenticationValues(userID));
    }

    public void SendDirectMessage(string recepient, string message)
    {
        chatClient.SendPrivateMessage(recepient, message);
    }


    public void DebugReturn(DebugLevel level, string message)
    {
    }

    public void OnChatStateChange(ChatState state)
    {
    }

    public void OnConnected()
    {
        OnChatConnected.Invoke(chatClient);
        chatClient.SetOnlineStatus(ChatUserStatus.Online);
    }

    public void OnDisconnected()
    {
        chatClient.SetOnlineStatus(ChatUserStatus.Offline);
    }

    public void OnGetMessages(string channelName, string[] senders, object[] messages)
    {
    }

    public void OnPrivateMessage(string sender, object message, string channelName)
    {
        if (!string.IsNullOrEmpty(message.ToString()))
        {
            string[] splitNames = channelName.Split(new char[] { ':' });
            string senderName = splitNames[0];
            Debug.Log(senderName);
            Debug.Log(splitNames[1]);
            if (!sender.Equals(senderName, StringComparison.OrdinalIgnoreCase))
            {
                    Debug.Log("MESSAGE");
                    getMassage.Invoke(senderName, message.ToString());
            }
        }
    }  

    public void OnStatusUpdate(string user, int status, bool gotMessage, object message)
    {
        PhotonStatus newStatus = new PhotonStatus(user, status, (string)message);
        OnStatusUpdated?.Invoke(newStatus);
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
