using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheeseObject : MonoBehaviour
{

    public byte cheeseId;

    private void Awake()
    {
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnCollect()
    {
        Destroy(gameObject);
    }
}
