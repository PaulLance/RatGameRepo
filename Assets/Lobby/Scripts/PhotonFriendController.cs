using Photon.Pun;
using Photon.Realtime;
using PlayfabFriendInfo=PlayFab.ClientModels.FriendInfo;
using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.Linq;

public class PhotonFriendController : MonoBehaviourPunCallbacks
{
    public static Action<List<PhotonFriendInfo>> FriendListUpdated;
    private void Awake()
    {
        PlayfabFriendController.OnFriendListUpdated += UpdateFriendsHandler;
    }

    private void UpdateFriendsHandler(List<PlayfabFriendInfo> friends)
    {
        if (friends.Count!=0)
        {
            string[] friendsName = friends.Select(f => f.TitleDisplayName).ToArray();
            PhotonNetwork.FindFriends(friendsName);
        }
    }

    private void OnDestroy(){

        PlayfabFriendController.OnFriendListUpdated -= UpdateFriendsHandler;

    }

    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
        FriendListUpdated?.Invoke(friendList);
    }

}
