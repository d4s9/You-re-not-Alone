using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ReadSceneManager : MonoBehaviour
{
    private PlayableDirector cutScene;
    void Start()
    {
        cutScene= GetComponent<PlayableDirector>();
        cutScene.Play();
    }

    void Update()
    {
       // if(cutScene.time > )
        if(Time.time > cutScene.duration)
        {
            Debug.Log("Change Scene");
            //this.GetComponent<Scene_Manager>().ChangeScene();
        }
    }
}
