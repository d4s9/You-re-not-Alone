using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventaire : MonoBehaviour
{
     public List<ItemData> contenu = new List<ItemData>();

    public void AddItem(ItemData item)
    {
        contenu.Add(item);
    }

}
