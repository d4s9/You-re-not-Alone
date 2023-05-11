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
    public int nb_spawned = 0;
    void Start()
    {
    }

    void spawn()
    {
        Instantiate(ennemi, this.transform);
        ennemi.transform.localPosition = new Vector3(0, 0, 0);
        ennemi.GetComponent<Unit>().target = player;
        ennemi.GetComponent<Unit>().ennemyDetectionDistance = 100;
        ennemi.GetComponent<Unit>().angleVision = 360;
        nb_spawned++;
    }

    void Update()
    {
        
       if (nb_spawned < nb_to_spawn)
        {
            spawn();
        }

        //verifier pour chaque instances.
        for (int l = 0; l < nb_spawned; l++)
        {
            //s'il est mort et qu'un troisième ennemi apparait, le premié disparait.
            if (transform.GetChild(l).GetComponent<Unit>()._zombHealth < 0 && nb_spawned == 3)
            {
                GameObject a = gameObject.transform.GetChild(l).gameObject;
                Destroy(a);
                nb_spawned -= 1;
            }

            else if (transform.GetChild(l).GetComponent<Unit>()._zombHealth <0)
            {
                nb_spawned -= 1;
            }
        }
    }
}
