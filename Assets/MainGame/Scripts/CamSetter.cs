using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamSetter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FindObjectOfType<CameraMovementBizzy>().cameraTarget = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
