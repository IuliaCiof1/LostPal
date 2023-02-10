using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class FinishPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI timeText;

    private float time=0.0f;

    private void Start()
    {
     //   Grid grid = new Grid(1000, 1000, 5);
    }

    void Update()
    {
        if (!timeText.IsActive())
        {
            time += Time.deltaTime;
            timeText.text = "Time: " + Math.Round(time,2) + " s";
        }
    }

    public void Retry()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
