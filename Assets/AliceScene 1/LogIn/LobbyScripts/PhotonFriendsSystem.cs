using PlayFab.ClientModels;
using System;
using System.Collections;
using System.Collections.Generic;
using PhotonFriendInfo = Photon.Realtime.FriendInfo;
using PlayFabFriendInfo = PlayFab.ClientModels.FriendInfo;
using UnityEngine;
using Photon.Pun;
using System.Linq;

public class PhotonFriendsSystem : MonoBehaviourPunCallbacks
{

    public static Action<List<PhotonFriendInfo>> friendListUI;

    private void Awake()
    {
        PlayfabFriendSystem.FriendsListInPhoton += ConvertToPhotonSide;
      
    }
    private void ConvertToPhotonSide(List<PlayFabFriendInfo> friends)
    {
        if (friends.Count == 0) { FindObjectOfType<FriendAreaUI>().CleanList(); return; }
        string[] friendNames = friends.Select(f => f.TitleDisplayName).ToArray();
        Debug.Log(friendNames.Length);
        if (friendNames != null)
        {
            PhotonNetwork.FindFriends(friendNames);

        }
    }

    public override void OnFriendListUpdate(List<PhotonFriendInfo> friendList)
    {
       friendListUI.Invoke(friendList);

    }
}
