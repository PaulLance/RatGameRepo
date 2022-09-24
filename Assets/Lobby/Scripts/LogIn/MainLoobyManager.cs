using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainLoobyManager : MonoBehaviourPunCallbacks
{

    public static MainLoobyManager lobbyManager;
    private void Awake()
    {
        
            lobbyManager = this;
    }
    public void ConnectToMaster()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        Debug.Log("On connected to master");
    }


}
