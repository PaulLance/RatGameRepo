using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine;

public class Room : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI roomName;
    [SerializeField] Transform playerContainer;
    [SerializeField] GameObject playerLabel;
    Player[] players;
    [SerializeField] GameObject startButton;
    public void SetupRoom()
    {
        roomName.text = PhotonNetwork.CurrentRoom.Name;
        startButton.SetActive(PhotonNetwork.IsMasterClient);
    }

    public void UpdateRoomList()
    {
        ClearRoomList();
        players = PhotonNetwork.PlayerList;
        foreach (Player player in players)
        {
            GameObject newLabel=Instantiate(playerLabel, playerContainer);
            newLabel.GetComponent<TextMeshProUGUI>().text = player.NickName;
        }

    }

    public void ClearRoomList()
    {
        foreach (Transform obj in playerContainer)
        {
            Destroy(obj.gameObject);
        }
    }

    
}
