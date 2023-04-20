using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BackGroundMusicManager : MonoBehaviour
{
    [SerializeField] private GameObject _soundPannel;
    /*
    [SerializeField] private Sprite[] _volumeLogo;
    */

    private Button _muteButton;
    private AudioSource _audioSource;
    private Slider _soundBar;

    void Start()
    {
        _muteButton = _soundPannel.GetComponentInChildren<Button>();
        _soundBar = _soundPannel.GetComponentInChildren<Slider>();
        //_audioSource = GameObject.FindGameObjectWithTag("Music").GetComponent<AudioSource>();
        _muteButton.image.color = (PlayerPrefs.GetInt("Muted") == 1) ? Color.red : Color.green;
        if(PlayerPrefs.GetInt("Muted") == 1)
            //_audioSource.Stop();
        
        _soundBar.value = PlayerPrefs.GetFloat("Volume");
    }

    public void MusicOnOff()
    {
        if (PlayerPrefs.GetInt("Muted") == 0)
        {
            _soundBar.value = 0;
            PlayerPrefs.SetInt("Muted", 1);
            _muteButton.image.color = Color.red;
           // _audioSource.Pause();
        } else if (PlayerPrefs.GetInt("Muted") == 1 && _soundBar.value != 0)
        {
            _muteButton.image.color = Color.green;
            PlayerPrefs.SetInt("Muted", 0);
           // _audioSource.Play();
        }
        PlayerPrefs.Save();
    }

    public void MusicVolumeUpdate()
    {
        float value = _soundBar.value;
       // _audioSource.volume = value;
        PlayerPrefs.SetFloat("Volume", value);
        if(value == 0 && PlayerPrefs.GetInt("Muted") == 0)
        {
            PlayerPrefs.SetInt("Muted", 1);
            _muteButton.image.color = Color.red;
           // _audioSource.Pause();
        } else if(value > 0 && PlayerPrefs.GetInt("Muted") == 1)
        {
            PlayerPrefs.SetInt("Muted", 0);
            _muteButton.image.color = Color.green;
           // _audioSource.Play();
        }
        PlayerPrefs.Save();
    }
}
