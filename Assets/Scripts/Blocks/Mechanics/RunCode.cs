using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Blocks.Mechanics
{
    public class RunCode : MonoBehaviour
    {
        [SerializeField] private Transform playerBlock;

        [SerializeField] private Transform CodeEditor;

        [SerializeField] private float secondsToWait;
        
        private Transform SnapPoint;
    
        [SerializeField] private GameObject finishPanel;
        [SerializeField] private PlayerController player;

        private Button runBtnComponent;

        [SerializeField] private AudioClip errorSound;
        
        private void Start()
        {
            runBtnComponent = GetComponent<Button>();
        }

        public void Run()
        {
            //get the blocks inside the "Player" block that's inside the CodeEditor
            //only those blocks will be executed
            if (playerBlock.IsChildOf(CodeEditor))
            {
                SnapPoint = playerBlock.Find("SnapPoint");

                StartCoroutine(CoroutineWaitForSeconds());
            }
        
        }

        //Caroutine is needed since for loops move faster than the frames and animation
        public IEnumerator CoroutineWaitForSeconds()
        {
                
                //Get every block inside the Player block and execute it
                for (int i = 0; i < SnapPoint.childCount; i++)
                {
                    Debug.Log("run coroutine");
                    runBtnComponent.enabled = false;
                    Transform block = SnapPoint.GetChild(i);

                    block.GetChild(0).gameObject.SetActive(true);

                    yield return
                        new WaitUntil(() =>
                            !block.GetChild(0).gameObject
                                .activeSelf); //wait until the gameobject on the block is disabled. Needed for repeat blocks
                    yield return new WaitForSeconds(secondsToWait); //wait until animation ends

                }

                player.RestartPosition();
                runBtnComponent.enabled = true;
            
        }

        public void OnEnable()
        {
            PlayerController.OnPlayerWins += PlayerWinHandle;
            PlayerController.OnPlayerFails += PlayerFailsHandler;
        }

        public void OnDisable()
        {
            PlayerController.OnPlayerWins -= PlayerWinHandle;
        }

        void PlayerWinHandle()
        {
            runBtnComponent.enabled = false;
            finishPanel.SetActive(true);
        }

        void PlayerFailsHandler()
        {
            AudioSource audioSource = GetComponent<AudioSource>();
            audioSource.PlayOneShot(errorSound);
            StopAllCoroutines();
            runBtnComponent.enabled = true;
        }
    }
}
