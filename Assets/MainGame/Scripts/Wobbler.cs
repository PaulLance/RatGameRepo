using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wobbler : MonoBehaviour
{

    [Range(0.1f, 5)]
    public float WaitBetweenWobbles = 1.72f;
    [Range(0.01f, 1)]
    public float AngleChangeSpeed = 0.603f;
    [Range(0.01f, 5)]
    public float FloatSpeed = 1.08f;
    [Range(0.01f, 5)]
    public float FloatHeight = 0.55f;

    Quaternion _targetAngle;

    Vector3 startpos;

    // Start is called before the first frame update
    void Start()
    {
        startpos = transform.position;
        InvokeRepeating("ChangeTarget", 0, WaitBetweenWobbles);
    }

    // Update is called once per frame
    void Update()
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, _targetAngle, Time.deltaTime * AngleChangeSpeed);
        transform.position = startpos + Vector3.up * Mathf.Sin(Time.time * FloatSpeed) * FloatHeight;
    }

    void ChangeTarget()
    {
        _targetAngle = Random.rotation;
        //var curve = Mathf.Sin(Random.Range(0, Mathf.PI * 2));
        //var curve2 = Mathf.Cos(Random.Range(0, Mathf.PI * 2));
        //_targetAngle = Quaternion.Euler(transform.forward * curve + transform.up * curve2);
    }

}
