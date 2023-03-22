using System.Collections;
using TMPro;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;

namespace Blocks
{
    public class RepeatBlocks : MonoBehaviour
    {
        [SerializeField] private TMP_InputField inputNumber; //number of iterations
        [SerializeField] private float secondsToWait;
        private Transform SnapPoint;
        private bool fail;
        
        public void OnEnable()
        {
            SnapPoint = transform.parent.Find("SnapPoint");
            PlayerController.OnPlayerFails += PlayerFailsHandler;
            fail = false;
            StartCoroutine(Repeat());
        }

        IEnumerator Repeat()
        {
            int i;
            for (i = 0; i < int.Parse(inputNumber.text); i++)
                {
                    for (int j = 0; j < SnapPoint.childCount; j++)
                    {
                        if (fail)
                            break;

                        Transform block = SnapPoint.GetChild(j);

                        block.GetChild(0).gameObject.SetActive(true);

                        yield return
                            new WaitUntil(() =>
                                !block.GetChild(0).gameObject
                                    .activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                        yield return new WaitForSeconds(secondsToWait); //wait until animation ends
                    }
                }

                if (i == int.Parse(inputNumber.text))
                {
                    gameObject.SetActive(false);
                }
            
        }
        void PlayerFailsHandler()
        {
            fail = true;
            StopAllCoroutines();
            gameObject.SetActive(false);
        }
    }
    
}
