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
    [SerializeField] GameObject invintationLabel;
    public GameObject actionsArea;
    public GameObject notification;



    private void Awake()
    {
        PhotonChatController.getMassage += GetMessage;
    }

    private void OnDestroy()
    {
        PhotonChatController.getMassage -= GetMessage;
    }

    public void GetMessage(string senderName)
    {
        Debug.Log(600);
        GameObject[] labels = GameObject.FindGameObjectsWithTag("notification");
        foreach (GameObject label in labels)
        {
            Destroy(label.gameObject);
        }
        GameObject lobbyCanvas = FindObjectOfType<LobbyUI>().gameObject;
        GameObject newNotificationLabel = Instantiate(notification, lobbyCanvas.transform);
        newNotificationLabel.GetComponent<RectTransform>().localPosition = new Vector2(lobbyCanvas.GetComponent<RectTransform>().sizeDelta.x / 2 - newNotificationLabel.GetComponent<RectTransform>().sizeDelta.x / 2, 0);
        newNotificationLabel.GetComponentInChildren<Label>().SetLabel($"{senderName} invites you to join the party. Ñheck the invitations.");
        GameObject newInvintationLabel=Instantiate(invintationLabel, invintationArea.transform);
        newInvintationLabel.GetComponentInChildren<TextMeshProUGUI>().text = senderName;
        
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
        friendListArea.SetActive(true);
        addFriendsArea.SetActive(false);
        invintationArea.SetActive(false);

    }

    public void ActiveAddFriendsArea()
    {
        friendListArea.SetActive(false);
        addFriendsArea.SetActive(true);
        invintationArea.SetActive(false);

    }

    public void ActiveInvintationArea()
    {
        friendListArea.SetActive(false);
        addFriendsArea.SetActive(false);
        invintationArea.SetActive(true);
    }




}
