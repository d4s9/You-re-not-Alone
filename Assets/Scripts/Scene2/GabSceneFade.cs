using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GabSceneFade : MonoBehaviour
{
    [SerializeField] private Image FadeImage;
    // Start is called before the first frame update
    void Start()
    {
        FadeImage.gameObject.SetActive(true);
        Destroy(this.gameObject);
    }

}