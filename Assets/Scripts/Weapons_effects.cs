using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Weapons_effects : MonoBehaviour
{
    [SerializeField] int damage_imput = 50;
    [SerializeField] GameObject player;
    [SerializeField] ParticleSystem blood_PS;
    public AudioClip[] clip;
    AudioSource audio;
    //on ne veux pas que le joueur fasse deux fois des dégats pendant la même animation.
    int did_damage = 0;
    int pelte = 0;

    void Start()
    {
        audio = GetComponent<AudioSource>();
    }

    //L'enemie prend des dégat lorsque le joueur le frappe.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemie" && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee"))
        {
            other.GetComponent<Unit>().TakeDamage(30);

            if (audio.isPlaying == false)
            {
                Instantiate(blood_PS, other.transform.position, other.transform.rotation);
                audio.clip = clip[Random.Range(0, 3)];
                audio.Play();
                Thread.Sleep(1);
                //await Task.Delay(1000);
            }
            did_damage = 1;
        }
    }

    void Update()
    {
        if (!(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee")))
        {
            did_damage = 0;
        }



        if (pelte == 0 && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Je pelte"))
        {
            audio.clip = clip[4];
            audio.Play();
            pelte = 1;
        }
        else if (!(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Je pelte")))
        {
            pelte = 0;
        }
    }
}
