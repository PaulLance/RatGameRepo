using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using UnityEngine.SceneManagement;
using Photon.Realtime;

public class GameManager : MonoBehaviourPunCallbacks
{

    public void LeftRoomButton()
    {
        PhotonNetwork.LeaveRoom();
    }
     public override void OnLeftRoom()
     {
        SceneManager.LoadScene("Lobby");
     }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log(newPlayer.NickName + "entered room");
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeRoomStatus();
        }
    }

    private void ChangeRoomStatus()
    {
        if (PhotonNetwork.CurrentRoom.PlayerCount >= PhotonNetwork.CurrentRoom.MaxPlayers)
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
        }
        else
        {
            PhotonNetwork.CurrentRoom.IsOpen = true;
        }
    }

   

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        Debug.Log(otherPlayer.NickName + "left room");
        if (PhotonNetwork.IsMasterClient)
        {
            ChangeRoomStatus();
        }

    }




}
