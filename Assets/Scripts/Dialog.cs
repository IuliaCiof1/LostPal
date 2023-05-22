using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class Dialog : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private TextMeshProUGUI textBox;
    [SerializeField] private string[] lines;
    [SerializeField] private Sprite[] characterImages;
    [SerializeField] private Image imageBox;
    [SerializeField] private float textSpeed;
    
    private int index;

    public delegate void DialogFinish();

    public static event DialogFinish OnDialogFinish;
    
    // Start is called before the first frame update
    void Start()
    {
        textBox.text = string.Empty;
        index = 0;
 
        imageBox.sprite = characterImages[index];
        StartCoroutine(TypeLine());
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (textBox.text == lines[index])
        {
            NextLine();
            imageBox.sprite = characterImages[index];
        }
        else
        {
            StopAllCoroutines();
            textBox.text = lines[index];
        }
            
    }

    IEnumerator TypeLine()
    {
        foreach (char c in lines[index].ToCharArray())
        {
            textBox.text += c;
            yield return new WaitForSeconds(textSpeed);
        }
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            textBox.text = string.Empty;
            StartCoroutine(TypeLine());
            
        }
        else
        {
            OnDialogFinish?.Invoke();
            gameObject.SetActive(false);
        }
    }
}
