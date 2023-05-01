using System;
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
    [SerializeField] private GameObject _playerDeadPanel;

    TimeSpan time;
    private int _score;
    private bool _isPaused = false;
    private bool _mort;

    void Start()
    {
        _score = PlayerPrefs.GetInt("PlayerScore");       
        _mort = false;

        UpdateScore();
    }

    void Update()
    {
        if (!_mort)
        {
            time = TimeSpan.FromSeconds(Time.time);
            _timeTxt.SetText("Time: " + time.ToString("mm':'ss':'ff"));
            PauseGame();
        }       
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
        StartCoroutine("GameEnding");
    }

    IEnumerator GameEnding()
    {
        yield return new WaitForSeconds(2f);
        Time.timeScale = 0;
        _playerDeadPanel.SetActive(true);
    }

    public void FinishedLvl()
    {
        PlayerPrefs.SetInt("PlayerScore", _score);
        PlayerPrefs.SetString("PlayerTime", Time.time.ToString() + float.Parse(PlayerPrefs.GetString("PlayerTime")));
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
