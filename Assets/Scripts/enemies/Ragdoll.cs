using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] ragRigidBody;
    //public GameObject parent;
    public Vector3[] pos_initial = {};
    public Vector3[] rot_initial = {};
    //private Game
    void Start()
    {
        //ragRigidBody = GetComponentsInChildren<Rigidbody>();
        ragState(true);
        
        for (int i = 0; i < ragRigidBody.Length;i++)
        {
            pos_initial[i] = ragRigidBody[i].GetComponent<Transform>().localPosition;
            rot_initial[i] = ragRigidBody[i].GetComponent<Transform>().localEulerAngles;
        }
    }

    public void ragState(bool m_state)
    {
        foreach (var rigidbody in ragRigidBody)
        {
            rigidbody.isKinematic = m_state;
        }
    }

    public void backup()
    {
        for (int i = 0; i <= ragRigidBody.Length; i++)
        {
            ragRigidBody[i].GetComponent<Transform>().localPosition = pos_initial[i];
            ragRigidBody[i].GetComponent<Transform>().localEulerAngles = rot_initial[i];
        }
    }

    void Update()
    {
        if (GetComponent<Unit>().isDead == true)
        {
            gameObject.GetComponent<Unit>().StopAllCoroutines();
            gameObject.GetComponent<Unit>().enabled = false;
            gameObject.GetComponent<CharacterController>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;
            ragState(false);
        }
    }
}
