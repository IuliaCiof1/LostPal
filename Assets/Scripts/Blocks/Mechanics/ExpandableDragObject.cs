using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ExpandableDragObject : MonoBehaviour
{
    private float addHeight;

    private RectTransform rt;

    private RectTransform middle;

    private RectTransform bottom;
    
    private RectTransform snapPoint;
    private GameObject objectDropped;
    
    private Vector2 rtInitialSize;

    private Vector2 middleInitialSize;

    private Vector2 bottomInitialLocation;
    
    private Vector2 snapPointInitialSize;

    public void Awake()
    {
        rt = gameObject.GetComponent<RectTransform>();
        middle = transform.Find("Middle") as RectTransform;
        bottom = transform.Find("Bottom") as RectTransform;
        snapPoint= transform.Find("SnapPoint") as RectTransform;

        rtInitialSize=rt.sizeDelta;
        middleInitialSize=middle.sizeDelta;
        bottomInitialLocation=bottom.localPosition;
        snapPointInitialSize=snapPoint.sizeDelta;
    }

    public void Expand(RectTransform draggingObject)
    {
        // GridLayoutGroup gd = GetComponent<GridLayoutGroup>();
        // gd.cellSize

        addHeight = draggingObject.GetComponent<RectTransform>().sizeDelta.y+
                    snapPoint.GetComponent<VerticalLayoutGroup>().spacing;
        //GridLayoutGroup component of CodeStorage automatically decides pivot position of the contained objects
        //We need the pivot present at the middle top so that the expandable object only expands downwards
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

        middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y + addHeight);
        //bottom.position = new Vector2(bottom.position.x, bottom.position.y - addHeight);
        bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y - addHeight);

        //rt.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight - snapPoint.sizeDelta.y);
        snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + addHeight);
    }
    public void Unexpand(RectTransform draggingObject)
    {
        addHeight = draggingObject.GetComponent<RectTransform>().sizeDelta.y+snapPoint.GetComponent<VerticalLayoutGroup>().spacing;

        middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y - addHeight);
        bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y + addHeight);
        
        snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y - addHeight);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y-addHeight);
        
        Debug.Log("unexpend " + draggingObject.name);
    }

    public void Reset()
    {
        middle.sizeDelta = middleInitialSize;
        bottom.localPosition = bottomInitialLocation;

        snapPoint.sizeDelta = snapPointInitialSize;
        rt.sizeDelta = rtInitialSize;
    }
}
