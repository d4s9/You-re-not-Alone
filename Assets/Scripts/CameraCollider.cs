using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraCollider : MonoBehaviour
{
    [SerializeField] private GameObject _player = default;
    [SerializeField] private Camera _camera = default;
    private float _colliderLenght;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        _colliderLenght = Mathf.Sqrt(Mathf.Pow(_player.transform.position.x - _camera.transform.position.x, 2) +
            Mathf.Pow(_player.transform.position.y - _camera.transform.position.y, 2) +
            Mathf.Pow(_player.transform.position.z - _camera.transform.position.z, 2));
        transform.localScale = new Vector3(_camera.transform.localScale.x, _camera.transform.localScale.y, _camera.transform.localScale.z * _colliderLenght);
        transform.rotation = _camera.transform.rotation;
        transform.position = _camera.transform.position + _camera.transform.forward * _colliderLenght / 2;
    }
}
