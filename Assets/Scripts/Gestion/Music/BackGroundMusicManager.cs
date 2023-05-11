using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField] private GameObject _soundPannel;
    [SerializeField] private int id = 0;
    [SerializeField] private AudioSource pelleSource = default;
    
    private Button _muteButton;
    private AudioSource _audioSource;
    private Slider _soundBar;

    void Start()
    {

        _muteButton = _soundPannel.GetComponentInChildren<Button>();
        _soundBar = _soundPannel.GetComponentInChildren<Slider>();
        //_audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        _muteButton.interactable = (PlayerPrefs.GetInt("Muted") == 1) ? false : true;
        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            //_audioSource.Stop();   
        }      

        if (id == 0)
                pelleSource.volume = PlayerPrefs.GetFloat("Volume");
        _soundBar.value = PlayerPrefs.GetFloat("Volume");
    }

    public void MusicOnOff()
    {
        _muteButton = _soundPannel.GetComponentInChildren<Button>();
        _soundBar = _soundPannel.GetComponentInChildren<Slider>();
        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            _soundBar.value = 0;
            if (id == 0)
                pelleSource.volume = 0;
            PlayerPrefs.SetInt("Muted", 1);
            _muteButton.image.color = Color.red;
            // _audioSource.Pause();
        } else if (PlayerPrefs.GetInt("Muted") == 1 && _soundBar.value != 0)
        {
            if (id == 0)
                pelleSource.volume = _soundBar.value;
            _muteButton.image.color = Color.green;
            PlayerPrefs.SetInt("Muted", 0);
            // _audioSource.Play();
        }
        PlayerPrefs.Save();
    }

    public void MusicVolumeUpdate()
    {
        _soundBar = _soundPannel.GetComponentInChildren<Slider>();
        float value = _soundBar.value;
        if(id == 0)
            pelleSource.volume = value;
        // _audioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        if(value == 0 && PlayerPrefs.GetInt("Muted") == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
            _muteButton.interactable = false;
           // _audioSource.Pause();
        } else if(value > 0 && PlayerPrefs.GetInt("Muted") == 1)
        {
            PlayerPrefs.SetInt("Muted", 0);
            _muteButton.interactable = true;
           // _audioSource.Play();
        }
        PlayerPrefs.Save();
    }

    public void OnApplicationQuit()
    {
        _soundBar = _soundPannel.GetComponentInChildren<Slider>();
        float value = _soundBar.value;
        PlayerPrefs.SetFloat("Volume", value);
        PlayerPrefs.Save();
    }
}
