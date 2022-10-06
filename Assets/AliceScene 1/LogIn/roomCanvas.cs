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
    [SerializeField] TextMeshProUGUI roleText;
    

    PhotonView photonView;

    private void Awake()
    {
        MainLoobyManager.updateRoomCanvas += CanvasActions;
        MainLoobyManager.clearRoomCanvas += ClearRoomCanvas;
        playerData.UpateRole += UpdateRole;


    }

    private void UpdateRole(int num)
    {
        roleText.text = num.ToString();
    }


    private void Start()
    {
        photonView = GetComponent<PhotonView>();
    }
    public void CanvasActions(int[] objects, string[] values)
    {
        photonView.RPC("UpdateCanvas", RpcTarget.All, objects, values);
    }

    [PunRPC]
    public void UpdateCanvas(int[] keys, string[] values)
    {
        ActiveStartButton();

        if (canvasArea.activeInHierarchy == false)
        {
            canvasArea.SetActive(true);
        }

        for (int i = 0; i < keys.Length; i++)
        {
            GameObject obj = playersLabels.GetChild(i).gameObject;
            if (keys[i] == -1)
            {
                obj.GetComponent<Roles>().ClearRole();
                obj.GetComponentInChildren<TextMeshProUGUI>().text = "";
                obj.SetActive(false);

            }
            else {

                 obj.SetActive(true);
                 Buttons(obj);
                 obj.GetComponentInChildren<Roles>().SetRole(keys[i]);
                 obj.GetComponentInChildren<TextMeshProUGUI>().text = values[i];
             }

      
        }

        ActionsWhenFourPlayers();

    }

    private void Buttons(GameObject obj)
    {
         if (PhotonNetwork.IsMasterClient)
         {
            obj.GetComponent<Roles>().EnableButtons();
         }
        else
         {
            obj.GetComponent<Roles>().DisableButtons();
         }
    }

    private void ActionsWhenFourPlayers()
    {
        int playersInRoom=PhotonNetwork.CurrentRoom.PlayerCount;
        if (playersInRoom == 4)
        {
            ChangeStartButtonState(true);
        }
        else
        {
            ChangeStartButtonState(false);
        }
        if (playersInRoom == 1)
        {
            ClearRoomCanvas();
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
        if (PhotonNetwork.IsMasterClient)
        {
            if (startButton.activeInHierarchy == false) 
            {
                ChangeStartButtonState(false);
                startButton.SetActive(true);
            }
        }
        else {
            if (startButton.activeInHierarchy == false) { return; }
                startButton.SetActive(false); 
        }
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
            child.GetComponent<Roles>().ClearRole();
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
