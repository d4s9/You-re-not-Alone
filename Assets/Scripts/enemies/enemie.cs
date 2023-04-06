using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class enemie : MonoBehaviour
{

    [SerializeField] int ptdevie = 1;
    public Collider[] ragCol;

    // Start is called before the first frame update
    void Start()
    {
        //les colliders du ragdoll commencent désactivé.
        for (int i = 0; i < ragCol.Length; i++)
        {
            ragCol[i].enabled = false;
        }
    }

    //activate ragdoll
    void Ragdoll()
    {
        if (ptdevie == 0)
        {
            //désactiver le collider de base en les animation pour laisser place au ragdoll.
            gameObject.GetComponent<CapsuleCollider>().enabled = false;
            gameObject.GetComponent<Animator>().enabled = false;

            for (int i = 0; i < ragCol.Length; i++)
            {
                //ignorer la collision entre les collider du ragdoll et le collider principal.
                Physics.IgnoreCollision(gameObject.GetComponent<CapsuleCollider>(), ragCol[i].GetComponent<Collider>());
                //
                ragCol[i].enabled = true;
            }


        }
        else
        {

        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }
}
