using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Threading;
using UnityEngine;

public class Weapons_effects : MonoBehaviour
{
    [SerializeField] public int damage_imput = 50;
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

    void playRandomAudio()
    {
        audio.clip = clip[Random.Range(0, clip.Length - 1)];
        audio.Play();
        did_damage = 1;
    }

    //L'enemie prend des dégat lorsque le joueur le frappe.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemie" && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee"))
        {
            other.GetComponent<Unit>().TakeDamage(30);

            if (did_damage == 0)
            {
                Instantiate(blood_PS, other.transform.position, other.transform.rotation);
                playRandomAudio();
            }
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
            audio.clip = clip[3];
            audio.Play();
            pelte = 1;
        }
        else if (!(player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Je pelte")))
        {
            pelte = 0;
        }
    }
}
