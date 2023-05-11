using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class spawner : MonoBehaviour
{
    [SerializeField] private GameObject ennemiPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deadZomb;
    public bool active = false;
    public float nb_to_spawn = 1f;
    public int nb_spawned = 0;
    void Start()
    {
        for(int i = 0; i < nb_to_spawn; i++)
        {
            spawn();
        }
    }

    void spawn()
    {
        GameObject ennemi = Instantiate(ennemiPrefab, this.transform);
        ennemi.transform.position = RandomPos();
        ennemi.GetComponent<Unit>().SetTarget(player);
        ennemi.GetComponent<Unit>().SetEnnemyDetection(0);
        ennemi.GetComponent<Unit>().SetAngleVision(360);

        RaycastHit hit;
        if(Physics.Raycast(ennemi.transform.position, Vector3.down, out hit, Mathf.Infinity, 15))
        {
            Debug.DrawRay(ennemi.transform.position, Vector3.down * hit.distance, Color.red);
            ennemi.transform.position = new Vector3(ennemi.transform.position.x, ennemi.transform.position.y - hit.distance, ennemi.transform.position.z);
        }
        nb_spawned++;
    }

    void Update()
    {            

       for(int i = 0; i < nb_spawned; i++)
        {
            if (transform.GetChild(i).GetComponent<Unit>().isDead)
            {
                transform.GetChild(i).parent = deadZomb.transform;
                nb_spawned--;
                spawn();
            }
        }    
    }

    private Vector3 RandomPos()
    {
        Vector3 pos;
        do
        {
            float x = Mathf.Clamp(Random.value * 58, 10, 58);
            float z = Mathf.Clamp(Random.value * 13, 0, 13);
            if (z > 1.5f)
                z = -z;
            pos = new Vector3(x, 3, z);
        } while(Physics.BoxCast(pos, new Vector3(0.5f, 1, 0.5f), Vector3.up));
        return pos;
    }
}
