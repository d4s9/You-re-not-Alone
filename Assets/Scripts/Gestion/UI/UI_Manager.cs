using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    [SerializeField] private GameObject _pausePanel;
    [SerializeField] private TextMeshProUGUI _timeTxt;
    [SerializeField] private TextMeshProUGUI _scoreTxt;
    [SerializeField] private Slider _healthbar; 

    private int _score = 0;
    private int invSpace = 5;
    private bool _isPaused = false;
    private bool _mort;

    void Start()
    {
        _mort = false;
    }

    void Update()
    {
        _timeTxt.SetText("Time : " + Time.time.ToString("00:00.00"));
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

    private void UpdateScore()
    {
        _scoreTxt.SetText("Score : " + _score);
    }

    public void UpdateHealth(int health, int maxHealth)
    {
        _healthbar.value =  health/float.Parse(maxHealth.ToString());
    }

    public void joueurMort()
    {
        _mort = true;
        PlayerPrefs.SetInt("PlayerScore", _score);
        PlayerPrefs.SetString("PlayerTime", Time.time.ToString());

    }

    public void AjouterScore(int scoreAAjouter)
    {
        _score += scoreAAjouter;
        UpdateScore();
    }

    public int GetScore()
    {
        return _score;
    }

    public bool GetJoueurMort()
    {
        return _mort;
    }
    
}
