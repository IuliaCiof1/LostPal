using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    
    private RectTransform draggingObject; //Transform Image
    public Transform parentOfDraggingObject;
    private Image image;
    private RectTransform CodeEditor;
    private Transform CodeStorage;

    private ExpandableDragObject expandableBlock;
    
    
    //Event for when a DragObject is dropped:
    //mode = 1 - dropped on an ExpandableDragObject, mode = 0 -> dropped on something else 
    public delegate void OnDropOnObject(int mode);
    public static event OnDropOnObject OnDropOnObjectSound;
  
    
    private void Awake()
    {
        draggingObject = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        CodeEditor = GameObject.FindGameObjectsWithTag("CodeEditor")[0].GetComponent<RectTransform>();
        CodeStorage = transform.parent; ;
    }
    
    //Events from UnityEngine.EventSystems
    public void OnBeginDrag(PointerEventData eventData)
    {
      
        OnDropOnObjectSound?.Invoke(0);
        parentOfDraggingObject = transform.parent;
        
        //If this block was contained in an Expandable block then unexpand the Expandable block and other parents that are expandable blocks
        if (transform.parent.parent.CompareTag("ExpandableCodeBlock"))
        {
            Transform parent = transform.parent.parent;
            while (parent.TryGetComponent(out ExpandableDragObject expandable))
            {
                expandable.Unexpand(draggingObject);
                parent = parent.parent.parent;
            }
        }
        
        //Keep UI Element in front
        transform.SetParent(transform.root); //set parent to transform of the topmost object in hierarcy
        transform.SetAsLastSibling();
    }

    
    public void OnDrag(PointerEventData eventData)
    {
        draggingObject.position = Input.mousePosition;  //Move object where mouse cursor is
        image.raycastTarget = false;
    }
    
    
    //A block will be parented to CodeEditor only if it fully overlaps with the CodeEditor, otherwise, it will be parented to CodeStorage
    public void OnEndDrag(PointerEventData eventData)
    {
        if (RectOverlap(draggingObject,CodeEditor)==0 && !ExpandableBlockOverlap())
        {
            OnDropOnObjectSound?.Invoke(0);
            parentOfDraggingObject = CodeEditor;
        }
        else if (RectOverlap(draggingObject, CodeEditor)!=0)
        {
            OnDropOnObjectSound?.Invoke(0);
            parentOfDraggingObject = CodeStorage;

            //When the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
            if (CompareTag("ExpandableCodeBlock"))
            {
                UnexpandNestedBlocks(GetComponent<ExpandableDragObject>());
            }
        }

        //If the object overlaps an ExpandableBlock, parent the object to it and expend it
        if (ExpandableBlockOverlap())
        {
            OnDropOnObjectSound?.Invoke(1);
            
            expandableBlock.Expand(draggingObject);
            Transform snapPoint = expandableBlock.transform.Find("SnapPoint");
            parentOfDraggingObject = snapPoint; //snap the dropped object on this object
        }
        
        transform.SetParent(parentOfDraggingObject);
        image.raycastTarget = true;
    }
    
    void UnexpandNestedBlocks(ExpandableDragObject block)
    {
        Transform sp = block.transform.Find("SnapPoint");
        // expandableBlock = block.GetComponent<ExpandableDragObject>();
        Debug.Log("start over. children: " + sp.childCount+ " name: " + block.name);
        for(int i = sp.childCount-1; i>=0; i-- )
        {
            sp = block.transform.Find("SnapPoint");
            expandableBlock = block.GetComponent<ExpandableDragObject>();
            ExpandableDragObject nestedExpandBlock;
            nestedExpandBlock = sp.GetChild(i).GetComponent<ExpandableDragObject>();
            if (nestedExpandBlock is not null)
            {
                Debug.Log("the block has an expandable block named: "+nestedExpandBlock.name);
                UnexpandNestedBlocks(nestedExpandBlock);
                Debug.Log(block.name);
                block.Unexpand(nestedExpandBlock.GetComponent<RectTransform>());
                nestedExpandBlock.Reset();
                block.Reset();
                //nestedExpandBlock.transform.SetParent(CodeStorage);
            }
            else
            {
                Debug.Log("continue to unexpand this block named: " + expandableBlock.name);
                //UnexpandAllExpandableParents(block);
                //nestedExpandBlock.Reset();
                block.Reset();
                //expandableBlock.Unexpand(sp.GetChild(i).GetComponent<RectTransform>());
                sp.GetChild(i).transform.SetParent(CodeStorage);
                
            }
        }
        
        block.transform.SetParent(CodeStorage);
    }
    

    // void UnexpandNestedBlocks(ExpandableDragObject block)
    // {
    //      Transform sp = block.transform.Find("SnapPoint");
    //     // expandableBlock = block.GetComponent<ExpandableDragObject>();
    //     Debug.Log("start over. children: " + sp.childCount+ " name: " + block.name);
    //     for(int i = sp.childCount-1; i>=0; i-- )
    //     {
    //         sp = block.transform.Find("SnapPoint");
    //         expandableBlock = block.GetComponent<ExpandableDragObject>();
    //         ExpandableDragObject nestedExpandBlock;
    //         nestedExpandBlock = sp.GetChild(i).GetComponent<ExpandableDragObject>();
    //         if (nestedExpandBlock is not null)
    //         {
    //             Debug.Log("the block has an expandable block named: "+nestedExpandBlock.name);
    //             UnexpandNestedBlocks(nestedExpandBlock);
    //             Debug.Log(block.name);
    //             block.Unexpand(nestedExpandBlock.GetComponent<RectTransform>());
    //             //nestedExpandBlock.transform.SetParent(CodeStorage);
    //         }
    //         else
    //         {
    //             Debug.Log("continue to unexpand this block named: " + expandableBlock.name);
    //             UnexpandAllExpandableParents(block);
    //             expandableBlock.Unexpand(sp.GetChild(i).GetComponent<RectTransform>());
    //             sp.GetChild(i).transform.SetParent(CodeStorage);
    //             
    //         }
    //     }
    //     
    //     block.transform.SetParent(CodeStorage);
    // }

    public void UnexpandAllExpandableParents(ExpandableDragObject block)
    {
        //ExpandableDragObject expandable;
        Transform sp = block.transform.Find("SnapPoint");

        foreach (Transform child in sp)
        {
            if (child.tag == "ExpandableCodeBlock")
            {
                
            }
        }
        //Debug.Log(block.transform.parent.TryGetComponent<ExpandableDragObject>(out ExpandableDragObject expandable));
        Debug.Log(block.name + block.transform.parent.name);
        if (block.transform.parent.TryGetComponent<ExpandableDragObject>(out ExpandableDragObject expandable))
        {
            expandable.Unexpand(expandable.transform.Find("SnapPoint").GetChild(0).GetComponent<RectTransform>());
            Debug.Log("asdf");
            UnexpandAllExpandableParents(expandable);
        }
        //ExpandableDragObject expandable = parent.GetComponent<ExpandableDragObject>();
        // while (expandable is not null)
        // {
        //     
        //     expandable.Unexpand(expandable.transform.Find("SnapPoint").GetChild(0).GetComponent<RectTransform>());
        //     //parent = expandable.transform.parent.parent;
        //     //if(parent is not null && (parent.GetComponent<ExpandableDragObject>()))
       // expandable = block.transform.parent.parent.GetComponent<ExpandableDragObject>();
        // }
    }
    
    public int RectOverlap(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] r1 = new Vector3[4];
        rect1.GetWorldCorners(r1);

        Vector3[] r2 = new Vector3[4];
        rect2.GetWorldCorners(r2);
        
        
        /*
        r1[1]               r1[2]
            ___________________
           |                  |
           |__________________|
        r1[0]               r1[3]
        */
        

        //Calculate the width and height of the rectangle (intersection) resulted from overlapping rect1 with rect2
        float X = Math.Min(r1[3].x, r2[3].x) - Math.Max(r1[1].x, r2[1].x);

        float Y = Math.Min(r1[1].y, r2[1].y) - Math.Max(r1[3].y, r2[3].y);
            
        //If the rectangles don't overlap, the X or Y above will result in a negative number.
        //Because of this, we need to make sure the area will be equal to 0 in case of no overlap.
        X = Math.Max(0, X);
        Y = Math.Max(0, Y);
        
        float A = X * Y; //calculate the intersecting area
        
        //Return 0 if any rect fully overlaps with the other rect 
        if (A == rect2.sizeDelta.x * rect2.sizeDelta.y || A == rect1.sizeDelta.x * rect1.sizeDelta.y)
        {
            return 0;
        }
        
        //Return -1 if the rectangles don't overlap
        if (A == 0)
        {
            return -1;
        }
        
        return (int)Math.Round(A);
    }

    //Check if any code block overlaps with this code block
    bool ExpandableBlockOverlap()
    { 
        
        foreach (Transform block in CodeEditor.transform)
        {
            if (block.CompareTag("ExpandableCodeBlock") && RectOverlap(draggingObject, block as RectTransform)>=0)
            {
                expandableBlock = block.GetComponent<ExpandableDragObject>();
                return true;
            }
        }

        return false;
    }

    // private void OnDrawGizmos()
    // {
    //     Gizmos.color=Color.blue;
    //     Gizmos.DrawWireCube(new Vector3(draggingObject.position.x, draggingObject.position.y, 1), new Vector3(draggingObject.sizeDelta.x, draggingObject.sizeDelta.y));
    //     Debug.Log($"{draggingObject.position.x} {draggingObject.position.y} ");
    //     
    //     Gizmos.color=Color.red;
    //     Gizmos.DrawWireCube(new Vector3(draggingObject.localPosition.x, draggingObject.localPosition.y, 1), new Vector3(draggingObject.sizeDelta.x, draggingObject.sizeDelta.y));
    //     Debug.Log($"{draggingObject.localPosition.x} {draggingObject.localPosition.y} ");
    //     
    //     Debug.Log($"{draggingObject.sizeDelta.x} {draggingObject.rect.width} ");
    // }


}
