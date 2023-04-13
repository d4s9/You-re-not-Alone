using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_Manager : MonoBehaviour
{
    private int invSpace = 5;
    [SerializeField] private GameObject _pausePanel;
    private bool _isPaused = false;


    void Start()
    {
        
    }

    void Update()
    {
        PauseGame();
    }

    public void PauseGame()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (_isPaused)
            {
                Time.timeScale = 1;
                _isPaused = false;
                _pausePanel.SetActive(false);
            }
            else
            {
                Time.timeScale = 0;
                _isPaused = true;
                _pausePanel.SetActive(true);
            }
        }
    }

    public void ResumeGame()
    {
        if(_isPaused) 
        {
            Time.timeScale = 1;
            _isPaused = false;
            _pausePanel.SetActive(false);
        }
    }
    
}
