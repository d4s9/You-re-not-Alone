using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UiScene2 : MonoBehaviour
{
    [SerializeField] private CharacterController PlayerCC;
    [SerializeField] private TextMeshProUGUI text;
    private float fadeOutTime = 0.2f;
    private float fadeAlpha;

    void Start()
    {
        fadeAlpha = 1f;
    }

    private void Update()
    {
        if (text.gameObject != null && text.gameObject.active)
        {
            FadeIn();
        }
    }

    public void FadeIn()
    {
        fadeAlpha -= fadeOutTime * Time.deltaTime;
        text.color = new Color(255f, 255f, 255f, fadeAlpha);
        if (fadeAlpha <= 0f)
        {
            Destroy(text.gameObject);
            Destroy(gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other == PlayerCC)
        {
            text.gameObject.SetActive(true);
        }
    }
}
