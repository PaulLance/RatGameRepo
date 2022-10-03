using Photon.Chat;
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
    [SerializeField] GameObject statusRound;
    Color onlineColor = new Color(0.3843137f, 0.9490196f, 0.4313726f, 1f);
    Color offlineColor = new Color(0.9490196f, 0.4047498f, 0.3843138f,1f);
    public static Action<string> GetCurrentStatus;


    private void Awake()
    {
        PhotonChatController.OnStatusUpdated += Status;
        PhotonChatFriendController.OnPhotonStatusUpdated += Status;
    }

    private void OnDestroy()
    {
        PhotonChatController.OnStatusUpdated -= Status;
        PhotonChatFriendController.OnPhotonStatusUpdated -= Status;
    }


    void Status(PhotonStatus status)
    {
        if (friendName.text == status.playerName)
        {
            ChangeStatus(status);
        }
    }
    private void ChangeStatus(PhotonStatus status)
    {
        if (status.status == ChatUserStatus.Online)
        {
            statusRound.GetComponent<Image>().color = onlineColor;
        }
        if (status.status == ChatUserStatus.Offline)
        {
            statusRound.GetComponent<Image>().color = offlineColor;

        }
    }

    internal void UpdateInfo(string name)
    { 
        friendName.text = name;
        GetCurrentStatus.Invoke(name);
    }

    
    public void CreateActionsArea()
    {
        ActionsArea[] actionsAreas = FindObjectsOfType<ActionsArea>();

        foreach (ActionsArea area in actionsAreas)
        {
            Destroy(area.gameObject);
        }

        GameObject actionsArea = FindObjectOfType<LobbyUI>().actionsArea;
        GameObject friendPanel = FindObjectOfType<LobbyUI>().friendPanel.gameObject;

        GameObject newActions = Instantiate(actionsArea, friendPanel.transform);
        newActions.GetComponent<ActionsArea>().SetFriendName(friendName.text);
        newActions.GetComponent<RectTransform>().position = GetComponent<RectTransform>().transform.position;
        newActions.GetComponent<RectTransform>().localPosition = new Vector2(0 + newActions.GetComponent<RectTransform>().sizeDelta.x / 2 + friendPanel.GetComponent<RectTransform>().sizeDelta.x / 2, newActions.GetComponent<RectTransform>().localPosition.y);
    }
}
