using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.Animations;

public class Bullet : MonoBehaviour
{
    [SerializeField] float lifeTime;
    [SerializeField] int damage_imput = 50;
    [SerializeField] ParticleSystem blood_PS;
    public AudioClip[] clip;
    AudioSource audio;

    private void Awake()
    {
        audio = GetComponent<AudioSource>();
        Destroy(gameObject, lifeTime);
    }

    //L'enemie prend des d�gat lorsque le joueur le frappe.
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Enemie")
        {
            Instantiate(blood_PS, other.transform.position, other.transform.rotation);
            Debug.Log("HIT !!!");
            other.GetComponent<Unit>().TakeDamage(damage_imput);
            audio.clip = clip[Random.Range(0, clip.Length)];
            audio.Play();
            Thread.Sleep(1);
            Destroy(gameObject);
        }
        //(félix) les balles ne ce détruisent pas lorsqu'ils touchent l'arme (il entrait en colision avec l'arme lorsque le joueur avancais dans la même direction qu'il tirait).
        if (other.tag == "Weapon")
        {
            Physics.IgnoreCollision(other.GetComponent<Collider>(), gameObject.GetComponent<Collider>());
        }
        else
        {
            Destroy(gameObject);    
        }
    }
    /*
        private void OnCollisionEnter(Collision collision)
        {

            Destroy(gameObject);    
        }
    */




}
