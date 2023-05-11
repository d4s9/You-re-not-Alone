using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cerceuil : MonoBehaviour
{
    [SerializeField] private GameObject escapeCar;
    [SerializeField] private GameObject spawner;

    //objet retrouver dans le corps creus�.
    public GameObject loot;
    public GameObject sfx;
    //nombre de pelletage pour d�terrer l'objet.
    [SerializeField] int nb;
    [SerializeField] GameObject player;
    public int i;//comptage de pelletage
    private bool diglock = false;
    //cr�er une list pour les model de troue.
    public Mesh[] holePhase;

    // Start is called before the first frame update
    void Start()
    {
        //Instantiate(loot, transform.position, transform.rotation);
    }

    //change le modele du troue.
    void hole_phase(int m_i)
    {
        //le meshfilter est chang�.
        //gameObject.GetComponent<MeshFilter>().mesh = holePhase[m_i];
    }

    void spawn()
    {
        Instantiate(loot, transform.position, transform.rotation);
        Instantiate(sfx, transform.position, transform.rotation);
    }

    //detecter si le joueur creuse sur le gameobject.
    void dig_detect()
    {
        //monter le compteur pour raprocher du nombre de pelletage necessaire.
        i++;
        //changer le modele du troue pour un troue plus profont. **** cr�er une fonction pour le changement de modele du troue.
        hole_phase(i);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage" && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee") && i < nb && diglock == false)
        {
            diglock = true;
            dig_detect();
        }
        // spawn();
        else if (i == nb)
        {
            //spawner le loot.
            spawn();
            escapeCar.SetActive(true);
            spawner.SetActive(true);
            //ne peut plus spawner d'objets.
            i = nb + 1;
        }
        else
        {
        }
    }

    void Update()
    {
        if (!player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee"))
        {
            diglock = false;
        }
    }
}
