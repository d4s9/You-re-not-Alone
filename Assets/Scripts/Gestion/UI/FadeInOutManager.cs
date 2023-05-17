using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class FadeInOutManager : MonoBehaviour
{
    [SerializeField] private bool isFadeIn;
    private Scene_Manager sceneManager;
    private float fadeOutTime = 1f;
    private float fadeAlpha;
    void Start()
    {

        sceneManager = FindObjectOfType<Scene_Manager>();
        fadeAlpha = isFadeIn ? 1f : 0f;
    }
    void Update()
    {

        if (isFadeIn)
        {
            FadeIn();
        }
        else
        {
            FadeOut();
        }
    }

    public void FadeIn()
    {
        fadeAlpha -= fadeOutTime * Time.deltaTime;
        this.GetComponent<Image>().color = new Color(0f, 0f, 0f, fadeAlpha);
        if (fadeAlpha <= 0f)
        {
            Destroy(gameObject);
        }
    }
    public void FadeOut()
    {
        fadeAlpha += fadeOutTime * Time.deltaTime;
        this.GetComponent<Image>().color = new Color(0f, 0f, 0f, fadeAlpha);
        if (fadeAlpha >= 1f)
        {
            sceneManager.ChangeScene();
        }
    }
}
