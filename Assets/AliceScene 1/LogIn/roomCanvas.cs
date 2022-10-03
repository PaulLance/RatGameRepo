using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;
using UnityEngine.UI;

public class roomCanvas : MonoBehaviour
{
    [SerializeField] GameObject textObject;
    [SerializeField] Transform playersLabels;
    [SerializeField] GameObject canvasArea;
    [SerializeField] GameObject startButton;
    Color buttonActive = new Color(0.4921149f, 0.3224012f, 0.5943396f, 1f);
    Color buttonDisable = new Color(0.6226415f, 0.6226415f, 0.6226415f, 1f);

    PhotonView photonView;

    private void Awake()
    {
        MainLoobyManager.updateRoomCanvas += CanvasActions;
        MainLoobyManager.updateRoomCanvas1 += CanvasActions1;
        MainLoobyManager.clearRoomCanvas += ClearRoomCanvas;


    }

    [PunRPC]
    private void CanvasActionsRemove(string playerName)
    {
        ChangeStartButtonState(false) ;
        foreach (Transform child in playersLabels)
        {
            if (child.GetComponentInChildren<TextMeshProUGUI>().text == playerName)
            {
                child.GetComponentInChildren<TextMeshProUGUI>().text = "";
                child.gameObject.SetActive(false);
                break;
            }
        }
        ActiveStartButton();


    }

    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void CanvasActions(int[] objects, string[] values)
    {
        photonView.RPC("UpdateCanvas", RpcTarget.All, objects, values);
    }

    public void CanvasActions1(string name)
    {
        photonView.RPC("CanvasActionsRemove", RpcTarget.AllBuffered, name);
    }

    [PunRPC]
    public void UpdateCanvas(int[] keys, string[] values)
    {
        ActiveStartButton();

        if (canvasArea.activeInHierarchy == false)
        {
            canvasArea.SetActive(true);
        }

        for (int i=0; i<keys.Length; i++)
        {
            GameObject obj = playersLabels.GetChild(i).gameObject;
            if (keys[i] == -1)
            {
                obj.SetActive(false);
                obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
            }
            else
            {
                if (PhotonNetwork.CurrentRoom.PlayerCount > 1)
                {
                    obj.SetActive(true);
                    obj.GetComponentInChildren<TextMeshProUGUI>().text = values[i];
                }
                else { obj.SetActive(false); }

            }
        }

        if (PhotonNetwork.CurrentRoom.PlayerCount == 4)
        {
            ChangeStartButtonState(true);
        }
        else
        {
            ChangeStartButtonState(false);
        }


    }

    private void ChangeStartButtonState(bool state)
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (state == true)
            {
                startButton.GetComponent<Button>().enabled = true;
                startButton.GetComponent<Image>().color = buttonActive;
            }
            if (state == false)
            {
                startButton.GetComponent<Button>().enabled = false;
                startButton.GetComponent<Image>().color = buttonDisable;
            }

        }


    }

    private void ActiveStartButton()
    {
        if (PhotonNetwork.IsMasterClient && startButton.activeInHierarchy==false)
        {
            Debug.Log(60);
            ChangeStartButtonState(false);
            startButton.SetActive(true);
        }
        else { startButton.SetActive(false); }
    }

    public void LeaveParty()
    {
        PhotonNetwork.LeaveRoom();
    }

    public void ClearRoomCanvas()
    {

        foreach (Transform child in playersLabels)
        {
            child.GetComponentInChildren<TextMeshProUGUI>().text = "";
            child.gameObject.SetActive(false);
        }
        canvasArea.SetActive(false);
        startButton.SetActive(false);
    }


    public void PlayTeam()
    {
        PhotonNetwork.LoadLevel(1);
    }
}
