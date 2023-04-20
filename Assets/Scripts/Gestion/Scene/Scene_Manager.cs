using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene_Manager : MonoBehaviour
{
    public void Quit()
    {
        Application.Quit();
    }
    public void ChangeScene()
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(currentIndex+1);
    }
    public void LoadBegenning()
    {
        SceneManager.LoadScene(0);
    }
}
