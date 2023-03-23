using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollidersScript : MonoBehaviour
{
    [SerializeField] private GameObject _colliderVCam;
    [SerializeField] GameObject _player = default;
    private CameraScript _cameraScript;

    void Start()
    {
        _cameraScript = Camera.main.gameObject.GetComponent<CameraScript>();
    }

    void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log(other.gameObject);
        if(other.gameObject == _player)
        {
            _cameraScript.OnCollisionEntering(gameObject, _colliderVCam);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject == _player)
        {
            _cameraScript.OnCollisionLeaving();
        }
    }
}
