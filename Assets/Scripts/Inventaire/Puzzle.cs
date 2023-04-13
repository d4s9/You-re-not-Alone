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
                    Instantiate(flamme);
                    print("allume le feux");

                }
            }

        }
    }


}
