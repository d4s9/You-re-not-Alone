using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class boss_damage_collider : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] int damage = 25;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Player")
        {
            other.GetComponent<Player>().PlayerDamage(damage);
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
