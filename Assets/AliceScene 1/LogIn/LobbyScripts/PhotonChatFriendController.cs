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
        OnPhotonStatusUpdated.Invoke(status);
    }

    private void ChatConnected(ChatClient client)
    {
        chatClient = client;
        RemovePhotonFriends();
        FindPhotonFriends();
    }

    void OnDestroy()
    {
        PlayfabFriendSystem.FriendsListInPhoton -= FriendsUpdated;
        PhotonChatController.OnStatusUpdated -= StatusUpdated;
        PhotonChatController.OnChatConnected -= ChatConnected;
        friendFieldUI.GetCurrentStatus -= CurrentStatus;
    }

    private void StatusUpdated(PhotonStatus status)
    {
        if (friendStatuses.ContainsKey(status.playerName))
        {
            friendStatuses[status.playerName] = status;
        }
        else
        {
            friendStatuses.Add(status.playerName, status);
        }
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
