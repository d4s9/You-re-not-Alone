using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PathFinding : MonoBehaviour
{

    private CameraCollidersScript a;
    [SerializeField] private GameObject _cell = default;
    private GameObject _startCell = default;
    private GameObject _endCell = default;
    [SerializeField] private GameObject _player;
    private GameObject _neighborCell;
    private GameObject[] _pathCells;


    void Start()
    {
        _endCell = CreateCell(transform.position, transform.rotation);
        _startCell = CreateCell(_player.transform.position, _player.transform.rotation);
    }

    void Update()
    {
    
    }

    public GameObject CreateCell(Vector3 cellPos, Quaternion cellRotation)
    {

        return Instantiate(_cell­, cellPos, cellRotation);
    }
}
