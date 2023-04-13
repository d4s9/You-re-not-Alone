using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class Slot : MonoBehaviour , IPointerEnterHandler, IPointerExitHandler
{
    public ItemData item;
    public Image  itemVisual;


  public void OnPointerEnter(PointerEventData eventData)
    {
        if (item != null)
        {
            ToolTipSystem.instance.Show(item.description, item.name);
        }
        
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        ToolTipSystem.instance.Hide();
    }
    public void ClickOnSlot()
    {
        Inventaire.instance.OpenActionPanel(item, transform.position);
    }
}
