using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class RepeatBlocks : MonoBehaviour
{
    [SerializeField] private TMP_InputField inputNumber;
    [SerializeField] private float secondsToWait;
    [SerializeField] private Animator anim;
    public void OnEnable()
    {
        StartCoroutine(Repeat());
    }

    IEnumerator Repeat()
    {
        Transform SnapPoint = transform.parent.Find("SnapPoint");

        int i;
        
        for (i = 0; i < int.Parse(inputNumber.text); i++)
        {
            for (int j = 0; j < SnapPoint.childCount; j++)
            {
                Transform block = SnapPoint.GetChild(j);
              
                block.GetChild(0).gameObject.SetActive(true);

                yield return new WaitUntil(()=>!block.GetChild(0).gameObject.activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                yield return new WaitForSeconds(secondsToWait); //wait until animation ends
                //yield return new WaitForSeconds(secondsToWait); //wait to finish animation
            }
            //yield return new WaitForSeconds(secondsToWait);
        }

        if (i == int.Parse(inputNumber.text))
        {
            gameObject.SetActive(false);
        }

    }
    
    
}
