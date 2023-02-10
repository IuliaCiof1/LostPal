using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
public class RunCode : MonoBehaviour
{
    [SerializeField]
    private Transform playerBlock;

    [SerializeField] private Transform CodeEditor;

    [SerializeField]
    private float secondsToWait;

    private Transform block;

    private Transform SnapPoint;

    private bool isWin;
    [SerializeField] private GameObject finishPanel;
    [SerializeField] private GameObject failPanel;
    [SerializeField] private PlayerController player;
    private void Start()
    {
        //PlayerWinWatcher.onPlayerWin += HandlePlayerWin;
    }

    public void Run()
    {
        //get the blocks inside the "Player" block that's inside the CodeEditor
        //only those blocks will be executed
        if (playerBlock.IsChildOf(CodeEditor))
        {
            SnapPoint = playerBlock.Find("SnapPoint");

            StartCoroutine(CoroutineWaitForSeconds());
            //CoroutineWaitForSeconds();
        }
        
    }

    //Caroutine is needed since for loops moves faster than the frames
    public IEnumerator CoroutineWaitForSeconds()
    {
        for (int i = 0; i < SnapPoint.childCount; i++)
        {
            block = SnapPoint.GetChild(i);
                
            //Activates the child of the executable block 
            block.GetChild(0).gameObject.SetActive(true);
            yield return new WaitForSeconds(secondsToWait);
        }
       
        if (player.IsWin)
        {
            finishPanel.SetActive(true);
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    void HandlePlayerWin()
    {
        
    }
}
