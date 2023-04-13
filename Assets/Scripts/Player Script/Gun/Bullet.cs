using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    Rigidbody m_Rigidbody;
    [SerializeField] float m_Thrust;

    private void Awake()
    {
        Destroy(gameObject, lifeTime);
        
    }
    

    private void OnCollisionEnter(Collision collision)
    {
        Destroy(gameObject);
    }

}
