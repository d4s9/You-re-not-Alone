using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleSeeThrough : MonoBehaviour
{
    [SerializeField] private static int _posID = Shader.PropertyToID("_Position");
    [SerializeField] private static int _sizeID = Shader.PropertyToID("_Size");

    [SerializeField] private Material _wallMaterial;
    [SerializeField] private Camera _camera;
    [SerializeField] private LayerMask _layerMask;


    void Update()
    {
        var dir = _camera.transform.position - transform.position;
        var ray = new Ray(transform.position, dir.normalized);

        if(Physics.Raycast(ray, 3000, _layerMask))
        {
            _wallMaterial.SetFloat(_sizeID, 1);
        }
        else
        {
            _wallMaterial.SetFloat(_sizeID, 0);
        }

        var view = _camera.WorldToViewportPoint(transform.position);
        _wallMaterial.SetVector(_posID, view);
    }
}
