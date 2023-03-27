using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Inventaire : MonoBehaviour
{
     [SerializeField] private List<ItemData> contenu = new List<ItemData>();
     [SerializeField] private GameObject inventoryPanel;
     [SerializeField] private Transform inventorySlotsParent;

     const int InventorySize = 24;

    private void Start()
    {
        RefreshContent();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryPanel.SetActive(!inventoryPanel.activeSelf);
        }
    }
        public void AddItem(ItemData item)
    {
        contenu.Add(item);
        RefreshContent();
    }
    private void RefreshContent()
    {
        for(int i = 0; i < contenu.Count;i++)
        {
            inventorySlotsParent.GetChild(i).GetChild(0).GetComponent<Image>().sprite = contenu[i].visuel;
        }
    }
    public bool IsFull()
    {
        return InventorySize == contenu.Count;
    }

}
