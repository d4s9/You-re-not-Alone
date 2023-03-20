using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class diggable : MonoBehaviour
{
    //objet retrouver dans le corps creusé.
    [SerializeField] GameObject loot;
    //nombre de pelletage pour déterrer l'objet.
    [SerializeField] int row = 1;
    int i;//comptage de pelletage

    //créer une list pour les model de troue.

    // Start is called before the first frame update
    void Start()
    {
        Instantiate(loot, transform.position, transform.rotation);
    }

    //change le modele du troue.
    void hole_phase()
    {

    }

    //detecter si le joueur creuse sur le gameobject.
    void dig_detect()
    {
        //si detecte interaction
        //detecter si le joueur creuse sur le gameobject.

        //monter le compteur pour raprocher du nombre de pelletage necessaire.
        i++;
        //changer le modele du troue pour un troue plus profont. **** créer une fonction pour le changement de modele du troue.
        hole_phase();

        //si atteint le nb de pelletage necessaire.
        if (i == row)
        {
            //spawner le loot.
            Instantiate(loot, transform.position, transform.rotation);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
