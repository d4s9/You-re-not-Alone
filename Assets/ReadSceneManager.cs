using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class ReadSceneManager : MonoBehaviour
{
    [SerializeField] private Playable cutScene;
    void Start()
    {
        cutScene.Play();
    }

    void Update()
    {
        if(Time.time > cutScene.GetDuration())
        {
            Debug.Log("Change Scene");
            //this.GetComponent<Scene_Manager>().ChangeScene();
        }
    }
}
