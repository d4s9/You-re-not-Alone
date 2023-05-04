using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject objet;
    public bool active = false;
    public float nb_to_spawn = 1f;


    void Start()
    {
        
    }

    void spawn()
    {
        Instantiate(objet, transform.position, transform.rotation);
    }

    void Update()
    {
       if (active==true && transform.childCount < nb_to_spawn)
        {
            spawn();
        }
    }
}
