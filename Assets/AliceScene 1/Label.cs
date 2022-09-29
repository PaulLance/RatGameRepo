using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Label : MonoBehaviour
{
    public void SetLabel(string label)
    {
        GetComponent<TextMeshProUGUI>().text = label;
    }
}
