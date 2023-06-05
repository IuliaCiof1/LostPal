using System;
using System.Collections.Generic;
using System.Linq;
using Unity.Mathematics;
using UnityEditor;
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

        [SerializeField]  private float raycastSize=3;
    
        private void Awake()
        {
            //Handles.CircleHandleCap(0, Event.current.mousePosition, quaternion.identity,raycastSize,EventType.Repaint );

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
            // ExpandableDragObject expandableBlock;
            //
            // // PointerEventData pointerEventData = new PointerEventData(EventSystem.current);
            // // pointerEventData.position = draggingObjectRT.position;
            // // pointerEventData.radius = draggingObjectRT.sizeDelta;
            // //
            // // Debug.Log(pointerEventData.position + " " +
            // // pointerEventData.radius);
            // List <RaycastResult> raycastResults = new List<RaycastResult>();
            // EventSystem.current.RaycastAll(eventData, raycastResults);
            // // for (int i = 0; i < raycastResults.Count; i++)
            // // {
            // //     Debug.Log(raycastResults.ElementAt(i).gameObject.name);
            // // }
            //
            //
            //
            //
            // int newIndex=-1;
            // Debug.Log("-------------------------");
            //
            // int editorOverlap = RectOverlap(draggingObjectRT, CodeEditor); 
            // Debug.Log(editorOverlap);
            // //Check if the dragged object overlaps with the code editor and parent it to CodeEditor
            // if (editorOverlap>=99 && ExpandableBlockOverlap(CodeEditor) is null)
            // {
            //     OnDropOnObjectSound?.Invoke(0);
            //     parentOfDraggingObject = CodeEditor;
            //   Debug.Log("parent to code editor");
            // }
            // //Check if the dragged object doesn't overlap with the code editor and parent it to CodeStorage
            // else if (editorOverlap<99)
            // {
            //     OnDropOnObjectSound?.Invoke(0);
            //     parentOfDraggingObject = CodeStorage;
            //    Debug.Log("storage");
            //     
            //     //When the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
            //     if (CompareTag("ExpandableCodeBlock"))
            //     {
            //         Debug.Log("name of dragging object " + draggingObjectRT.name);
            //         MoveNestedBlocksToCodeStorage(GetComponent<ExpandableDragObject>());
            //         parentOfDraggingObject = CodeStorage;
            //     }
            //     
            //   //  Debug.Log("parent to code storage");
            //
            // }
            //If the object overlaps an ExpandableBlock, parent the object to it and expend it
            // else if ((expandableBlock = ExpandableBlockOverlap(CodeEditor)) 
            //          is not  null && ((expandableBlock.name.CompareTo("Player") == 0 &&
            //              RectOverlap(draggingObjectRT, expandableBlock.GetComponent<RectTransform>()) >0)
            //          || (RectOverlap(draggingObjectRT, expandableBlock.GetComponent<RectTransform>()) >= 70)))
            // {
            //     Debug.Log("expandableblock " + expandableBlock.name);
            //     // if ((expandableBlock.name.CompareTo("Player") == 0 &&
            //     //      RectOverlap(draggingObjectRT, expandableBlock.GetComponent<RectTransform>()) >0)
            //     //     || (RectOverlap(draggingObjectRT, expandableBlock.GetComponent<RectTransform>()) >= 70))
            //     // {
            //         OnDropOnObjectSound?.Invoke(1);
            //
            //         ExpandParentBlocks(expandableBlock);
            //         Transform snapPoint = expandableBlock.transform.Find("SnapPoint");
            //         parentOfDraggingObject = snapPoint; //snap the dropped object on this object
            //
            //         Debug.Log("parent to snappoint");
            //     //}
            //
            // }
            // else{
            //     Debug.Log(parentOfDraggingObject.name);
            //     List<DragObject> overlappedBlocks = GetOverlapBlocks(CodeEditor);
            //     Debug.Log("count  = " + overlappedBlocks.Count);
            //     
            //     if (overlappedBlocks.Count == 2)
            //     {
            //         newIndex = overlappedBlocks.ElementAt(0).gameObject.transform.GetSiblingIndex();
            //         //transform.SetSiblingIndex(newIndex);
            //     }
            //     else if (overlappedBlocks.Count == 1)
            //     {
            //         if (overlappedBlocks[0].transform.position.y < transform.position.y)
            //             newIndex = overlappedBlocks.ElementAt(0).gameObject.transform.GetSiblingIndex();
            //         else
            //             newIndex = overlappedBlocks.ElementAt(0).gameObject.transform.GetSiblingIndex() + 1;
            //
            //         //transform.SetSiblingIndex(newIndex);
            //         Debug.Log("count 1 index = " + newIndex);
            //     }
            //
            //     if (overlappedBlocks.ElementAt(0).gameObject.transform.parent != parentOfDraggingObject)
            //     {
            //         parentOfDraggingObject = overlappedBlocks.ElementAt(0).gameObject.transform.parent;
            //         Debug.Log("parent = " + parentOfDraggingObject);
            //         if(parentOfDraggingObject.parent.GetComponent<ExpandableDragObject>() is not null)
            //             parentOfDraggingObject.parent.GetComponent<ExpandableDragObject>().Expand(draggingObjectRT);
            //     }
            //     // transform.SetParent(parentOfDraggingObject);
            //     // transform.SetSiblingIndex(newIndex);
            // }
            // Debug.Log("parent = " + parentOfDraggingObject);
            // transform.SetParent(parentOfDraggingObject);
            // transform.SetSiblingIndex(newIndex);
            // image.raycastTarget = true;

            /////////////////////////////////////////////

            //List<DragObject> overlappedBlocks = GetOverlapBlocks(CodeEditor);
            //DragObject firstOverlapBlock = overlappedBlocks.ElementAt(0);

            int editorOverlap = RectOverlap(draggingObjectRT, CodeEditor);
            if (editorOverlap >= 99) //the block was dropped on CodeEditor
            {
                OnDropOnObjectSound?.Invoke(0);

                ExpandableDragObject expandableBlock = null;

                if ((expandableBlock = ExpandableBlockOverlap(CodeEditor)) is not null)  //the block was dropped on an Expandable block
                {     //blocks can be ordered only if they are in an Expandable block
                    
                    List<DragObject> overlappedBlocks = GetOverlapBlocks(CodeEditor);  //get the blocks under this block, they must be siblings, the parent block is excluded
                    
                    Transform snapPoint = expandableBlock.transform.Find("SnapPoint");
                    int newIndex = snapPoint.childCount;
                    
                    if (overlappedBlocks.Count == 2) //there are 2 blocks underneath. Move this block between those 2
                    {
                        newIndex = overlappedBlocks.ElementAt(0).transform.GetSiblingIndex() + 1;
                        parentOfDraggingObject = overlappedBlocks.ElementAt(0).transform.parent;
                        ExpandParentBlocks(expandableBlock);
                    
                    }
                    else if (overlappedBlocks.Count == 1) //there is only 1 block underneath
                    {
                        ExpandableDragObject expandableBlockInside;
                        
                        if ((expandableBlockInside = overlappedBlocks.ElementAt(0).GetComponent<ExpandableDragObject>()) is not null &&
                        RectOverlap(expandableBlockInside.GetComponent<RectTransform>(), draggingObjectRT) >= 65) //check if that 1 block is Expandable and that it overlaps with this block 65% or more
                        {  //place this block inside that 1 block
                            OnDropOnObjectSound?.Invoke(1);

                            ExpandParentBlocks(expandableBlockInside);
                            snapPoint = expandableBlockInside.transform.Find("SnapPoint");
                            parentOfDraggingObject = snapPoint;
                        }
                        else //otherwise we want to place this block above or below that block
                        {
                            if (overlappedBlocks[0].transform.position.y < transform.position.y) //place it above the block
                            {
                                newIndex = overlappedBlocks.ElementAt(0).transform.GetSiblingIndex();
                            }
                            else  //place it below the block
                                newIndex = overlappedBlocks.ElementAt(0).transform.GetSiblingIndex() + 1;

                            parentOfDraggingObject = overlappedBlocks.ElementAt(0).transform.parent;
                            
                            ExpandParentBlocks(parentOfDraggingObject.parent.GetComponent<ExpandableDragObject>());

                        }
                    }
                    else //there are no blocks to order, the expandable block is empty
                    {
                        OnDropOnObjectSound?.Invoke(1);

                        ExpandParentBlocks(expandableBlock);
                        
                        parentOfDraggingObject = snapPoint;
                    }


                    // if (transform.parent != overlappedBlocks.ElementAt(0).transform.parent)
                    //     parentOfDraggingObject.parent.GetComponent<ExpandableDragObject>().Expand(draggingObjectRT);

                    transform.SetParent(parentOfDraggingObject);
                    transform.SetSiblingIndex(newIndex);
                }
                else  //if the this block is not placed in in an expandable block just parent it ti CodeEditor
                {
                    parentOfDraggingObject = CodeEditor;
                    transform.SetParent(parentOfDraggingObject);
                }
                
            }
            else if (editorOverlap < 99)  //the block was not fully dropped in CodeEditor, the block goes to CodeStorage
            {
                OnDropOnObjectSound?.Invoke(0);
                parentOfDraggingObject = CodeStorage;


                //When the expandable block returns to CodeStorage, unparent all the contained blocks from this expandable block
                if (CompareTag("ExpandableCodeBlock"))
                {
                    MoveNestedBlocksToCodeStorage(GetComponent<ExpandableDragObject>());
                    parentOfDraggingObject = CodeStorage;
                }

                transform.SetParent(parentOfDraggingObject);

            }
            
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
        //the percentage of the overlapping
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
//            Debug.Log($"{A} {(r1[1].y-r1[0].y) * (r1[1].x-r1[2].x)} {(r2[1].y-r2[0].y) * (r2[1].x-r2[2].x)}");

            //Return 0 if any rect fully overlaps with the other rect 
            // if (A == Math.Abs((r1[1].y-r1[0].y) * (r1[1].x-r1[2].x)) || A == Math.Abs((r2[1].y-r2[0].y) * (r2[1].x-r2[2].x)))
            // {
            //     return 0;
            // }
        
            //Return -1 if the rectangles don't overlap
            if (A == 0)
            {
                return -1;
            }
        
            Debug.Log("precentage = "+(int)((Math.Round(A/(rect1.rect.width * rect1.rect.height)))*100));
            return (int)((Math.Round(A)/(rect1.rect.width * rect1.rect.height))*100);
        }
        
        //Check if this block overlaps with any expandable block
        ExpandableDragObject ExpandableBlockOverlap(Transform container)
        {
            ExpandableDragObject [] blocks = container.GetComponentsInChildren<ExpandableDragObject>();
            Debug.Log("expandable under");
            for(int i=blocks.Length-1; i>=0; i--){
                Debug.Log("blocks["+i+"]= " + blocks[i].name);
                if (blocks[i].CompareTag("ExpandableCodeBlock"))
                {
                    // if(blocks[i].name.CompareTo("Player")==0 && RectOverlap(draggingObjectRT, blocks[i].GetComponent<RectTransform>())>=30)
                    //     return blocks[i].GetComponent<ExpandableDragObject>();
                    // else if(RectOverlap(draggingObjectRT, blocks[i].GetComponent<RectTransform>())>=60)
                    if(RectOverlap(draggingObjectRT, blocks[i].GetComponent<RectTransform>())>0)

                        return blocks[i].GetComponent<ExpandableDragObject>();
                }
            }
            Debug.Log("null");
            return null;
        }

        //Get the blocks under this block. Those blocks need to have the same parent
        public List<DragObject> GetOverlapBlocks(Transform container)
        {
            DragObject [] blocks = container.GetComponentsInChildren<DragObject>();
            List<DragObject> overlappedBlocks = new List<DragObject>();

            for(int i=0; i<blocks.Length; i++){

                DragObject child = blocks[i];
                
                if (child.transform!=transform && child.name!="Player" && ((child.CompareTag("CodeBlock") || child.CompareTag("ExpandableCodeBlock")) && RectOverlap(draggingObjectRT, child.GetComponent<RectTransform>())>0))
                {
                    overlappedBlocks.Add(child);
                }
            }

            //remove unwanted blocks (parents)
            for (int i=0; i<overlappedBlocks.Count-1; i++)
            {
                Debug.Log("block "+ overlappedBlocks.ElementAt(i).transform.name+" next block parent is "+ overlappedBlocks.ElementAt(i + 1).transform.parent);
                if (overlappedBlocks.ElementAt(i).transform.parent != overlappedBlocks.ElementAt(i + 1).transform.parent.parent)
                {
                    overlappedBlocks.RemoveAt(i);
                    i--;
                }
            }

            return overlappedBlocks;
        }
    }
}
