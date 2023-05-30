using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Services.Analytics;
using Unity.Services.Core;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.SceneManagement;
using Events = UnityEditor.PackageManager.Events;

public class AnalyticsSender : MonoBehaviour
{
    [SerializeField] private Transform playerBlock;
    async void Start()
    {
        // try
        // {
        //     await UnityServices.InitializeAsync();
        //     //List<string> consentIdentifiers = await AnalyticsService.Instance.CheckForRequiredConsents();
        // }
        // catch (ConsentCheckException e)
        // {
        //     Console.WriteLine(e);
        //     throw;
        // }
    }

    private void OnEnable()
    {
        PlayerController.OnPlayerWins += OnPlayerWinsHandler;
    }
    
    private void OnDisable()
    {
        PlayerController.OnPlayerWins -= OnPlayerWinsHandler;
    }

    void OnPlayerWinsHandler()
    {
        //GetHierarcy a = new GetHierarcy();
            
        //Debug.Log(a.GetCodeEditorChildrenString(playerBlock, 0));

        //Unity Analytics
        // AnalyticsService.Instance.CustomData("BlocksCombination", new Dictionary<string, object>
        // {
        //     {"Level", SceneManager.GetActiveScene().buildIndex},
        //     {"Combination", a.GetCodeEditorChildrenString(playerBlock, 0)},
        //     {"Time", Math.Round(Time.deltaTime,2)}
        // });
        
        //CustomEventManager.
        
        // AnalyticsEvent.Custom("BlocksCombination", new Dictionary<string, object>
        // {
        //     {"Level", SceneManager.GetActiveScene().buildIndex},
        //     {"Combination", a.GetCodeEditorChildrenString(playerBlock, 0)},
        //     {"Time", Math.Round(Time.deltaTime,2)}
        // });
        //PlayerController.OnPlayerWins -= OnPlayerWinsHandler;
        // AnalyticsService.Instance.Flush();
        // Debug.Log("analytics saved");
    }
}
