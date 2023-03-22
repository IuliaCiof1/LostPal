using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class FinishPanel : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI timeText;

        private float time;

        [SerializeField] private GameObject play;
        [SerializeField] private GameObject menu;
        [SerializeField] private GameObject settings;
        [SerializeField] private GameObject sound;
        [SerializeField] private GameObject graphics;
    
        [SerializeField] private AudioClip buttonSound;
        private AudioSource audioSource;

        private void Start()
        {
            audioSource = GetComponent<AudioSource>();
        }

        void Update()
        {
            //calculate time until player gets to finish
            if (timeText is not null && !timeText.IsActive())
            {
                time += Time.deltaTime;
                timeText.text = "Time: " + Math.Round(time,2) + " s";
            }
        }

        public void Retry()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void Menu()
        {
            menu.SetActive(true);
            Time.timeScale = 0;
        
        }

        public void Play()
        {
            SceneManager.LoadScene("Level1");
        }
    
    
        public void Resume()
        {
            Time.timeScale = 1;
            menu.SetActive(false);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    
        public void Settings()
        {
            menu.SetActive(false);
            settings.SetActive(true);
        }

        public void SettingsBack()
        {
            menu.SetActive(true);
            settings.SetActive(false);
        }

        public void Sound()
        {
            settings.SetActive(false);
            sound.SetActive(true);
        }
    
        public void SoundBack()
        {
            settings.SetActive(true);
            sound.SetActive(false);
        }
    
        public void Graphics()
        {
            settings.SetActive(false);
            graphics.SetActive(true);
        }
    
        public void GraphicsBack()
        {
            settings.SetActive(true);
            graphics.SetActive(false);
        }
        public void Quit()
        {
            Application.Quit();
        }
    
        public void ButtonClick()
        {
            audioSource.PlayOneShot(buttonSound);
        }
    
    }
}
