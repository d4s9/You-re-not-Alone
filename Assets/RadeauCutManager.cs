
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Playables;

public class RadeauCutManager : MonoBehaviour
{
    [SerializeField] private Image FadeImage;
    private PlayableDirector cutScene;
    void Start()
    {
        cutScene = GetComponent<PlayableDirector>();
        cutScene.Play();
    }
    void Update()
    {
        if (cutScene.time > 4f && (FadeImage != null))
        {
            StartFade();
        }
        if (Time.time > cutScene.duration)
        {
            Debug.Log("Change Scene");
            //this.GetComponent<Scene_Manager>().ChangeScene();
        }
    }
    private void StartFade()
    {
        FadeImage.gameObject.SetActive(true);
    }

}
