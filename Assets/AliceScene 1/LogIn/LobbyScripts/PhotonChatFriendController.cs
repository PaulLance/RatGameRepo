using Photon.Chat;
using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PhotonChatFriendController : MonoBehaviour
{
    bool initialized;
    List<string> friendList = new List<string>();
    ChatClient chatClient;
    public static Dictionary<string, PhotonStatus> friendStatuses = new Dictionary<string, PhotonStatus>();
    public static Action<List<string>> OnDisplayFriends;
    public static Action<PhotonStatus> OnPhotonStatusUpdated;
    public static Action<string[], PhotonStatus[]> forcedStatusUpdate;



    private void Awake()
    {
        PlayfabFriendSystem.FriendsListInPhoton += FriendsUpdated;
        PhotonChatController.OnChatConnected += ChatConnected;
        PhotonChatController.OnStatusUpdated += StatusUpdated;
        friendFieldUI.GetCurrentStatus += CurrentStatus;

    }

    private void CurrentStatus(string friendName)
    {
      
        PhotonStatus status;
        if (friendStatuses.ContainsKey(friendName))
        {
            status = friendStatuses[friendName];
        }
        else
        {
            status = new PhotonStatus(friendName, 0, "");
        }
        Debug.Log(status.status);
        OnPhotonStatusUpdated?.Invoke(status);
    }

    private void ChatConnected(ChatClient client)
    {
        chatClient = client;
        RemovePhotonFriends();
        FindPhotonFriends();
    }

    void OnDestroy()
    {
        PhotonChatController.OnStatusUpdated -= StatusUpdated;
        PhotonChatController.OnChatConnected -= ChatConnected;
        friendFieldUI.GetCurrentStatus -= CurrentStatus;
        PlayfabFriendSystem.FriendsListInPhoton -= FriendsUpdated;

    }

    private void StatusUpdated(PhotonStatus status)
    {
        Debug.Log("Status" + status.playerName);
        if (friendStatuses.ContainsKey(status.playerName))
        {
            friendStatuses[status.playerName] = status;
        }
        else
        {
            friendStatuses.Add(status.playerName, status);
        }
        //foreach (KeyValuePair<string,PhotonStatus> status1 in friendStatuses)
        //{
        //    Debug.Log(status1.Key);
        //    Debug.Log(status1.Value.status);
        //}
        string[] playerNames = friendStatuses.Select(f => f.Key).ToArray();
        PhotonStatus[] statuses = friendStatuses.Select(f => f.Value).ToArray();


    }

    private void FriendsUpdated(List<FriendInfo> friends)
    {
        friendList = friends.Select(f=>f.TitleDisplayName).ToList();
        RemovePhotonFriends();
        FindPhotonFriends();
    }

    private void RemovePhotonFriends()
    {
        if (friendList.Count!=0 && initialized)
        {
            string[] friendsArray = friendList.ToArray();
            chatClient.RemoveFriends(friendsArray);
        }
    }

    private void FindPhotonFriends()
    {
        Debug.Log(19);
        if (chatClient==null) { return; }
        if (friendList.Count != 0)
        {
            initialized = true;
            string[] friendsArray = friendList.ToArray();
            chatClient.AddFriends(friendsArray);
        }
        OnDisplayFriends.Invoke(friendList);
    }
}
