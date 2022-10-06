using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LobbyUI : MonoBehaviour
{
    public panel friendPanel;
    [SerializeField] panel invitePanel;
    [SerializeField] GameObject friendListArea;
    [SerializeField] GameObject addFriendsArea;
    [SerializeField] GameObject invintationArea;
    [SerializeField] TextMeshProUGUI playerName;
    public GameObject actionsArea;
    public GameObject notification;
    public static Action<string, string> createInvintation;




    private void Awake()
    {
        PhotonChatController.getMassage += GetMessage;
    }

    private void OnDestroy()
    {
        PhotonChatController.getMassage -= GetMessage;
    }

    public void GetMessage(string senderName, string roomName)
    {
        GameObject[] labels = GameObject.FindGameObjectsWithTag("notification");
        foreach (GameObject label in labels)
        {
            Destroy(label.gameObject);
        }
        GameObject lobbyCanvas = FindObjectOfType<LobbyUI>().gameObject;
        GameObject newNotificationLabel = Instantiate(notification, lobbyCanvas.transform);
        newNotificationLabel.GetComponent<RectTransform>().localPosition = new Vector2(lobbyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2 - newNotificationLabel.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
        newNotificationLabel.GetComponentInChildren<Label>().SetLabel($"{senderName} invites you to join the party. Ñheck the invitations.");
        createInvintation.Invoke(senderName, roomName);

        
    }
    public void Start()
    {
        playerName.text = PlayerPrefs.GetString("user");
    }
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

    public void ActiveFriendListArea()
    {
        DestroyActionsArea();
        friendListArea.GetComponent<CanvasGroup>().alpha = 1;
        friendListArea.GetComponent<CanvasGroup>().blocksRaycasts = true;
        addFriendsArea.SetActive(false);
        invintationArea.SetActive(false);

    }

    public void DestroyActionsArea()
    {
        ActionsArea[] actionsAreas = FindObjectsOfType<ActionsArea>();

        foreach (ActionsArea area in actionsAreas)
        {
            Destroy(area.gameObject);
        }
    }

    public void ActiveAddFriendsArea()
    {
        DestroyActionsArea();
        friendListArea.GetComponent<CanvasGroup>().alpha = 0;
        friendListArea.GetComponent<CanvasGroup>().blocksRaycasts = false;
        addFriendsArea.SetActive(true);
        invintationArea.SetActive(false);

    }

    public void ActiveInvintationArea()
    {
        DestroyActionsArea();
        friendListArea.GetComponent<CanvasGroup>().alpha = 0;
        friendListArea.GetComponent<CanvasGroup>().blocksRaycasts = false;
        addFriendsArea.SetActive(false);
        invintationArea.SetActive(true);
    }




}
