using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventManager : MonoBehaviour
{
    public delegate void PlayerWins();

    public static event PlayerWins OnPlayerWins;

    [SerializeField] private PlayerController player;
    private void Update()
    {
        if(player.IsWin)
            OnPlayerWins?.Invoke();
    }
}

