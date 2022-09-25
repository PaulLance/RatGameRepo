using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Launcher : MonoBehaviourPunCallbacks
{
    Menu menu;
    Dictionary<string, GameObject> rooms=new Dictionary<string, GameObject>();
    public static Action GetPhotonFriends;

    void Start()
    {
        menu = FindObjectOfType<Menu>();
        menu.LoadMenu();
        PhotonNetwork.ConnectUsingSettings();

        string username = PlayerPrefs.GetString("username", "Player " + UnityEngine.Random.Range(0, 1000));

        PhotonNetwork.NickName = username;
        PhotonNetwork.GameVersion = "1";
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public override void OnConnectedToMaster()
    {
        menu.OpenLobbyMenu();
        Debug.Log("ConnectedToMaster");
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {

        Debug.Log("JoinedLobby");
    }

    public void CreateRoomButton()
    {
        if (string.IsNullOrEmpty(menu.inputField.text)) { return; }
        if (rooms.ContainsKey(menu.inputField.text))
        {
            menu.inputField.text = null;
            menu.inputField.placeholder.GetComponent<TextMeshProUGUI>().color = new Color(255,0,0,170);
            menu.inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "This name already exists";
            return;
        }
        PhotonNetwork.CreateRoom(menu.inputField.text, new Photon.Realtime.RoomOptions { MaxPlayers = 8, EmptyRoomTtl = 0 });
        Menu.menu.CloseCreateRoomMenu();
        Menu.menu.OpenLoadMenu();
    }

    public void JoinRoom(string roomName)
    {
        // GetPhotonFriends.Invoke();
        Menu.menu.CloseFindRoomMenu();
        Menu.menu.OpenLoadMenu();
        PhotonNetwork.JoinRoom(roomName);
    }

    public override void OnJoinedRoom()
    {
        PhotonView Pv = GetComponent<PhotonView>();
        Menu.menu.CloseLoadMenu();
        Menu.menu.OpenRoom();
        Pv.RPC("UpdateRoomList", RpcTarget.All);

        // PhotonNetwork.LoadLevel("Game");
    }

    [PunRPC]
    void UpdateRoomList()
    {
        FindObjectOfType<Room>().UpdateRoomList();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
    
        FindObjectOfType<Room>().UpdateRoomList();

    }

    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
        menu.CloseRoom();
        menu.OpenLobbyMenu();
    }

    public void StartGame()
    {
        PhotonNetwork.LoadLevel(2);
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        base.OnCreateRoomFailed(returnCode, message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
          
            if (rooms.ContainsKey(info.Name))
            {
                rooms[info.Name].GetComponent<ButtonRoom>().SetRoomInformation(info);
            }
            else
            {
                GameObject button = Instantiate(menu.roomButton, menu.listContent);
                button.GetComponent<ButtonRoom>().SetRoomInformation(info);
                rooms.Add(info.Name, button);
            }
            rooms[info.Name].GetComponent<ButtonRoom>().RoomStatus(info.IsOpen);

            if (info.PlayerCount == 0)
            {
                Destroy(rooms[info.Name].gameObject);
                rooms.Remove(info.Name);
            }


        }
    }

 


}
