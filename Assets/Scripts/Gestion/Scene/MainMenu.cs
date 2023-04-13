using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _infoPannel;
    [SerializeField] private GameObject _mainPannel;
    public void InfoPannel()
    {
        bool isInfo = _infoPannel.active;
        _infoPannel.SetActive(!isInfo);
        _mainPannel.SetActive(isInfo);
    }
}
