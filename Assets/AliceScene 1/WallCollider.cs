using Photon.Pun;
using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Hashtable = ExitGames.Client.Photon.Hashtable;

public class WallCollider : MonoBehaviourPunCallbacks
{
    PhotonView photonView;
    [SerializeField] GameObject wall;

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            wall.SetActive(true);
            CatAI[] cats = FindObjectsOfType<CatAI>();
            foreach (CatAI cat in cats)
            {
                cat.EnableChase();
            }
            photonView.RPC("EnableWall", RpcTarget.Others);

        }
    }


    [PunRPC]
    public void EnableWall()
    {
        wall.SetActive(true);
        CatAI[] cats=FindObjectsOfType<CatAI>();
        foreach (CatAI cat in cats)
        {
            cat.EnableChase();
        }
    }





}
