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
    public static Action updateActionsArea;
    public Dictionary<int, string> dict = new Dictionary<int, string>();
    PhotonView photonView;
    Dictionary<string, playerData> playerdict = new Dictionary<string, playerData>();
    playerData data;


    private void Awake()
    {
        lobbyManager = this;
        InviteUI.OnAcceptNetwork += InintationAccept;
        Roles.changeRole += ChangeRoles;
        photonView = GetComponent<PhotonView>();
        for (int i = 0; i < 4; i++)
        {
            dict.Add(i, "");
        }
    }

    private void ChangeRoles(int oldNumber, int newNumber)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            int[] keys = dict.Select(f => f.Key).ToArray();
            string[] values = dict.Select(f => f.Value).ToArray();

            int p = 0;
            int m = 0;

            for (int i = 0; i < keys.Length; i++)
            {
                if (keys[i] == oldNumber)
                {
                    p = i;
                }

                if (keys[i] == newNumber)
                {
                    m = i;
                }
            }
            keys[p] = newNumber;
            keys[m] = oldNumber;

            Dictionary<int, string> newDict = new Dictionary<int, string>();

            playerData[] datas = FindObjectsOfType<playerData>();
            Debug.Log(datas.Length + "length");
            for (int i = 0; i < keys.Length; i++)
            {
                newDict.Add(keys[i], values[i]);
            }
            dict = newDict;
            UpdateRoomDict();
            Debug.Log(values[0] + "VALUE");


        }

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
        Debug.Log(80);
        string roomName = PlayerPrefs.GetString("ROOM");
        PlayerPrefs.SetString("ROOM", "");
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {

        if (PhotonNetwork.IsMasterClient)
        {
            for (int i = 0; i < 4; i++)
            {

                if (string.IsNullOrEmpty(dict[i]))
                {
                    Debug.Log(i + "key");
                    Debug.Log(dict.Count);
                    dict[i] = newPlayer.NickName;
                    break;
                }
            }
            UpdateRoomDict();
            updateActionsArea?.Invoke();
        }


    }

    private void UpdateRoomDict()
    {
        int[] keys = dict.Select(f => f.Key).ToArray();
        string[] values = dict.Select(f => f.Value).ToArray();
        photonView.RPC("UpdateDictionaries", RpcTarget.All, keys, values);
        UpdateInformation();
    }

    [PunRPC]

    public void UpdateDictionaries(int[] keys, string[] values)
    {
        Dictionary<int, string> newDict = new Dictionary<int, string>();
        List<playerData> datas = FindObjectsOfType<playerData>().ToList();
        Debug.Log(datas.Count);


        for (int i = 0; i < keys.Length; i++)
        {
            newDict[keys[i]] = values[i];
            if (PhotonNetwork.LocalPlayer.NickName == values[i])
            {
                data.SetRole(keys[i]);
            }
        
        }
        dict = newDict;

    }


    private void UpdateInformation()
    {
        int[] keys1 = new int[4] { -1, -1, -1, -1 };
        string[] values = new string[4] { "", "", "", "" };
        int m = 0;

        foreach (KeyValuePair<int, string> d in dict)
        {
            if (!string.IsNullOrEmpty(d.Value))
            {
                keys1[m] = d.Key;
                values[m] = d.Value;
            }
            else
            {
                keys1[m] = -1;
                values[m] = "";
            }
            m++;
        }


        updateRoomCanvas.Invoke(keys1, values);
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            foreach (KeyValuePair<int, string> pair in dict)
            {
                if (string.Equals(pair.Value, otherPlayer.NickName))
                {
                    dict[pair.Key] = "";
                    break;
                }
            }
            int[] keys = dict.Select(f => f.Key).ToArray();
            string[] values = dict.Select(f => f.Value).ToArray();
            photonView.RPC("UpdateDictionaries", RpcTarget.Others, keys, values);
            UpdateInformation();
        }
        updateActionsArea?.Invoke();

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

        }
    }

    private void CreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.IsOpen = true;
        roomOptions.IsVisible = false;
        roomOptions.MaxPlayers = 4;
        roomOptions.PublishUserId = true;
        string roomName = System.Guid.NewGuid().ToString();
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }


    public void SoloPlay()
    {
        PhotonNetwork.JoinRandomOrCreateRoom();
    }


    public override void OnJoinedRoom()
    {
        GameObject player = PhotonNetwork.Instantiate("PlayerData", Vector3.zero, Quaternion.identity);
        data = player.GetComponent<playerData>();
        playerdict.Add(PhotonNetwork.NickName, player.GetComponent<playerData>());
        if (PhotonNetwork.CurrentRoom.PlayerCount == 1)
        {
            dict[0] = PhotonNetwork.LocalPlayer.NickName;
        }
    }

    public override void OnLeftRoom()
    {
        playerdict.Remove(PhotonNetwork.NickName);
        clearRoomCanvas.Invoke();
        ClearDict();
        data = null;
    }

    public void ClearDict()
    {
        for (int i = 0; i < 4; i++)
        {
            dict[i] = "";
        }
        dict[0]= PhotonNetwork.LocalPlayer.NickName;
    }

}



