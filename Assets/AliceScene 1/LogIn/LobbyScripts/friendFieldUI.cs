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
    internal string name;
    [SerializeField] GameObject statusRound;
    Color onlineColor = new Color(0.3843137f, 0.9490196f, 0.4313726f, 1f);
    Color offlineColor = new Color(0.9490196f, 0.4047498f, 0.3843138f,1f);
    public static Action<string> GetCurrentStatus;
    [SerializeField] ActionsArea currentActionArea;
    PhotonStatus newStatus;


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
        Debug.Log(status.status + "S");
        if (friendName.text == status.playerName)
        {
            Debug.Log(gameObject.GetInstanceID());
            ChangeStatus(status);
            if (currentActionArea != null)
            {
                currentActionArea.SetStatus(status);
            }
        }
    }
    private void ChangeStatus(PhotonStatus status)
    {
        Debug.Log(status.status + "M");
        if (status.status == ChatUserStatus.Online)
        {
            statusRound.GetComponent<Image>().color = onlineColor;
        }
        else
        {
            statusRound.GetComponent<Image>().color = offlineColor;

        }
        newStatus = status;
    }

    internal void UpdateInfo(string name1)
    {
        name = name1;
        friendName.text = name1;
        GetCurrentStatus.Invoke(name1);
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
        currentActionArea = newActions.GetComponent<ActionsArea>();
        currentActionArea.SetStatus(newStatus);
        currentActionArea.SetFriendName(friendName.text);
        newActions.GetComponent<RectTransform>().position = GetComponent<RectTransform>().transform.position;
        newActions.GetComponent<RectTransform>().localPosition = new Vector2(0 + newActions.GetComponent<RectTransform>().sizeDelta.x / 2 + friendPanel.GetComponent<RectTransform>().sizeDelta.x / 2, newActions.GetComponent<RectTransform>().localPosition.y);
    }
}
