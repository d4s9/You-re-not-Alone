using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    [SerializeField] private float _secondsDestroy = 4f; 

    void Start()
    {
        StartCoroutine("RagdollDestroy");
    }
    IEnumerator RagdollDestroy()
    {
        yield return new WaitForSeconds(_secondsDestroy);
        Destroy(this.gameObject);
    }


}
