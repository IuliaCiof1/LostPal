using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundMusic : MonoBehaviour
{
    [SerializeField] private AudioClip loopClip;
    [SerializeField] private AudioClip endingClip;
    
    private AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = loopClip;
        audioSource.Play();
    }

    private void OnEnable()
    {
        EventManager.OnPlayerWins += TriggerEnding;
    }

    private void OnDisable()
    {
        EventManager.OnPlayerWins -= TriggerEnding;
    }

    public void TriggerEnding()
    {
        audioSource.clip = endingClip;
        audioSource.Play();
        
        EventManager.OnPlayerWins -= TriggerEnding;
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
