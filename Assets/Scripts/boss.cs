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


        //public float knockbackForce;
        private Vector3 impact = Vector3.zero;
        public bool kb = false;
        public float KnockBackTime = 5;
        private float KnockBackTimer = 1;
        // Start is called before the first frame update
        void Start()
        {

        }

        void rotate()
        {
           y += Time.deltaTime * rotation_speed;
           transform.localRotation = Quaternion.Euler(0f, y, 0f);
        }

        void knockback()
        {
            //RECALCULER LA POSITION***
            Vector3 pp = player.transform.position;
            Vector3 pb = gameObject.transform.position;

            float xx = pb.x - pp.x;
            float zz = pb.y - pp.y;
            //push_angle = Mathf.Atan(xx/zz);//angle entre les deux
            Vector3 direction = pp - pb;
            Vector3 new_pos = (direction * 100);
            KnockBackTimer -= Time.deltaTime;
            Vector3 new_location = (new_pos * KnockBackTimer);

            parent.GetComponent<CharacterController>().Move(new_location.normalized);
        }
    
        private void OnTriggerEnter(Collider other)
        {
              if (other.tag == "Damage" && kb == false && player.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("attack melee"))
              {
                parent.GetComponent<Unit>().TakeDamage(other.GetComponent<Weapons_effects>().damage_imput);
                kb = true;
                parent.GetComponent<Unit>().StopAllCoroutines();
                parent.GetComponent<Unit>().enabled = false;
                parent.GetComponent<CharacterController>().enabled = false;
                parent.GetComponent<Animator>().enabled = false;
                gameObject.GetComponent<Ragdoll>().ragState(false);
        }
        }


        // Update is called once per frame
        void Update()
        {
            //durée du knockback
            if (kb == true)
            {
                
                KnockBackTimer += Time.deltaTime;    

                if (KnockBackTimer >= KnockBackTime)
                {
                    kb = false;
                    KnockBackTimer = 0;
                    parent.GetComponent<Unit>().enabled = true;
                    StartCoroutine(parent.GetComponent<Unit>().FollowPath());
                    parent.GetComponent<CharacterController>().enabled = true;
                    parent.GetComponent<Animator>().enabled = true;
                    gameObject.GetComponent<Ragdoll>().ragState(true);
                    gameObject.GetComponent<Ragdoll>().backup();
                }
            }


            if (parent.GetComponent<Unit>().isDead == false && kb == false)
            {
                rotate();
            }
            else
            {

            }
        }
    
  }