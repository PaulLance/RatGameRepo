using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Roles : MonoBehaviour
{
    [SerializeField] Image mainImage;
    public static Action<int, int> changeRole;
    [SerializeField] List<ImObject> sprites;
    int roleNumber = -1;
    [SerializeField] GameObject buttons;

    [System.Serializable]
    struct ImObject
    {
        public Sprite sprite;
        public float width;
        public float height;
    }
    public void Next()
    {
        if (roleNumber == -1) { return; }
        int newNumber = roleNumber + 1;
        if (newNumber > 3)
        {
            newNumber = 0;
        }
      //  mainImage.sprite = sprites[newNumber];

        changeRole.Invoke(roleNumber,  newNumber);
    }

    public void Back()
    {
        if (roleNumber == -1) { return; }
        Debug.Log(99);
        int newNumber = roleNumber - 1;
        if (newNumber < 0)
        {
            newNumber = 3;
        }
        //mainImage.sprite = sprites[newNumber];

        changeRole.Invoke(roleNumber, newNumber);

    }

    public void DisableButtons()
    {
        buttons.SetActive(false);
    }

    public void EnableButtons()
    {
        buttons.SetActive(true);
    }

    public void SetRole(int roleNumber1)
    {
        Debug.Log(34);
        roleNumber = roleNumber1;
        ImObject obj = sprites[roleNumber];
        mainImage.sprite = sprites[roleNumber].sprite;
        mainImage.GetComponent<RectTransform>().sizeDelta = new Vector2(obj.width, obj.height);
    }

    internal void ClearRole()
    {
        roleNumber = -1;
    }
}
