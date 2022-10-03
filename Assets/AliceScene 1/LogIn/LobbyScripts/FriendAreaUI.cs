using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using Photon.Realtime;

public class FriendAreaUI : MonoBehaviour
{
    [SerializeField] TMP_InputField friendField;
    public static Action<string> addFriend;

    string friendName;
    [SerializeField] Transform listArea;
    [SerializeField] GameObject friendUIObject;



    private void Awake()
    {
        PhotonChatFriendController.OnDisplayFriends += UpdatePlayerList;
    }

    public void CleanList()
    {
        foreach (Transform child in listArea)
        {
            Destroy(child.gameObject);
        }

    }


    private void UpdatePlayerList(List<string> players)
    {
        CleanList();

        foreach (string player in players)
        {
            GameObject friendField=Instantiate(friendUIObject, listArea);
            friendField.GetComponent<friendFieldUI>().UpdateInfo(player); 
        }
    }

    public void AddFriend()
    {
        friendName = friendField.text;
        if (!string.IsNullOrEmpty(friendName)) 
        {
            friendField.text = null;
            addFriend.Invoke(friendName);
        }
    }

    

   
}
