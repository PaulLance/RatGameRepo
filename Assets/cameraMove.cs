using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraMove : MonoBehaviour
{
    [SerializeField] Transform point;
    void Update()
    {
        transform.LookAt(point);
    }
}
