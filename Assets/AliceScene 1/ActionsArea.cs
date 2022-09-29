using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionsArea : MonoBehaviour
{

    string friendName;
    public static Action<string> SendInvintation;
    [SerializeField] GameObject sendInvintationLabel;
   


    public void SetFriendName(string name)
    {
        friendName = name;
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


}
