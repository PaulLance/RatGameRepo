using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;
using ExitGames.Client.Photon;
using PlayFab.EconomyModels;
using System.Linq;
using System;
using Random = UnityEngine.Random;

public class MainGameManager : MonoBehaviourPunCallbacks, IOnEventCallback
{
    [Tooltip("The prefab to use for representing the player")]
    public GameObject playerPrefab;
    public GameObject cheesePrefab;
    public GameObject trapPrefab;
    public static MainGameManager Instance;

    public Transform[] mouseTrapPositions;
    public Vector3[] mouseTrapSavedPositions;

    public const int NUMBER_OF_TEAMS = 1;
    public const int PLAYERS_PER_TEAM = 4;
    public const int CHEESE_AMOUNT = 25;
    Vector3 cheeseMinPos = new Vector3(16, -0.5f, 32f);
    Vector3 cheeseMaxPos = new Vector3(16, -1.25f, 32f);
    //public const int TRAPS_AMOUNT = 45;

    GameTeamManager[] gameTeamManagers;
    private bool setupDone = false;
    private bool tryingToSetup = false;
    [SerializeField] private bool DBG_FULL_TEAMS = false;

    public const byte DoSetupEventCode = 1;
    public const byte CollectCheeseEventCode = 5;

    public byte MyTeamNum;

    [SerializeField] Transform pointToCreate;

    public enum PlayerType : byte
    {
        Movement = 0,
        Nose,
        Eyes,
        Ears,
    }

    void Start()
    {
        Instance = this;

        mouseTrapSavedPositions = new Vector3[mouseTrapPositions.Length];
        for (int i = 0; i < mouseTrapSavedPositions.Length; i++)
        {
            mouseTrapSavedPositions[i] = mouseTrapPositions[i].position;
            Destroy(mouseTrapPositions[i].gameObject);
        }

    }

    public void OnCollectCheese(CheeseObject co, byte teamNum)
    {
        gameTeamManagers[teamNum].allCheese[co.cheeseId].collected = true;
        RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All };
        object[] content = new object[] { co.cheeseId, teamNum };
        PhotonNetwork.RaiseEvent(CollectCheeseEventCode, content, raiseEventOptions, SendOptions.SendReliable);
    }

    private void CollectCheese(object[] data)
    {
        byte cheeseId = (byte)data[0];
        byte teamNum = (byte)data[1];

        if(teamNum != MyTeamNum)
        {
            return;
        }

        var cheese = gameTeamManagers[teamNum].CollectCheese(cheeseId);
        AudioManager.Instance.OnCollectCheese(cheese.position);
    }


    void DoSetup(object[] data)
    {
        Debug.Log("DoSetup getting executed ... !");

        byte teamNum = (byte)data[0];
        byte playerType = (byte)data[1];

        MyTeamNum = teamNum;

        PlayerType pt = (PlayerType)playerType;
        //TestPlayerManager.LocalPlayerInstance.GetComponent<TestPlayerManager>().playerType = pt;

        gameTeamManagers = new GameTeamManager[NUMBER_OF_TEAMS];
        for (int i = 0; i < gameTeamManagers.Length; i++)
        {
            gameTeamManagers[i] = new GameTeamManager(teamNum, "");
        }

        Vector3[] cheeseLocations = (Vector3[])data[2];


        Vector3[] trapLocations = (Vector3[])data[3];
        byte[] trapTypesByte = (byte[])data[4];
        GameTeamManager.Trap.TrapType[] trapTypes = new GameTeamManager.Trap.TrapType[trapLocations.Length];
        for (int i = 0; i < trapTypesByte.Length; i++)
        {
            trapTypes[i] = (GameTeamManager.Trap.TrapType)trapTypesByte[i];
        }

        CheeseObject[] cheeseObjects = new CheeseObject[cheeseLocations.Length];

        for (int i = 0; i < cheeseLocations.Length; i++)
        {
            var cheeseObj = Instantiate(cheesePrefab, cheeseLocations[i], Quaternion.identity);
            cheeseObjects[i] = cheeseObj.GetComponent<CheeseObject>();
            cheeseObjects[i].cheeseId = (byte)i;



            bool enableMeshRenderer = true;
            bool enableTrigger = true;

            if (pt == PlayerType.Nose)
            {
                enableMeshRenderer = true;
                enableTrigger = true;
            }
            else if (pt == PlayerType.Movement)
            {
                enableMeshRenderer = false;
                enableTrigger = true;
            }
            else
            {
                enableMeshRenderer = false;
                enableTrigger = true;
            }

            cheeseObj.GetComponentInChildren<MeshRenderer>().enabled = enableMeshRenderer;
            cheeseObj.GetComponentInChildren<Collider>().enabled = enableTrigger;

        }

        foreach (var tl in trapLocations)
        {
            var trapObj = Instantiate(trapPrefab, tl, Quaternion.identity);

            bool enableMeshRenderer = true;
            bool enableTrigger = true;

            if (pt == PlayerType.Eyes)
            {
                enableMeshRenderer = true;
                enableTrigger = true;
            }
            else if(pt == PlayerType.Movement)
            {
                enableMeshRenderer = false;
                enableTrigger = true;
            }
            else
            {
                enableMeshRenderer = false;
                enableTrigger = true;
            }

            var mrs = trapObj.GetComponentsInChildren<MeshRenderer>();
            var colls = trapObj.GetComponentsInChildren<Collider>();
            for (int i = 0; i < mrs.Length; i++)
            {
                mrs[i].enabled = enableMeshRenderer;
            }
            for (int i = 0; i < colls.Length; i++)
            {
                colls[i].enabled = enableTrigger;
            }
        }

        gameTeamManagers[teamNum].SetCheeseLocations(cheeseLocations, cheeseObjects);
        gameTeamManagers[teamNum].SetTrapLocations(trapLocations, trapTypes);

        if (pt == PlayerType.Movement)
        {

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
                    GameObject newObject = PhotonNetwork.Instantiate(this.playerPrefab.name, pointToCreate.position, Quaternion.identity, 0);
                    TestPlayerManager tpm = newObject.GetComponent<TestPlayerManager>();
                    tpm.TeamNum = teamNum;
                }
                else
                {
                    Debug.LogFormat("Ignoring scene load for {0}", SceneManagerHelper.ActiveSceneName);
                }
            }
        }
        else
        {
            StartCoroutine(FindCameraWork());

        }

        setupDone = true;
    }

    IEnumerator FindCameraWork()
    {

        while (true)
        {
            CameraWork cw = FindObjectOfType<CameraWork>();

            if(cw != null)
            {
                cw.OnStartFollowing();
                break;
            }

            yield return new WaitForSeconds(0.5f);
        }
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

            DoSetup(data);
        }else if(eventCode == CollectCheeseEventCode)
        {
            object[] data = (object[])photonEvent.CustomData;

            CollectCheese(data);
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
                byte currentPlayerTypeByte = 0;
                byte teamNum = 0;

                Vector3[] cheeseLocations = new Vector3[CHEESE_AMOUNT];
                for (int i = 0; i < cheeseLocations.Length; i++)
                {
                    cheeseLocations[i] = new Vector3(
                        Random.Range(cheeseMinPos.x, cheeseMaxPos.x),
                        Random.Range(cheeseMinPos.y, cheeseMaxPos.y),
                        Random.Range(cheeseMinPos.z, cheeseMaxPos.z));
                }
                //Vector3[] trapLocations = new Vector3[TRAPS_AMOUNT];
                //byte[] trapTypes = new byte[TRAPS_AMOUNT];
                Vector3[] trapLocations = new Vector3[mouseTrapSavedPositions.Length];
                byte[] trapTypes = new byte[mouseTrapSavedPositions.Length];
                for (int i = 0; i < trapLocations.Length; i++)
                {
                    //trapLocations[i] = new Vector3(Random.Range(-35, 35), -1.22f, Random.Range(-35, 35));
                    trapLocations[i] = mouseTrapSavedPositions[i];
                    trapTypes[i] = (byte)Random.Range(0, Enum.GetValues(typeof(GameTeamManager.Trap.TrapType)).Length);
                }


                //PhotonView[] pv = FindObjectsOfType<PhotonView>();
                playerData[] pds = FindObjectsOfType<playerData>();

                int amountPlayers = 0;
                List<PlayerType> currentTeamTypes = new List<PlayerType>(); // to make sure every team has every slot and not double


                foreach (var playerEntry in PhotonNetwork.CurrentRoom.Players)
                {
                    //PlayerType currentPlayerType = (PlayerType)currentPlayerTypeByte; 
                    var player = playerEntry.Value;

                    PhotonView playerView = null;
                    playerData pd = null;
                    for (int i = 0; i < pds.Length; i++)
                    {
                        var pv = pds[i].GetComponent<PhotonView>();
                        if (pv && pv.Owner.ActorNumber == player.ActorNumber)
                        {
                            pd = pds[i];
                            playerView = pv;
                            break;
                        }
                    }

                    if(playerView == null)
                    {
                        Debug.LogError("PV = NULL!!");
                        return;
                    }

                    // Assign player type from role number now!
                    int role = pd.GetNumber();
                    Debug.Log("Player " + playerView.Owner.ActorNumber + " : "  + role);
                    currentPlayerTypeByte = (byte)role;
                    PlayerType currentPlayerType = (PlayerType)currentPlayerTypeByte;
                    while (currentTeamTypes.Contains(currentPlayerType))
                    {
                        currentPlayerTypeByte = (byte)Random.Range(0, PLAYERS_PER_TEAM);
                        currentPlayerType = (PlayerType)currentPlayerTypeByte;
                    }
                    currentTeamTypes.Add(currentPlayerType);
                    pd.SetRole(currentPlayerTypeByte);


                    object[] content;

                    content = new object[] { 
                        teamNum,
                        currentPlayerTypeByte,
                        cheeseLocations,
                        trapLocations,
                        trapTypes
                    };

                    // extra stuff maybe?
                    if (currentPlayerType == PlayerType.Nose)
                    {

                    }
                    else
                    {

                    }

                    RaiseEventOptions raiseEventOptions = new RaiseEventOptions { Receivers = ReceiverGroup.All, TargetActors = new int[] { player.ActorNumber } }; // You would have to set the Receivers to All in order to receive this event on the local client as well
                    PhotonNetwork.RaiseEvent(DoSetupEventCode, content, raiseEventOptions, SendOptions.SendReliable);

                    //PhotonNetwork.RPC("DoSetup", player, currentPlayerType);

                    //currentPlayerTypeByte++;
                    //if (currentPlayerTypeByte >= PLAYERS_PER_TEAM)
                    //{
                    //    currentPlayerTypeByte = 0;
                    //    teamNum++;
                    //}
                    

                    // temporary team number setter ... 
                    // TODO : replace it with actual team 
                    amountPlayers++;
                    if(amountPlayers >= PLAYERS_PER_TEAM)
                    {
                        currentTeamTypes = new List<PlayerType>();
                        teamNum++;
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
