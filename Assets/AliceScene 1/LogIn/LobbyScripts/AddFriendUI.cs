using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System;

public class AddFriendUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI report;

    private void Awake()
    {
        PlayfabFriendSystem.FriendUIReport += SetReportText;
    }
    private void OnDestroy()
    {
        PlayfabFriendSystem.FriendUIReport -= SetReportText;

    }
    public void SetReportText(string text)
    {
        report.text = text;
    }
}
