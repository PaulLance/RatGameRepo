using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class panel : MonoBehaviour
{
    int friendPanelMove = 0;
    Vector3 fPanelStartPos;
    Vector3 fPanelEndPos;
    [SerializeField] float fPanelSpeed = 1000;
    RectTransform panelRect;


    private void Start()
    {
        panelRect = GetComponent<RectTransform>();
        fPanelStartPos = panelRect.anchoredPosition;
        fPanelEndPos = new Vector2(-panelRect.anchoredPosition.x, panelRect.anchoredPosition.y);

    }


    private void Update()
    {
        if (friendPanelMove == 1)
        {
            Vector3 newPos = Vector3.MoveTowards(panelRect.anchoredPosition, fPanelEndPos, fPanelSpeed * Time.deltaTime);
            panelRect.anchoredPosition = newPos;
            if (Vector3.Distance(newPos, fPanelEndPos) < 0.05f)
            {
                panelRect.anchoredPosition = fPanelEndPos;
                friendPanelMove = 0;
            }
        }

        if (friendPanelMove == 2)
        {
            Vector3 newPos = Vector3.MoveTowards(panelRect.anchoredPosition, fPanelStartPos, fPanelSpeed * Time.deltaTime);
            panelRect.anchoredPosition = newPos;
            if (Vector3.Distance(newPos, fPanelEndPos) < 0.05f)
            {
                panelRect.anchoredPosition = fPanelStartPos;
                friendPanelMove = 0;
            }
        }
    }


    public void MoveTowards()
    {
        friendPanelMove = 1;
    }
  
    public void MoveBackwards()
    {
        friendPanelMove = 2;
    }
    

  
}
