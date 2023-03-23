using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
     [SerializeField] private List<ItemData> contenu = new List<ItemData>();
     [SerializeField] private GameObject inventoryPanel;



    public void AddItem(ItemData item)
    {
        contenu.Add(item);
    }
    
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }

}
