using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Niveau1End : MonoBehaviour
{
    [SerializeField] private CharacterController playerCC;
    [SerializeField] private TextMeshProUGUI escapeTxt;
    [SerializeField] private Image FadeImage;

    private void Update()
    {
        if (escapeTxt.gameObject.active)
        {
            if (Input.GetKeyDown(KeyCode.F))
            {
                FadeImage.gameObject.SetActive(true);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other == playerCC)
        {
            escapeTxt.gameObject.SetActive(true);
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other == playerCC)
        {
            escapeTxt.gameObject.SetActive(false);
        }
    }
}
