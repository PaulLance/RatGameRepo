using Photon.Realtime;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ButtonRoom : MonoBehaviour
{
    public TextMeshProUGUI roomName;
    [SerializeField] TextMeshProUGUI playersAmount;
    Color colorOpen = new Color(255, 255, 255, 1);
    Color colorClose = new Color(255, 255, 255, 0.1f);
    RoomInfo currentRoomInfo;



    public void SetRoomInformation(RoomInfo info )
    {
        roomName.text = info.Name;
        playersAmount.text = info.PlayerCount.ToString()+"/"+info.MaxPlayers.ToString();
   
    }

    public void RoomStatus(bool status)
    {
        if (status==true)
        {
            GetComponent<Image>().color = colorOpen;
        }
        else if (status == false)
        {
            GetComponent<Image>().color = colorClose;
        }
    }

    public void JoinRoom()
    {
        FindObjectOfType<Launcher>().JoinRoom(roomName.text);
    }
}
