using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] GameObject radeau;
    [SerializeField] GameObject posRadeau;
    [SerializeField] GameObject txtActivePuzzle;

    private List<ItemData> contenu = new List<ItemData>();

    void Start()
    {
        contenu = inventaire.getList();
        txtActivePuzzle.SetActive(false);
    }
    void Update()
    {
        allumeFeu(contenu);
        verifBois();
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
                txtActivePuzzle.SetActive(true);
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
                else
                {
                    txtActivePuzzle.SetActive(false);
                }
            }

        }

    }
    
    public void verifBois()
    {
        List<ItemData> listBois = contenu.FindAll(findBois);
        if (listBois.Count >= 5 )
        {
            activeRadeau();
        }
    }
    
    public void activeRadeau()
    {
        RaycastHit hit;
        if (Physics.Raycast(player.transform.position, player.transform.forward, out hit, 2.6f))
        {
            if (hit.transform.CompareTag("Puzzle"))
            {
                txtActivePuzzle.SetActive(true);
                if (Input.GetKeyDown(KeyCode.E))
                {
                    GameObject rad;
                    rad = Instantiate(radeau);
                    radeau.transform.position = posRadeau.transform.position;
                }
            }
            else
            {
                txtActivePuzzle.SetActive(false);
            }

        }
    }
    private static bool findBois(ItemData Bois)
    {

        if (Bois.nom == "Bois")
        {
            return true;
        }
        else
        {
            return false;
        }
    }
}



