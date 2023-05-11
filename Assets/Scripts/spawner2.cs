using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class spawner2 : MonoBehaviour
{
    [SerializeField] GameObject ennemiPrefab;
    [SerializeField] GameObject player;
    [SerializeField] Transform deadCell;
    public bool active = false;
    public float nb_to_spawn = 1f;
    public int nb_spawned = 0;
    void Start()
    {
    }

    void spawn()
    {
        GameObject ennemi = Instantiate(ennemiPrefab, this.transform);
        ennemi.transform.localPosition = new Vector3(0, 0, 0);
        ennemi.GetComponent<Unit>().SetTarget(player);
        ennemi.GetComponent<Unit>().SetEnnemyDetection(100f);
        ennemi.GetComponent<Unit>().SetAngleVision(360f);
        nb_spawned++;
    }

    void Update()
    {
        if (nb_spawned < nb_to_spawn && active == true)
        {
            spawn();
        }
        if (deadCell.childCount > 2)
        {
            GameObject a = deadCell.GetChild(0).gameObject;
            Destroy(a);
        }

        //verifier pour chaque instances.
        for (int l = 0; l < nb_spawned; l++)
        {
            if (transform.GetChild(l).GetComponent<Unit>()._zombHealth < 0)
            {
                nb_spawned -= 1;
                //GameObject a = gameObject.transform.GetChild(l).gameObject;
                gameObject.transform.GetChild(l).gameObject.transform.SetParent(deadCell, false);
            }
        }
    }
}
