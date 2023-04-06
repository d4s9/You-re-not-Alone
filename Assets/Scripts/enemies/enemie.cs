using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemie : MonoBehaviour
{

    [SerializeField] int ptdevie = 1;
    public Rigidbody[] ragRB;
    public bool state;

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

    /*
    void disableRagdoll()
    {
        foreach (var rigidbody in ragRB)
        {
            rigidbody.isKinematic = true;
        }
    }

    void enableRagdoll()
    {
        foreach (var rigidbody in ragRB)
        {
            rigidbody.isKinematic = false;
        }
    }
    */
 
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
            ragState(false);
        }
    }
}
