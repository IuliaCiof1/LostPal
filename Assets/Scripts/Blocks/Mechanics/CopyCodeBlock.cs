using UnityEngine;
using UnityEngine.EventSystems;

namespace Blocks.Mechanics
{
    public class CopyCodeBlock : MonoBehaviour, IBeginDragHandler, IEndDragHandler
    {
        private Transform CodeStorage;
        private bool copied = false;
        private void Awake()
        {
            CodeStorage = GameObject.FindGameObjectWithTag("CodeStorage").GetComponent<RectTransform>(); ;
        }
    
        public void OnBeginDrag(PointerEventData eventData)
        {
            if (!copied)
            {
//                Debug.Log("copy");
                GameObject copy = Instantiate(this, transform.position, Quaternion.identity, CodeStorage).gameObject;
                copied = true;
            }
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            if (transform.parent == CodeStorage)
            {
                Destroy(gameObject);
            }
        }
    }
}
