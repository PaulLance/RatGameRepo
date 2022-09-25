using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class friendFieldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI friendName;
    [SerializeField] Transform status;

    internal void UpdateInfo(FriendInfo friendInfo)
    {
        friendName.text = friendInfo.Name;

        Debug.Log(friendInfo.IsOnline);
        if (friendInfo.IsOnline)
        {
            status.GetChild(0).gameObject.SetActive(true);
            status.GetChild(1).gameObject.SetActive(false);

        }
        else
        {
            status.GetChild(1).gameObject.SetActive(true);
            status.GetChild(0).gameObject.SetActive(false);

        }

    }

    public void RemoveFriend() {

        FindObjectOfType<PlayfabFriendSystem>().RemoveFriend(friendName.text);
    }
}
