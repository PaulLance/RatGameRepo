using Photon.Realtime;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class friendFieldUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI friendName;
    [SerializeField] Transform status;

    internal void UpdateInfo(FriendInfo friendInfo)
    {
        friendName.text = friendInfo.Name;

        //Debug.Log(friendInfo.IsOnline);
        //if (friendInfo.IsOnline)
        //{
        //    status.GetChild(0).gameObject.SetActive(true);
        //    status.GetChild(1).gameObject.SetActive(false);

        //}
        //else
        //{
        //    status.GetChild(1).gameObject.SetActive(true);
        //    status.GetChild(0).gameObject.SetActive(false);

        //}

    }

    
    public void CreateActionsArea()
    {
        ActionsArea[] actionsAreas = FindObjectsOfType<ActionsArea>();

        foreach (ActionsArea area in actionsAreas)
        {
            Destroy(area.gameObject);
        }

        GameObject actionsArea = FindObjectOfType<LobbyUI>().actionsArea;
        GameObject friendPanel = FindObjectOfType<LobbyUI>().friendPanel.gameObject;

        GameObject newActions = Instantiate(actionsArea, friendPanel.transform);
        newActions.GetComponent<ActionsArea>().SetFriendName(friendName.text);
        newActions.GetComponent<RectTransform>().position = GetComponent<RectTransform>().transform.position;
        newActions.GetComponent<RectTransform>().localPosition = new Vector2(0 + newActions.GetComponent<RectTransform>().sizeDelta.x / 2 + friendPanel.GetComponent<RectTransform>().sizeDelta.x / 2, newActions.GetComponent<RectTransform>().localPosition.y);
    }
}
