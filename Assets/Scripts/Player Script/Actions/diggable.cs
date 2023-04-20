using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diggable : MonoBehaviour
{
    //objet retrouver dans le corps creusé.
    public GameObject loot;
    //nombre de pelletage pour déterrer l'objet.
    [SerializeField] int nb = 1;
    
    int i;//comptage de pelletage

    //créer une list pour les model de troue.
    public Mesh[] holePhase;

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(loot, transform.position, transform.rotation);
    }

    //change le modele du troue.
    void hole_phase(int m_i)
    {
        //le meshfilter est changé.
        gameObject.GetComponent<MeshFilter>().mesh = holePhase[m_i];
    }

    void spawn()
    {
        Instantiate(loot, transform.position, transform.rotation);
    }

    //detecter si le joueur creuse sur le gameobject.
    void dig_detect()
    {
        //si detecte interaction
        //detecter si le joueur creuse sur le gameobject.

        //monter le compteur pour raprocher du nombre de pelletage necessaire.
        i++;
        //changer le modele du troue pour un troue plus profont. **** créer une fonction pour le changement de modele du troue.
        hole_phase(i);

        //si atteint le nb de pelletage necessaire.
        if (i == nb)
        {
            //spawner le loot.
            spawn();
        }
    }

    /*
    void dig_none()
    {
        hole_phase(0);
    }*/

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "shovel")
        {
            dig_detect();
        }
    }

    void Update()
    {
    }
}
