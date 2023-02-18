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

    //private Vector3[] edgePoints;
    public int range;

    private AudioSource audioSource;
    [SerializeField] private AudioClip dragClip;
    [SerializeField] private AudioClip inBlockClip;
    //private bool inRange;

    [SerializeField] private RectTransform expandableBlock;
    private void Awake()
    {
        draggingObject = GetComponent<RectTransform>();
        image = GetComponent<Image>();
        CodeEditor = GameObject.FindGameObjectsWithTag("CodeEditor")[0].GetComponent<RectTransform>();
        CodeStorage = transform.parent;

        audioSource = GetComponent<AudioSource>();
        // ChildCountWatcher.OnSmallerChildCount += HandleSmallerChildCount();
    }

    private void FixedUpdate()
    {
        RaycastHit hit;

        if (Physics.CheckSphere(transform.position, range))
        {
            //Debug.Log("in range");
        }
    }

    // private void HandleSmallerChildCount()
    // {
    //     parentOfDraggingObject = CodeEditor;
    // }
    
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position,range);
    }

    //Events from UnityEngine.EventSystems
    public void OnBeginDrag(PointerEventData eventData)
    {
        audioSource.PlayOneShot(dragClip);
        parentOfDraggingObject = transform.parent;

        //check if this code block is in an expandable block of code
        // if (draggingObject.parent.CompareTag("ExpandableCodeBlock"))
        // {
        //     ExpandableDragObject parent = draggingObject.GetComponentInParent<ExpandableDragObject>();
        //     parent.Unexpand();
        // }

        //If this block was contained in an Expandable block then unexpand the Expandable block
        if (transform.parent.parent.CompareTag("ExpandableCodeBlock"))
        {
            transform.parent.parent.GetComponent<ExpandableDragObject>().Unexpand();
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
    
    
    //A block will be parented to CodeStorage if it doesn't fully overlap with the CodeEditor, otherwise, it will be parented to CodeStorage
    public void OnEndDrag(PointerEventData eventData)
    {
        //draggingObject.pivot = new Vector2(0.5f, 1f);
  
        if (rectFullyOverlaps(draggingObject, CodeEditor) && !ExpandableBlockOverlap())
        {
            audioSource.PlayOneShot(dragClip);
            parentOfDraggingObject = CodeEditor;
            //transform.SetParent(CodeEditor.transform);
        }
        else if (!rectFullyOverlaps(draggingObject, CodeEditor))
        {
            audioSource.PlayOneShot(dragClip);
            parentOfDraggingObject = CodeStorage;

            //when the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
            if (CompareTag("ExpandableCodeBlock"))
            {
                Transform sp = transform.Find("SnapPoint");
                
                for(int i = sp.childCount-1; i>=0; i-- )
                {
                    gameObject.GetComponent<ExpandableDragObject>().Unexpand();
                    sp.GetChild(i).transform.SetParent(CodeStorage);
                    Debug.Log("alalss");
                    
                }
            }
            //draggingObject.pa = CodeStorage.transform;
            //transform.SetParent(CodeStorage); //revert to initial hierarcy
        }

        if (ExpandableBlockOverlap())
        {
            audioSource.PlayOneShot(inBlockClip);
            
            expandableBlock.GetComponent<ExpandableDragObject>().Expand(draggingObject);
            RectTransform snapPoint = expandableBlock.transform.Find("SnapPoint") as RectTransform;
            parentOfDraggingObject = snapPoint.transform; //snap the dropped object on this object

            
        }
        
        //else if(rectOverlaps(draggingObject,eventData.pointerCurrentRaycast))
            transform.SetParent(parentOfDraggingObject);
            
        

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
    bool ExpandableBlockOverlap()
    { 
        
        foreach (Transform block in CodeEditor.transform)
        {
            Debug.Log(CodeEditor.transform.childCount);
            if (block.CompareTag("ExpandableCodeBlock") && rectOverlaps(draggingObject, block as RectTransform))
            {
                expandableBlock = block.GetComponent<RectTransform>();
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
