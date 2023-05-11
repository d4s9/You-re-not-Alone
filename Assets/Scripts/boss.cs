using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.XR;

public class boss : MonoBehaviour
{
        
        float y = 0;
        [SerializeField] float rotation_speed = 500f;
        [SerializeField] GameObject player;
        float push_angle;
        public CharacterController con;
        public GameObject parent;
        
        private float phase1;
        private float phase2;
        private float phase3;
        private float phaseCounter = 0;
        private float maxhealt;
        //public float knockbackForce;
        private Vector3 impact = Vector3.zero;
        public bool kb = false;
        public float KnockBackTime = 5;
        private float KnockBackTimer = 1;
        // Start is called before the first frame update
        void Start()
        {
            maxhealt = (gameObject.GetComponent<Unit>()._maxZombHealth);
           phase1 = maxhealt * 0.25f;       // 25/100
           phase2 = maxhealt * 0.50f;      // 50/100
           phase3 = maxhealt * 0.75f;     // 75/100
        }

        void rotate()
        {
           y += Time.deltaTime * rotation_speed;
           parent.transform.localRotation = Quaternion.Euler(0f, y, 0f);
        }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage" && kb == false && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee"))
        {
            gameObject.GetComponent<Unit>().TakeDamage(other.GetComponent<Weapons_effects>().damage_imput);
            kb = true;
            parent.GetComponent<Ragdoll>().activerag(false);
        }
        
    }


        // Update is called once per frame
        void Update()
        {
            //durée du knockback
            if (kb == true && gameObject.GetComponent<Unit>().isDead == false)
            {
                KnockBackTimer += Time.deltaTime;    

                if (KnockBackTimer >= KnockBackTime)
                {
                    kb = false;
                    KnockBackTimer = 0;
                    parent.GetComponent<Ragdoll>().activerag(true);
                    //gameObject.GetComponent<Unit>().speed += 2f;
                    rotation_speed += 100f;
                }
            }
            else if (gameObject.GetComponent<Unit>().isDead == false && kb == false)
            {
                rotate();
            }
            else
            {

            }
        }
    
  }