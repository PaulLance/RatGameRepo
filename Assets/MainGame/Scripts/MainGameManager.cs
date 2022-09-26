using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using PlayFab.EconomyModels;

public class MainGameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public static MainGameManager Instance;

    public const int NUMBER_OF_TEAMS = 2;
    public const int PLAYERS_PER_TEAM = 4;

    GameTeamManager[] gameTeamManagers;
    private bool setupDone = false;
    private bool tryingToSetup = false;
    [SerializeField] private bool DBG_FULL_TEAMS = false;

    public const byte DoSetupEventCode = 1;

    public enum PlayerType : byte
    {
        Movement = 0,
        Nose,
        Ears,
        Eyes,
    }

    void Start()
    {
        Instance = this;


        if (playerPrefab == null)
        {
            Debug.LogError("<Color=Red><a>Missing</a></Color> playerPrefab Reference. Please set it up in GameObject 'Game Manager'", this);
        }
        else
        {
            Debug.LogFormat("We are Instantiating LocalPlayer from {0}", Application.loadedLevelName);
            // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
            if (TestPlayerManager.LocalPlayerInstance == null)
            {
                Debug.LogFormat("We are Instantiating LocalPlayer from {0}", SceneManagerHelper.ActiveSceneName);
                // we're in a room. spawn a character for the local player. it gets synced by using PhotonNetwork.Instantiate
                GameObject newObject = PhotonNetwork.Instantiate(this.playerPrefab.name, new Vector3(0f, 5f, 0f), Quaternion.identity, 0);
            }
            else
            {
                Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
            }
        }
    }



    [PunRPC]
    void SetupCheese(Vector3[] cheeseLocations, PhotonMessageInfo info)
    {

    }

    void DoSetup(byte playerType)
    {   
        Debug.Log("DoSetup getting executed ... !");

        PlayerType pt = (PlayerType)playerType;
        TestPlayerManager.LocalPlayerInstance.GetComponent<TestPlayerManager>().playerType = pt;
        setupDone = true;
    }


    private void OnEnable()
    {
        base.OnEnable();
        PhotonNetwork.AddCallbackTarget(this);
    }

    private void OnDisable()
    {
        base.OnDisable();
        PhotonNetwork.RemoveCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        //Debug.Log("OnEvent getting executed ... !");
        byte eventCode = photonEvent.Code;

        if (eventCode == DoSetupEventCode)
        {
            if (setupDone)
            {
                return;
            }
            setupDone = true;
            Debug.Log("DoSetupEventCode!");
            object[] data = (object[])photonEvent.CustomData;

            byte playerType = (byte)data[0];
            DoSetup(playerType);
        }
    }


    private void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (Input.GetKeyDown(KeyCode.F6))
            {
                DBG_FULL_TEAMS = true;
            }

            if (!tryingToSetup && (PhotonNetwork.CurrentRoom.PlayerCount == NUMBER_OF_TEAMS * PLAYERS_PER_TEAM || DBG_FULL_TEAMS))
            {
                tryingToSetup = true;
                Debug.Log("Masterclient here. Setting up!");
                byte currentPlayerType = 0;
                foreach(var playerEntry in PhotonNetwork.CurrentRoom.Players)
                {
                    var player = playerEntry.Value;

                    object[] content = new object[] { currentPlayerType };
                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, TargetActors = new int[] { player.ActorNumber } }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                    PhotonNetwork.RaiseEvent(DoSetupEventCode, content, raiseEventOptions, SendOptions.SendReliable);

                    //PhotonNetwork.RPC("DoSetup", player, currentPlayerType);

                    currentPlayerType++;
                    if (currentPlayerType >= PLAYERS_PER_TEAM)
                    {
                        currentPlayerType = 0;
                    }
                }
            }
        }
        
    }


    #region Photon Callbacks


    /// <summary>
    /// Called when the local player left the room. We need to load the launcher scene.
    /// </summary>
    public override void OnLeftRoom()
    {
        SceneManager.LoadScene("Lobby");
    }


    #endregion


    #region Public Methods


    public void LeaveRoom()
    {
        PhotonNetwork.LeaveRoom();
    }


    #endregion
}
