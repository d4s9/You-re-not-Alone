using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private GameObject _player = default;

    private Vector3 _camPos = new Vector3(0f, 8f, -2f);
    private Vector3 _camRotation = new Vector3(65f, 0f, 0f);
    private Quaternion _targetRotation;
    void Start()
    {

    }

    void Update()
    {
        
        //transform.position = _player.transform.position + _camPos;
    }

    private void FixedUpdate()
    {

    }

}
