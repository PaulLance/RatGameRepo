using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundMove : MonoBehaviour
{
    [SerializeField] Transform center;
    [SerializeField] float radius;
    float speed;
    [SerializeField] float value;
    [SerializeField] float minAngle;
    [SerializeField] float maxAngle;

    Vector3 positionX;
    Vector3 positionY;
    float angle;



    private void Update()
    {
        angle += Time.deltaTime*speed;
        Debug.Log(angle);
        float posX = center.position.x + Mathf.Cos(angle) * radius;
        float posZ = center.position.z + Mathf.Sin(angle) * radius;
        transform.position = new Vector3(posX, transform.position.y, posZ);
        if (angle <= minAngle)  //-3.7f
        {
            speed = value;
        }
        if (angle >= maxAngle) //-0.79f
        {
            speed = -value;
        }



    }
}
