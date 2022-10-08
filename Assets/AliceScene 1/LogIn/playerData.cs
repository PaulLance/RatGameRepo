using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;


public class playerData : MonoBehaviourPunCallbacks
{
    [SerializeField] int number;
    public static Action<int> UpateRole;
    private void Awake()
    {
        DontDestroyOnLoad(this.gameObject);
    }

    private void Start()
    {
        PhotonView photonView = GetComponent<PhotonView>();
    }
    public override void OnLeftRoom()
    {
        PhotonNetwork.Destroy(photonView);
    }

    internal void SetRole(int roleNumber)
    {
        number = roleNumber;
        if (photonView.IsMine)
        {
            Hashtable hash = new Hashtable();
            hash.Add("number", number);
            PhotonNetwork.LocalPlayer.SetCustomProperties(hash);
            UpateRole.Invoke(number);
        }
    }

    public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
    {
        if (!photonView.IsMine && targetPlayer == photonView.Owner)
        {
            SetRole((int)changedProps["number"]);
        }
    }



    public int GetNumber()
    {
        return number;
    }

}
