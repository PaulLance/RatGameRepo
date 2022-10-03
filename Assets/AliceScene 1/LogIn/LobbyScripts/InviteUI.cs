using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InviteUI : MonoBehaviour
{
    public static Action<InviteUI> OnAccept;
    public static Action<string> OnAcceptNetwork;
    public static Action<InviteUI> OnDecline;

    public string senderName;
    string roomName;


    public void SetData(string name, string roomName1)
    {
        senderName = name;
        roomName = roomName1;
        GetComponentInChildren<TextMeshProUGUI>().text = senderName;

    }

    public void Accept()
    {
        OnAccept.Invoke(this);
        OnAcceptNetwork.Invoke(roomName);
    }

    public void Decline()
    {
        OnDecline.Invoke(this);
    }

    internal bool Contains(InviteUI invintation)
    {
        throw new NotImplementedException();
    }
}
