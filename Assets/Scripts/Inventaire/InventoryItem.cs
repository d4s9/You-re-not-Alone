using System.Collections;
using System.Collections.Generic;
using UnityEngine;
include Objet.cs;

public class InventoryItem : MonoBehaviour
{
    [SerializedField] private int nbrItem;
    [SerializedField] private Image itemImg;
    [SerializedField] private TextMeshProUGUI nbrObjTxt;
    [SerializedField] private Objet objet;
}
