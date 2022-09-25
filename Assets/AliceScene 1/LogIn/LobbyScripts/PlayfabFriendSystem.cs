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
        string id = friends.FirstOrDefault(f => f.Username == friendName).FriendPlayFabId;
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
        request.FriendUsername = friendName;
        PlayFabClientAPI.AddFriend(request, OnSuccess, OnFailure);
    }

    private void OnFailure(PlayFabError error)
    {
        Debug.Log(error.GenerateErrorReport());
    }

    private void OnSuccess(AddFriendResult result)
    {
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
