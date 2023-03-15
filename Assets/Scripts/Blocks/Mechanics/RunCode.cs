using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class RunCode : MonoBehaviour
{
    [SerializeField] private Transform playerBlock;

    [SerializeField] private Transform CodeEditor;

    [SerializeField] private float secondsToWait;

    [SerializeField] private Animator anim;
    private Transform SnapPoint;
    
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private PlayerController player;

    private Button runBtnComponent;

    private bool isAllBlocksExecuted;
    private void Start()
    {
        //PlayerWinWatcher.onPlayerWin += HandlePlayerWin;
        runBtnComponent = GetComponent<Button>();
        //audioSource = GetComponent<AudioSource>();
    }

    public void Run()
    {
        isAllBlocksExecuted = false;
        
        //get the blocks inside the "Player" block that's inside the CodeEditor
        //only those blocks will be executed
        if (playerBlock.IsChildOf(CodeEditor))
        {
            SnapPoint = playerBlock.Find("SnapPoint");

            StartCoroutine(CoroutineWaitForSeconds());
        }
        
    }

    //Caroutine is needed since for loops move faster than the frames and animation
    public IEnumerator CoroutineWaitForSeconds()
    {
        //Get every block inside the Player block and execute it
        for (int i = 0; i < SnapPoint.childCount; i++)
        {
            runBtnComponent.enabled = false;
            Transform block = SnapPoint.GetChild(i);
            
            block.GetChild(0).gameObject.SetActive(true);
            
            yield return new WaitUntil(()=>!block.GetChild(0).gameObject.activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
            yield return new WaitForSeconds(secondsToWait); //wait until animation ends
           
        }

        isAllBlocksExecuted = true;
        player.RestartPosition();
        runBtnComponent.enabled = true;
    }

    public void OnEnable()
    {
        EventManager.OnPlayerWins += PlayerWinHandle;
    }

    public void OnDisable()
    {
        EventManager.OnPlayerWins -= PlayerWinHandle;
    }

    void PlayerWinHandle()
    {
        runBtnComponent.enabled = false;
        finishPanel.SetActive(true);
    }
}
