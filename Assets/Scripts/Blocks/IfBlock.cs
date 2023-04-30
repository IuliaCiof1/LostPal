using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;
using UnityEngine.UI;

namespace Blocks
{
    public class IfBlock : MonoBehaviour
    {
        [SerializeField] private TMP_Dropdown conditionDropdown;
        [SerializeField] private Rigidbody2D player;
        private Transform snapPoint;

        private float secondsToWait = 0.4f;

        private bool fail;

        [SerializeField] private Tilemap tilemap;
        private Outline outline;
        
        private void OnEnable()
        {
            
            
            snapPoint = transform.parent.Find("SnapPoint");
            PlayerController.OnPlayerFails += PlayerFailsHandler;
            
            fail = false;
            StartCoroutine(IfStatement());
        }

        public IEnumerator IfStatement()
        {
            if (conditionDropdown.options[conditionDropdown.value].text.Equals("block ahead"))
            {
                float offset = 0.33f;
                float positionX = player.transform.position.x;
                float positionY = player.transform.position.y;
                
        
                //get positions for nearby tiles
                Vector3[] tilePositions =
                {
                    new Vector3(positionX, (positionY + offset), 0), new Vector3((positionX + offset), positionY,0 ), 
                    new Vector3(positionX, (positionY + offset), 0), new Vector3((positionX + offset), positionY, 0)
        
                };
                
                for (int i = 0; i < tilePositions.Length; i++)
                {
                    //check if tiles exist at the tilePositions
                    if (tilemap.GetTile(tilemap.WorldToCell(tilePositions[i])) is not null)
                    {
                        for (int j = 0; j < snapPoint.childCount; j++)
                        {
                            if (fail)
                                break;

                            Transform block = snapPoint.GetChild(j);

                            block.GetChild(0).gameObject.SetActive(true);
                            outline =  block.GetComponent<Outline>();
                            outline.enabled = true;
                            
                            yield return
                                new WaitUntil(() =>
                                    !block.GetChild(0).gameObject
                                        .activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                            yield return new WaitForSeconds(secondsToWait); //wait until animation ends

                            outline.enabled = false;
                        }
                    }
                    
                    PlayerController.OnPlayerFails -= PlayerFailsHandler;
                    gameObject.SetActive(false);
                    break;
                }
            }
        }
        
        void PlayerFailsHandler()
        {
            outline.enabled = false;
            fail = true;
            StopAllCoroutines();
            PlayerController.OnPlayerFails -= PlayerFailsHandler;

            gameObject.SetActive(false);
        }
        
        public void OnDisable()
        {
            PlayerController.OnPlayerFails -= PlayerFailsHandler;
        }
    }
}
