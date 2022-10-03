using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;
using System;
using Photon.Realtime;
using System.Linq;

public class MainLoobyManager : MonoBehaviourPunCallbacks
{

    public static MainLoobyManager lobbyManager;
    [SerializeField] GameObject loadingCanvas;
    [SerializeField] GameObject lobbyCanvas;
    [SerializeField] GameObject roomCanvas;
    [SerializeField] GameObject chat;
    public static Action<int[], string[]> updateRoomCanvas;
    public static Action<string> updateRoomCanvas1;
    public static Action clearRoomCanvas;
    public Dictionary<int, string> dict = new Dictionary<int, string>();
    PhotonView photonView;


    private void Awake()
    {
        lobbyManager = this;
        InviteUI.OnAcceptNetwork += InintationAccept;
    }

    private void Start()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
        photonView = GetComponent<PhotonView>();
    }

    private void InintationAccept(string roomName)
    {
        PlayerPrefs.SetString("ROOM", roomName);
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
        }
        else
        {
            if (PhotonNetwork.InLobby)
            {
                JoinPlayerRoom();
            }
        }
    }

    private void JoinPlayerRoom()
    {
        string roomName = PlayerPrefs.GetString("ROOM");
        PlayerPrefs.SetString("ROOM", "");
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 4; i++) {

                if (!dict.ContainsKey(i)) 
                {

                    dict[i] = newPlayer.NickName;
                    break;
                }
            }
           int[] keys=dict.Select(f => f.Key).ToArray();
           string[] values = dict.Select(f => f.Value).ToArray();
           photonView.RPC("UpdateDictionaries",RpcTarget.Others, keys, values);
           UpdateInformation();

        }


    }

    [PunRPC]

    public void UpdateDictionaries(int[] keys, string[] values)
    {
        Dictionary<int, string> newDict = new Dictionary<int, string>();
        for (int i=0; i<keys.Length; i++)
        {
            newDict[keys[i]] = values[i];
        }
        dict = newDict;

    }
    

    private void UpdateInformation()
    {
        int[] keys1=new int[4] { -1, -1, -1, -1 };
        string[] values = new string[4] { "", "", "", "" };
        int m = 0;
        for (int i = 0; i < 4; i++)
        {
            if (dict.ContainsKey(i))
            {
                keys1[m] = i;
                values[m] = dict[i];
            }
            else
            {
                keys1[m] = -1;
                values[m] = "";
            }
            m++;
        }
        updateRoomCanvas.Invoke(keys1,values);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int, string> pair in dict)
            {
                if (pair.Value == otherPlayer.NickName)
                {
                    dict.Remove(pair.Key);
                    break;
                }
            }
            UpdateInformation();
        }
    }

    private void OnDestroy()
    {
        InviteUI.OnAcceptNetwork -= InintationAccept;
    }

    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
        loadingCanvas.SetActive(true);
    }

    public override void OnConnectedToMaster()
    {
        chat.SetActive(true);
        loadingCanvas.SetActive(false);
        lobbyCanvas.SetActive(true);
        PhotonNetwork.JoinLobby();

    }

    public override void OnJoinedLobby()
    {
        FindObjectOfType<PlayfabFriendSystem>().FriendList();
        string roomName = PlayerPrefs.GetString("ROOM");
        if (!string.IsNullOrEmpty(roomName))
        {
            JoinPlayerRoom();
        }
        else
        {
            CreateRoom();
            dict[0] = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        roomOptions.PublishUserId = true;
        string roomName=System.Guid.NewGuid().ToString();
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    public void SoloPlay()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }

    public override void OnLeftRoom()
    {
        clearRoomCanvas.Invoke();
        dict.Clear();

    }

}
