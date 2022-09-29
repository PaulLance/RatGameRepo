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
    public static Action<string> getMassage;


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
        chatClient.SendPrivateMessage(friendName, "Invintation");
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
        Debug.Log("connecting to phonon chat");
        Debug.Log(PhotonNetwork.NickName);
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
        Debug.Log("ChatSuccess");
        SendDirectMessage("SmallRat", "Hi, rat!");
    }

    public void OnDisconnected()
    {
        Debug.Log(56);
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
                Debug.Log(message.ToString());
                if (message.ToString() == "Invintation")
                {
                    Debug.Log("MESSAGE");
                    getMassage.Invoke(senderName);
                }
            }
        }
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
