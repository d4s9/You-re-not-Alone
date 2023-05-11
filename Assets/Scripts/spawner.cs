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
    public int nb_inst=0;
    void Start()
    {
    }

    void spawn()
    {
        Instantiate(ennemi, this.transform);
        ennemi.transform.localPosition = new Vector3 (0,0,0);
        ennemi.GetComponent<Unit>().target = player;
        ennemi.GetComponent<Unit>().ennemyDetectionDistance = 100;
        ennemi.GetComponent<Unit>().angleVision = 360;
    }

    void Update()
    {
        
       if (/*active==true &&*/ i < nb_to_spawn)
        {
            spawn();
            i += 1;
            nb_inst++;
        }
       
        for (int l = 0; l < i; l++)
        {
            GameObject a = gameObject.transform.GetChild(l).gameObject;
            if (transform.GetChild(l).GetComponent<Unit>()._zombHealth < 0)
            {
                i -= 1;
                transform.GetChild(l).GetComponent<Unit>()._zombHealth = 0;
            }
            if (nb_inst == 2 && transform.GetChild(l).GetComponent<Unit>()._zombHealth == 0)
            {
                Destroy(a);
                nb_inst -= 1;
            }
        }
    }
}
