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
    //private Image image;
    public Transform parentOfDraggingObject;
    private Image image;
    private RectTransform CodeEditor;
    private Transform CodeStorage;

    private Vector3[] edgePoints;
    public int range;

    private bool inRange;
    private void Awake()
    {
        draggingObject = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        CodeEditor = GameObject.FindGameObjectsWithTag("Editor")[0].GetComponent<RectTransform>();
        CodeStorage = transform.parent;
        
        
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.CheckSphere(transform.position, range))
        {
            Debug.Log("in range");
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }

    //Events from UnityEngine.EventSystems
    public void OnBeginDrag(PointerEventData eventData)
    {
        parentOfDraggingObject = transform.parent;

        //check if this code block is in an expandable block of code
        if (draggingObject.parent.CompareTag("ExpandableCodeBlock"))
        {
            ExpandableDragObject parent = draggingObject.GetComponentInParent<ExpandableDragObject>();
            parent.Unexpand();
        }
        
        //Keep UI Element in front
        transform.SetParent(transform.root); //set parent to transform of the topmost object in hierarcy
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
       
        
        draggingObject.position = Input.mousePosition;  //copy mouse position to object position
        image.raycastTarget = false;

    }

    public void OnEndDrag(PointerEventData eventData)
    {
        //draggingObject.pivot = new Vector2(0.5f, 1f);
  
        if (rectFullyOverlaps(draggingObject, CodeEditor) && !CodeBlockOverlap())
        {
            transform.SetParent(CodeEditor.transform);
        }
        else if (inRange|| rectFullyOverlaps(draggingObject, CodeEditor) && CodeBlockOverlap())
        {
            transform.SetParent(parentOfDraggingObject);
        }
        //else if(rectOverlaps(draggingObject,eventData.pointerCurrentRaycast))
        else
        {
            //parentOfDraggingObject = CodeStorage;
            //draggingObject.pa = CodeStorage.transform;
            transform.SetParent(CodeStorage); //revert to initial hierarc
        }

        image.raycastTarget = true;
    }
    
    public bool rectFullyOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] cornersCodeBlock = new Vector3[4];
        rect1.GetWorldCorners(cornersCodeBlock);

        Vector3[] cornersCodeEditor = new Vector3[4];
        rect2.GetWorldCorners(cornersCodeEditor);

        if (cornersCodeEditor[1].x < cornersCodeBlock[3].x && cornersCodeEditor[3].x > cornersCodeBlock[1].x &&
            cornersCodeEditor[1].y > cornersCodeBlock[3].y && cornersCodeEditor[3].y < cornersCodeBlock[1].y &&
            cornersCodeEditor[1].x < cornersCodeBlock[1].x && cornersCodeEditor[1].y > cornersCodeBlock[1].y &&
            cornersCodeEditor[3].x > cornersCodeBlock[3].x && cornersCodeEditor[3].y < cornersCodeBlock[3].y)
            return true;

        return false;
    }
    
    public bool rectOverlaps(RectTransform rect1, RectTransform rect2)
    {
        Vector3[] cornersCodeBlock = new Vector3[4];
        rect1.GetWorldCorners(cornersCodeBlock);

        Vector3[] cornersCodeEditor = new Vector3[4];
        rect2.GetWorldCorners(cornersCodeEditor);

        if (cornersCodeEditor[1].x < cornersCodeBlock[3].x && cornersCodeEditor[3].x > cornersCodeBlock[1].x &&
            cornersCodeEditor[1].y > cornersCodeBlock[3].y && cornersCodeEditor[3].y < cornersCodeBlock[1].y)
            return true;

        return false;
    }
    
    //Check if 2 Canvas RectTransforms overlap
    // bool rectOverlaps(RectTransform rectTrans1, RectTransform rectTrans2)
    //     {
    //
    //         //Convert coordinates of the first Canvas RectTransform to World Rects
    //         Vector3[] corners = new Vector3[4];
    //         rectTrans1.GetWorldCorners(corners);
    //         //Vector3 a = transform.TransformPoint();
    //         //Actually convert the first Canvas RectTransforms to World Rects
    //         Rect rect1 = new Rect(corners[0].x,corners[0].y, rectTrans1.rect.width, rectTrans1.rect.height);
    //         
    //         //Convert coordinates of the second Canvas RectTransform to World Rects
    //         corners = new Vector3[4];
    //         rectTrans2.GetWorldCorners(corners);
    //         
    //         float paddingX = rectTrans1.rect.width;
    //         float paddingY = rectTrans1.rect.height;
    //         
    //         //Actually convert the second Canvas RectTransforms to World Rects
    //         Rect rect2 = new Rect(corners[0].x,corners[0].y, rectTrans2.rect.width, 
    //             rectTrans2.rect.height);
    //
    //         
    //         return rect1.Overlaps(rect2);
    //     }

    //Check if any code block overlaps with this code block
    bool CodeBlockOverlap()
    { 
        
        foreach (Transform block in CodeEditor.transform)
        {
            Debug.Log(CodeEditor.transform.childCount);
            if (rectOverlaps(draggingObject, block as RectTransform))
                return true;
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
