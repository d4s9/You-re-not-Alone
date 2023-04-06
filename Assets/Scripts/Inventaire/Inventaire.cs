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
        inventoryPanel.SetActive(false);
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
        if(contenu.Count < 24)
        {
            contenu.Add(item);

        }
        else
        {

        }
        RefreshContent();
    }
    private void RefreshContent()
    {
        for(int i = 0; i < contenu.Count;i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = contenu[i];
            currentSlot.itemVisual.sprite = contenu[i].visuel;
        }
    }
    public bool IsFull()
    {
        return InventorySize < contenu.Count;
    }

}
