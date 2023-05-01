using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;

public class Ragdoll : MonoBehaviour
{
    public Rigidbody[] ragRigidBody;
    //public GameObject parent;
    public Vector3[] pos_initial = { };
    public Vector3[] rot_initial = { };
    public GameObject unitParent;
    public bool isBoss = false;
    private float charControlerStepOffset;
    //private Game
    void Start()
    {
        //ragRigidBody = GetComponentsInChildren<Rigidbody>();
        ragState(true);
        charControlerStepOffset = unitParent.GetComponent<CharacterController>().stepOffset;
        for (int i = 0; i < ragRigidBody.Length; i++)
        {
            pos_initial[i] = ragRigidBody[i].GetComponent<Transform>().localPosition;
            rot_initial[i] = ragRigidBody[i].GetComponent<Transform>().localEulerAngles;
            Physics.IgnoreCollision(unitParent.GetComponent<CharacterController>(), ragRigidBody[i].GetComponent<Collider>());
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

    public void activerag(bool m_state)
    {

        if (m_state == false)
        {
            unitParent.GetComponent<CharacterController>().stepOffset = 0;
            unitParent.GetComponent<Unit>().StopAllCoroutines();
            unitParent.GetComponent<Unit>().enabled = false;
            unitParent.GetComponent<CharacterController>().enabled = false;
            unitParent.GetComponent<Animator>().enabled = false;
            ragState(false);
        }
        else if (m_state == true)
        {
            unitParent.GetComponent<CharacterController>().stepOffset = charControlerStepOffset;
            unitParent.GetComponent<Unit>().enabled = true;
            StartCoroutine(unitParent.GetComponent<Unit>().FollowPath());
            unitParent.GetComponent<CharacterController>().enabled = true;
            unitParent.GetComponent<Animator>().enabled = true;
            ragState(true);
            backup();
        }
    }

    void Update()
    {
    }
}
