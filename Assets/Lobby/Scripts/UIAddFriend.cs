using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIAddFriend : MonoBehaviour
{
    string name;

    public static Action<string> OnAddFriend;
    public static Action<string> OnRemoveFriend;


    public void SetFriendName(string friendName)
    {
        name = friendName;
    }
    public void AddButtom()
    {
        if (string.IsNullOrEmpty(name)) { return; }
        OnAddFriend?.Invoke(name);
        name = null;
    }
}
