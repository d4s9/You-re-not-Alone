using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [SerializeField] private GameObject _infoPannel;
    [SerializeField] private GameObject _infoContent;
    [SerializeField] private GameObject _mainPannel;
    public void InfoPannel()
    {
        bool isInfo = _infoPannel.active;

        _infoPannel.SetActive(!isInfo);
        if (!isInfo)
        {
            _infoContent.GetComponent<RectTransform>().anchoredPosition = Vector2.one;
        }
        _mainPannel.SetActive(isInfo);
        
    }
}
