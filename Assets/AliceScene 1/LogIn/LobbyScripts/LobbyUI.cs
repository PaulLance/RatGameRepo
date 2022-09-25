using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    [SerializeField] panel friendPanel;
    [SerializeField] panel invitePanel;

  
    public void AddFriends()
    {
        friendPanel.MoveTowards();

    }

    public void CloseFriends()
    {
        friendPanel.MoveBackwards();

    }

    public void InviteFriends()
    {
        invitePanel.MoveTowards();

    }

    public void CloseInvintation()
    {
        invitePanel.MoveBackwards();

    }
}
