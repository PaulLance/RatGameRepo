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




}
