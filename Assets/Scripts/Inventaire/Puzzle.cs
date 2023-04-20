using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class Puzzle : MonoBehaviour
{
    [SerializeField] ItemData Bois;
    [SerializeField] ItemData Batterie;
    [SerializeField] GameObject feuxCamp;
    [SerializeField] GameObject flamme;
    [SerializeField] private Inventaire inventaire;
    [SerializeField] private Player player;
    [SerializeField] GameObject barriereOuvert;
    [SerializeField] GameObject barriereFermer;

    private List<ItemData> contenu = new List<ItemData>();

    void Start()
    {
        contenu = inventaire.getList();
    }
    void Update()
    {
        allumeFeu(contenu);
    }
    public void allumeFeu(List<ItemData> contenu)
    {
       if(contenu.Contains(item:Bois) == true && contenu.Contains(item:Batterie) == true)
        {
            activeFeux();
        }
    }
    public void activeFeux()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 2.6f))
        {
            if (hit.transform.CompareTag("Puzzle"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GameObject feuxActive;
                    GameObject positionBarriere;
                    feuxActive = Instantiate(flamme);
                    feuxActive.transform.position = feuxCamp.transform.position;
                   
                    positionBarriere = Instantiate(barriereOuvert);
                    positionBarriere.transform.position = barriereFermer.transform.position;
                    Destroy(barriereFermer);
                    contenu.Remove(Bois); contenu.Remove(Batterie);
                   
                    inventaire.RefreshContent();
                }
            }

        }
    }
    
    public void verifBois()
    {
        List<ItemData> listBois = contenu.FindAll(item:Bois);
        if()
        {

        }
    }
    
    public void activeRadeau()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 2.6f))
        {
            if (hit.transform.CompareTag("Puzzle"))
            {
                if (Input.GetKeyDown(KeyCode.E))
                {
                   
                }
            }

        }
    }
}


}
