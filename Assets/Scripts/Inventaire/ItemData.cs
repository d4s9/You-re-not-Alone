using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Nouvel objet", menuName ="Inventaire/Créer un objet")]
public class ItemData : ScriptableObject
{
 public string nom;
 public string description;
 public Sprite visuel;
 public GameObject prefab;
 public ItemType itemType;
}

public enum ItemType
{
    Ressource,
    Equipement,

}

