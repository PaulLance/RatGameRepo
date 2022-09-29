using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using System.Linq;

public class PlayfabFriendSystem : MonoBehaviour
{
    public static Action<List<FriendInfo>> FriendsListInPhoton;
    public static Action<string> FriendUIReport;
    List<FriendInfo> friends;
    private void Awake()
    {
        friends = new List<FriendInfo>();
        FriendAreaUI.addFriend += TryToAddFriend;

    }

    private void OnDestroy()
    {
        FriendAreaUI.addFriend -= TryToAddFriend;


    }

    public void RemoveFriend(string friendName)
    {
        string id = friends.FirstOrDefault(f => f.TitleDisplayName == friendName).FriendPlayFabId;
        var request = new RemoveFriendRequest();
        request.FriendPlayFabId = id;
        PlayFabClientAPI.RemoveFriend(request, OnFriendRemoveSuccess, OnFriendRemoveFailure);
    }

    private void OnFriendRemoveFailure(PlayFabError result)
    {
        Debug.Log(result.GenerateErrorReport());
    }

    private void OnFriendRemoveSuccess(RemoveFriendResult result)
    {
        FriendList();
    }

    private void TryToAddFriend(string friendName)
    {
        var request = new AddFriendRequest();
        request.FriendTitleDisplayName = friendName;
        PlayFabClientAPI.AddFriend(request, OnSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        FriendUIReport?.Invoke(error.ErrorMessage);
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnSuccess(AddFriendResult result)
    {
        FriendUIReport?.Invoke("Friend was added successfully. Check your friends list.");
        FriendList();
    }

    public void FriendList()
    {
        var request = new GetFriendsListRequest();
        PlayFabClientAPI.GetFriendsList(request, OnListSuccess, OnListFailure);

    }

    private void OnListFailure(PlayFabError error)
    {
        throw new NotImplementedException();
    }

    private void OnListSuccess(GetFriendsListResult result)
    {
        friends = result.Friends;
        List<FriendInfo> friendList = result.Friends;
        FriendsListInPhoton.Invoke(friendList);
    }
}
