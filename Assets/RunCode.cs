using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class RunCode : MonoBehaviour
{
    [SerializeField]
    private Transform playerBlock;

    [SerializeField] private Transform CodeEditor;

    [SerializeField]
    private float secondsToWait;

    private Transform block;

    private Transform SnapPoint;
    private void Start()
    {
        //StartCoroutine(CoroutineWaitForSeconds());
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

    public IEnumerator CoroutineWaitForSeconds()
    {
        for (int i = 0; i < SnapPoint.childCount; i++)
        {
            block = SnapPoint.GetChild(i);
                
            //Activates the child of the executable block 
            block.GetChild(0).gameObject.SetActive(true);
            Debug.Log("execute");
            yield return new WaitForSeconds(secondsToWait);
        }
       
        
        Debug.Log("execute");
    }
}
