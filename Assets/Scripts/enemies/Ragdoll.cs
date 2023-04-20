using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] ragRB;

    // Start is called before the first frame update
    void Start()
    {
        ragRB = GetComponentsInChildren<Rigidbody>();
        ragState(true);
    }

    void ragState(bool m_state)
    {
        foreach (var rigidbody in ragRB)
        {
            rigidbody.isKinematic = m_state;
        }
    }

    void Update()
    {
        if (GetComponent<Unit>().isDead == true)
        {
            gameObject.GetComponent<Unit>().enabled = false;
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
            
            ragState(false);
        }
    }
}
