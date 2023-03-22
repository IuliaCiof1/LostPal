using System;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Blocks.Mechanics
{
    public class DragObject : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
    
        private RectTransform draggingObjectRT;
        private Transform parentOfDraggingObject;
        private Image image; //used to enable and disable raycast;
        private RectTransform CodeEditor;
        private Transform CodeStorage;

        //Event for when a DragObject is dropped:
        public delegate void OnDropOnObject(int mode);  //mode = 1 if the object is dropped on an expandable block, otherwise it's 0
        public static event OnDropOnObject OnDropOnObjectSound;
  
    
        private void Awake()
        {
            draggingObjectRT = GetComponent<RectTransform>();
            image = GetComponent<Image>();
            CodeEditor = GameObject.FindGameObjectWithTag("CodeEditor").GetComponent<RectTransform>();
            CodeStorage = GameObject.FindGameObjectWithTag("CodeStorage").GetComponent<RectTransform>(); ;
        }
    
        //Events from UnityEngine.EventSystems
        public void OnBeginDrag(PointerEventData eventData)
        {
            OnDropOnObjectSound?.Invoke(0);
            parentOfDraggingObject = transform.parent;
        
            //If this block was contained in an Expandable block then unexpand every Expandable block in hierarchy
            Transform parent = transform.parent.parent;
            if (parent.CompareTag("ExpandableCodeBlock"))
            {
                UnexpandParentBlocks(parent.GetComponent<ExpandableDragObject>());
            }
              
            //Keep UI Element in front
            transform.SetParent(transform.root); //set parent to transform of the topmost object in hierarcy
            transform.SetAsLastSibling();
        }

    
        public void OnDrag(PointerEventData eventData)
        {
            draggingObjectRT.position = Input.mousePosition;  //Move object where mouse cursor is
            image.raycastTarget = false;
        }
    
    
        //A block will be parented to CodeEditor only if it fully overlaps with the CodeEditor, otherwise, it will be parented to CodeStorage
        public void OnEndDrag(PointerEventData eventData)
        {
            ExpandableDragObject expandableBlock;
        
            //Check if the dragged object overlaps with the code editor and parent it to CodeEditor
            if (RectOverlap(draggingObjectRT,CodeEditor)==0 && !ExpandableBlockOverlap(CodeEditor))
            {
                OnDropOnObjectSound?.Invoke(0);
                parentOfDraggingObject = CodeEditor;
                Debug.Log("parent to code editor");
            }
            //Check if the dragged object doesn't overlap with the code editor and parent it to CodeStorage
            else if (RectOverlap(draggingObjectRT, CodeEditor)!=0)
            {
                OnDropOnObjectSound?.Invoke(0);
                parentOfDraggingObject = CodeStorage;
                Debug.Log("storage");
                
                //When the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
                if (CompareTag("ExpandableCodeBlock"))
                {
                    Debug.Log("name of dragging object " + draggingObjectRT.name);
                    MoveNestedBlocksToCodeStorage(GetComponent<ExpandableDragObject>());
                    parentOfDraggingObject = CodeStorage;
                }
                
                Debug.Log("parent to code storage");

            }
            //If the object overlaps an ExpandableBlock, parent the object to it and expend it
            else if ((expandableBlock = ExpandableBlockOverlap(CodeEditor)) is not  null)
            {
                OnDropOnObjectSound?.Invoke(1);
                
                ExpandParentBlocks(expandableBlock);
                Transform snapPoint = expandableBlock.transform.Find("SnapPoint");
                parentOfDraggingObject = snapPoint; //snap the dropped object on this object
                
                Debug.Log("parent to snappoint");

            }
        
            transform.SetParent(parentOfDraggingObject);
            image.raycastTarget = true;
        }
    
        void MoveNestedBlocksToCodeStorage(ExpandableDragObject block)
        {
            Transform sp = block.transform.Find("SnapPoint");
        
            for(int i = sp.childCount-1; i>=0; i-- )
            {
                ExpandableDragObject nestedExpandBlock = sp.GetChild(i).GetComponent<ExpandableDragObject>();
            
                if (nestedExpandBlock is not null)
                {
                    MoveNestedBlocksToCodeStorage(nestedExpandBlock);
                    block.Unexpand(nestedExpandBlock.GetComponent<RectTransform>());
                    nestedExpandBlock.Reset();
                }
                else
                {
                    sp.GetChild(i).transform.SetParent(CodeStorage);
                }
            }
        
            block.Reset();
            block.transform.SetParent(CodeStorage);
        }
    
        void ExpandParentBlocks(ExpandableDragObject block)
        {
            block.Expand(draggingObjectRT);
            Transform sp = block.transform.parent;
           
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    ExpandParentBlocks(nestedExpandBlock);
                }
            }
        }
        
        void UnexpandParentBlocks(ExpandableDragObject block)
        {
            block.Unexpand(draggingObjectRT);
            Transform sp = block.transform.parent;
          
            if (sp is not null)
            {
                ExpandableDragObject nestedExpandBlock = sp.transform.parent.GetComponent<ExpandableDragObject>();
               
                if (nestedExpandBlock is not null)
                {
                    UnexpandParentBlocks(nestedExpandBlock);
                }
            }
        }
        
        
        //Function takes 2 rects as input and returns:
        //-1 if the 2 rectangles don't overlap,
        //0 if the area of a rect fully overlaps the other rect,
        //a number greater than 0 that represents the area overlapped
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
            Debug.Log($"{A} {(r1[1].y-r1[0].y) * (r1[1].x-r1[2].x)} {(r2[1].y-r2[0].y) * (r2[1].x-r2[2].x)}");

            //Return 0 if any rect fully overlaps with the other rect 
            if (A == Math.Abs((r1[1].y-r1[0].y) * (r1[1].x-r1[2].x)) || A == Math.Abs((r2[1].y-r2[0].y) * (r2[1].x-r2[2].x)))
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
        
        //Check if this block overlaps with any expandable block
        ExpandableDragObject ExpandableBlockOverlap(Transform container)
        {
            ExpandableDragObject [] blocks = container.GetComponentsInChildren<ExpandableDragObject>();
     
            for(int i=blocks.Length-1; i>=0; i--){
            
                if (blocks[i].CompareTag("ExpandableCodeBlock") && RectOverlap(draggingObjectRT, blocks[i].GetComponent<RectTransform>())>=0)
                {
                    return blocks[i].GetComponent<ExpandableDragObject>();
                }
            }

            return null;
        }
    }
}
