using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Objet : MonoBehaviour
{
 public string nom;
    public string description;
    public int nombre;
    public int nombreMax;
    public Sprite icon;

}
public abstract class Ressource : Objet
{

}
public abstract class Arme : Objet
{
    int degats;
    public int force {private get; set ;}
    public int get_degats() {return degats;}

    public virtual void Equip()
    {
        
    }
}
