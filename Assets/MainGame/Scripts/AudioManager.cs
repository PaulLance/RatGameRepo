using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public class AudioManager : MonoBehaviour
{

    public AudioClip[] cheeseCollectClips;
    private int currentCheeseIndex = 0;

    public AudioClip laserBurnClip;
    public AudioClip mewoClip;
    public AudioClip mouseTrapClip;

    public static AudioManager Instance;

    private void Awake()
    {
        Instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void OnCollectCheese(Vector3 pos)
    {
        AudioSource.PlayClipAtPoint(cheeseCollectClips[currentCheeseIndex], pos);
        currentCheeseIndex++;
        if(currentCheeseIndex >= cheeseCollectClips.Length)
        {
            currentCheeseIndex = 0;
        }
    }

    internal void OnMouseTrap(Vector3 position)
    {
        AudioSource.PlayClipAtPoint(mouseTrapClip, position);
    }
}
