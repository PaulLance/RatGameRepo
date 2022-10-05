using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ActionsArea : MonoBehaviour
{
    string friendName;
    public static Action<string> SendInvintation;
    [SerializeField] GameObject sendInvintationLabel;
    [SerializeField] GameObject inviteToPartyButton;
    PhotonStatus status;

    private void Awake()
    {
        MainLoobyManager.updateActionsArea += CheckIfFriendInRoom;
    }

    private void OnDestroy()
    {
        MainLoobyManager.updateActionsArea -= CheckIfFriendInRoom;

    }

    public void SetFriendName(string name)
    {
        friendName = name;

        CheckIfFriendInRoom();

    }

    private void CheckIfFriendInRoom()
    {
        bool shouldActive = true;
        Dictionary<int, Photon.Realtime.Player> players = PhotonNetwork.CurrentRoom.Players;

        string[] curPlayers = players.Select(f => f.Value.NickName).ToArray();

        foreach (string player in curPlayers)
        {
            if (string.Equals(friendName, player))
            {
                inviteToPartyButton.SetActive(false);
                shouldActive = false;
                break;
            }
        }

        if (shouldActive == true && status.status==2)
        {
            inviteToPartyButton.SetActive(true);
        }
    }

    public void CloseActionsArea()
    {
        Destroy(this.gameObject);
    }

    public void RemoveFriend()
    {

        FindObjectOfType<PlayfabFriendSystem>().RemoveFriend(friendName);
        Destroy(this.gameObject);
    }

    public void InviteFriend()
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("notification");
        foreach (GameObject label in labels)
        {
            Destroy(label.gameObject);
        }
        GameObject lobbyCanvas = FindObjectOfType<LobbyUI>().gameObject;
        GameObject newInvintationLabel=Instantiate(sendInvintationLabel, lobbyCanvas.transform);
        newInvintationLabel.GetComponent<RectTransform>().localPosition = new Vector2(lobbyCanvas.GetComponent<RectTransform>().sizeDelta.x/2 - newInvintationLabel.GetComponent<RectTransform>().sizeDelta.x/2, 0);
        newInvintationLabel.GetComponentInChildren<Label>().SetLabel($"Party invite sent to {friendName}");
        SendInvintation.Invoke(friendName);
        Destroy(this.gameObject);
    }

    internal void SetStatus(PhotonStatus newStatus)
    {
        status = newStatus;
        if (newStatus.status == 0)
        {
            inviteToPartyButton.SetActive(false);
        }
        else
        {
            inviteToPartyButton.SetActive(true);

        }
    }
}
