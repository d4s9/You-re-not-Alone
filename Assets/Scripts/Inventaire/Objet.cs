using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="Nouvel objet", menuName ="Inventaire/Créer un objet")]
public class Objet : ScriptableObject
{
 public string nom;
    public string description;
    public Sprite icon;

}

