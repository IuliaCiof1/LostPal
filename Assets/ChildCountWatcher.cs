using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Create an event for when the object to which is the script attached to, changes in child count
public class ChildCountWatcher : MonoBehaviour
{
    public delegate void OnSmallerChildCount(int newChildCount);

    public static event OnSmallerChildCount onSmallerChildCount;

    private int lastChildCount;
    
    void Start()
    {
        lastChildCount = transform.childCount;
    }
    
    void Update()
    {
        int currentChildCount = transform.childCount;
        //Debug.Log(currentChildCount+"  "+lastChildCount);
        if (currentChildCount < lastChildCount)
        {
            lastChildCount = currentChildCount;
           // Debug.Log("lalalalal");
            onSmallerChildCount?.Invoke(currentChildCount);
            //Debug.Log("lalalalal");
        }
        lastChildCount = currentChildCount;
    }
}
