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


    [SerializeField] private GameObject actionPanel;
    [SerializeField] private GameObject useActionButton;
    [SerializeField] private GameObject deleteActionButton;

    private ItemData itemCurrentlySelected;

    [SerializeField] private Sprite slotVide;
     

    public static Inventaire instance;

    private void Awake()
    {
        instance = this; 
    }

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
            ToolTipSystem.instance.Hide();
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
    public void RefreshContent()
    {

        for(int i = 0; i < inventorySlotsParent.childCount; i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = null;
            currentSlot.itemVisual.sprite = slotVide;
        }
        for(int i = 0; i < contenu.Count;i++)
        {
            Slot currentSlot = inventorySlotsParent.GetChild(i).GetComponent<Slot>();
            currentSlot.item = contenu[i];
            currentSlot.itemVisual.sprite = contenu[i].visuel;
        }
    }
    public bool IsFull()
    {
        return InventorySize == contenu.Count;
    }

    public void OpenActionPanel(ItemData item, Vector3 slotPosition)
    {
        itemCurrentlySelected = item;
        if(item == null) 
        {
            return;
        }

        switch(item.itemType)
        {
            case ItemType.Ressource:
                useActionButton.SetActive(false);
                break;
            case ItemType.Equipement:
                useActionButton.SetActive(true);
                break;

        }
        actionPanel.transform.position = slotPosition;
        actionPanel.SetActive(true);
    }
    public void CloseActionPanel()
    {
        actionPanel.SetActive(false);
        itemCurrentlySelected = null;
    }
    public void UseActionButton()
    {
        print("Utilisation : " + itemCurrentlySelected.name);
        CloseActionPanel();
    }
    public void DestroyActionButton()
    {
        contenu.Remove(itemCurrentlySelected);
        RefreshContent();
        CloseActionPanel();
    }
    public List<ItemData> getList()
    {
        return contenu;
    }
    public ItemData getItemCurrentlySelected()
    {
        return itemCurrentlySelected;
    }
}
