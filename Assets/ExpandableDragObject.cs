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
    public void Start()
    {
        rt = gameObject.GetComponent<RectTransform>();
        middle = transform.Find("Middle") as RectTransform;
        bottom = transform.Find("Bottom") as RectTransform;
        snapPoint = transform.Find("SnapPoint") as RectTransform;
        
        ChildCountWatcher.onSmallerChildCount += HandleSmallerChildCount;
    }
    
    void HandleSmallerChildCount(int newChildCount)
    {
       //  DragObject draggableObject = objectDropped.GetComponent<DragObject>();
       //  draggableObject.parentOfDraggingObject = transform.parent.parent;
       // //Unexpand();
    }
    // public void OnDrop(PointerEventData eventData)
    // {
    //     if (transform.parent.CompareTag("CodeEditor"))
    //     {
    //         objectDropped = eventData.pointerDrag;
    //
    //         addHeight = objectDropped.GetComponent<RectTransform>().sizeDelta.y +
    //                     snapPoint.GetComponent<GridLayoutGroup>().spacing.y;
    //         //GridLayoutGroup component of CodeStorage automatically decides pivot position of the contained objects
    //         //We need the pivot present at the middle top so that the expandable object only expands downwards
    //         gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);
    //
    //         middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y + addHeight);
    //         bottom.position = new Vector2(bottom.position.x, bottom.position.y - addHeight);
    //         //bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y - addHeight);
    //
    //         //rt.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight - snapPoint.sizeDelta.y);
    //         snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight);
    //         rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + addHeight);
    //
    //         //Get the gameobjects that was dropped on this gameobject.
    //         //For this to work we need to disable Raycast target of the dragged object while it's being dragged and enable
    //         //it when it is dropped.
    //         //GameObject objectDropped = eventData.pointerDrag;
    //         DragObject draggableObject = objectDropped.GetComponent<DragObject>();
    //         draggableObject.parentOfDraggingObject = snapPoint.transform; //snap the dropped object on this object
    //
    //     }
    // }

    public void Expand(RectTransform draggingObject)
    {
        addHeight = draggingObject.GetComponent<RectTransform>().sizeDelta.y +
                    snapPoint.GetComponent<GridLayoutGroup>().spacing.y;
        //GridLayoutGroup component of CodeStorage automatically decides pivot position of the contained objects
        //We need the pivot present at the middle top so that the expandable object only expands downwards
        gameObject.GetComponent<RectTransform>().pivot = new Vector2(0.5f, 1f);

        middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y + addHeight);
        bottom.position = new Vector2(bottom.position.x, bottom.position.y - addHeight);
        //bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y - addHeight);

        //rt.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight - snapPoint.sizeDelta.y);
        snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y + addHeight);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y + addHeight);
    }
    public void Unexpand()
    {
        middle.sizeDelta = new Vector2(middle.sizeDelta.x, middle.sizeDelta.y - addHeight);
        bottom.localPosition = new Vector2(bottom.localPosition.x, bottom.localPosition.y + addHeight);
        
        snapPoint.sizeDelta = new Vector2(snapPoint.sizeDelta.x, snapPoint.sizeDelta.y - addHeight);
        rt.sizeDelta = new Vector2(rt.sizeDelta.x, rt.sizeDelta.y-addHeight);
    }
}
