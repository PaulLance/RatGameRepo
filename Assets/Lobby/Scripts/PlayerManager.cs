using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    PhotonView Pv;
    void Awake()
    {
        Pv = GetComponent<PhotonView>();
    }

    void Start()
    {
        CreatePlayerController();
    }

    void CreatePlayerController()
    {
        if (Pv.IsMine)
        {
            Vector3 position = new Vector3(Random.Range(-5f, 5f), 0.5f, Random.Range(-5f, 5f));
            PhotonNetwork.Instantiate(Path.Combine("Prefabs", "Player"), position, Quaternion.identity);
        }
    }
}
