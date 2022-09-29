using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;


public class MainLoobyManager : MonoBehaviourPunCallbacks
{

    public static MainLoobyManager lobbyManager;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] GameObject lobbyCanvas;
    [SerializeField] GameObject chat;


    private void Awake()
    {
        lobbyManager = this;
    }
    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        loadingCanvas.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        loadingCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
        chat.SetActive(true);
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        FindObjectOfType<PlayfabFriendSystem>().FriendList();
    }

    public void SoloPlay()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    //public void 
//



}
