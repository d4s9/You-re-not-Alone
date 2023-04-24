using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class boss : MonoBehaviour
{
    float y = 0;
    [SerializeField] float rotation_speed = 10f;
    [SerializeField] GameObject player;
    float push_angle;
    public CharacterController con;
    public GameObject par;
    public bool kb = false;
    float KnockBackTimer = 0;
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
        float xx = pp.x - pb.x;
        float zz = pp.z - pb.z;
        //push_angle = Mathf.Atan(xx/zz);//angle entre les deux
        Vector3 direction = new Vector3 (xx,0f,zz);
        Vector3 new_pos = (direction * 100);
        KnockBackTimer += Time.deltaTime;
        Vector3 new_location = (new_pos * KnockBackTimer);

        par.GetComponent<CharacterController>().Move(new_location);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Damage" && kb == false)
        {
            kb = true;
            knockback();
        }
    }


    // Update is called once per frame
    void Update()
    {
        //durée du knockback
        if (kb == true)
        {
            KnockBackTimer += Time.deltaTime;

            if (KnockBackTimer >= 0.5)
            {
                kb = false;
                KnockBackTimer = 0;
            }
        }


        if (par.GetComponent<Unit>().isDead == false)
        {
            rotate();
        }
        else
        {

        }
    }
}
