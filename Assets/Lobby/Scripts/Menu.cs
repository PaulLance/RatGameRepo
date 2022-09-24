using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Photon.Realtime;

public class Menu : MonoBehaviour
{
    [SerializeField] GameObject loadMenu;
    [SerializeField] GameObject lobbyMenu;
    [SerializeField] GameObject createRoomMenu;
    [SerializeField] GameObject findroomMenu;
    [SerializeField] GameObject room;


    public TMP_InputField inputField;
    public GameObject roomButton;
    public Transform listContent;

    public static Menu menu;

    private void Start()
    {
        menu = this;
    }

    public void OpenLoadMenu()
    {
        loadMenu.SetActive(true);
    }

    public void CloseLoadMenu()
    {
        loadMenu.SetActive(false);

    }
    public void OpenLobbyMenu()
    {
        lobbyMenu.SetActive(true);
        loadMenu.SetActive(false);
    }

    public void LoadMenu()
    {
        loadMenu.SetActive(true);

    }

    public void Quit()
    {
        Application.Quit();
    
    }

    public void CreateRoomMenu()
    {
        createRoomMenu.SetActive(true);
        lobbyMenu.SetActive(false);
    }

    public void LobbyMenu()
    {
        lobbyMenu.SetActive(true);

    }

    public void CloseCreateRoomMenu()
    {
        inputField.text = null;
        inputField.placeholder.GetComponent<TextMeshProUGUI>().color = new Color32(50, 50, 50, 128);
        inputField.placeholder.GetComponent<TextMeshProUGUI>().text = "Enter text...";
        createRoomMenu.SetActive(false);
    }


    public void CloseLobbyMenu()
    {
        lobbyMenu.SetActive(false);
    }

    public void CloseFindRoomMenu()
    {
        findroomMenu.SetActive(false);
    }

    public void OpenFindRoomMenu()
    {
        findroomMenu.SetActive(true);
    }


    public void OpenRoom()
    {
        room.SetActive(true);

        room.GetComponent<Room>().SetupRoom();
    }

    public void CloseRoom()
    {
        room.SetActive(false);
    }





}
