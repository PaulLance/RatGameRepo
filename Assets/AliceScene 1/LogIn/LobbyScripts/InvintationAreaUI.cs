using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InvintationAreaUI : MonoBehaviour
{
    List<InviteUI> invintations = new List<InviteUI>();
    [SerializeField] GameObject invintationLabel;



    private void Awake()
    {
        LobbyUI.createInvintation += CreateInvintation;
        InviteUI.OnAccept += AcceptInvite;
        InviteUI.OnDecline += DeclineInite;
    }

    private void DeclineInite(InviteUI invintation)
    {
        if (invintations.Contains(invintation))
        {
            invintations.Remove(invintation);
            Destroy(invintation.gameObject);
        }
    }

    private void AcceptInvite(InviteUI invintation)
    {
        if (invintations.Contains(invintation))
        {
            invintations.Remove(invintation);
            Destroy(invintation.gameObject);
        }
    }

    private void CreateInvintation(string senderName, string roomName)
    {
        CheckIfSenderInvited(senderName);
        GameObject newInvintationLabel = Instantiate(invintationLabel, transform.GetChild(0).transform);
        newInvintationLabel.GetComponent<InviteUI>().SetData(senderName, roomName);
        invintations.Add(newInvintationLabel.GetComponent<InviteUI>());
    }

    private void CheckIfSenderInvited(string sendername)
    {
        InviteUI invintation = invintations.FirstOrDefault(f => (f.senderName.Equals(sendername)));
        if (invintation != null)
        {
            Destroy(invintation.gameObject);

        }

    }

    private void OnDestroy()
    {
        LobbyUI.createInvintation -= CreateInvintation;

    }
}
