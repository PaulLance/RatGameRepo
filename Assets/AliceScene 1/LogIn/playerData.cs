using Photon.Pun;
using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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
        UpateRole.Invoke(number);
    }

    public int GetNumber()
    {
        return number;
    }

}
