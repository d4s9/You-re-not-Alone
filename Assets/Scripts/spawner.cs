using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] GameObject ennemi;
    [SerializeField] GameObject player;
    public bool active = false;
    public float nb_to_spawn = 1f;
    private int i = 0;

    void Start()
    {
        
    }

    void spawn()
    {
        Instantiate(ennemi, transform.position, transform.rotation);
        ennemi.GetComponent<Unit>().target = player;
        i++;
    }

    

    void Update()
    {
       if (active==true && i < nb_to_spawn)
        {
            spawn();
        }

       if (ennemi.GetComponent<Unit>()._zombHealth < 0) 
       {
            i -= 1;
            Destroy(ennemi);
       }
    }
}
