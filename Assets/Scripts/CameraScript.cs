using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private GameObject _player = default;
    [SerializeField] private GameObject[] _cameraList;
    private GameObject _currentCamera;
    private GameObject _mainCam;

    void Start()
    {
        _mainCam = _cameraList[0];
        for (int i = 1; i < _cameraList.Length; i++)
        {
            _cameraList[i].SetActive(false);
        }
        _currentCamera = _cameraList[0];
        _currentCamera.gameObject.SetActive(true);
    }

    void Update()
    {
        
    }

    private void FixedUpdate()
    {

    }
    public void OnCollisionEntering(GameObject collider, GameObject VCam)
    {
        _currentCamera.SetActive(false);
        _currentCamera = VCam;
        _currentCamera.SetActive(true);
    }
    public void OnCollisionLeaving()
    {
        _currentCamera.SetActive(false);
        _currentCamera = _mainCam;
        _currentCamera.SetActive(true);
    }
}
