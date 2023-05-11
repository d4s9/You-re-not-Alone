using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using TMPro;

public class spawner : MonoBehaviour
{
    [SerializeField] private GameObject ennemiPrefab;
    [SerializeField] private GameObject player;
    [SerializeField] private GameObject deadZomb;
    [SerializeField] private TextMeshProUGUI waveInfo;
    [SerializeField] private GameObject escapeCar;
    [SerializeField] private int id = 0;
    public bool active = false;
    public float nb_to_spawn = 1f;
    public int nb_spawned = 0;
    private int nbrSpawnedKilled = 0;
    void Start()
    {
        if(id == 0)
            waveInfo.gameObject.SetActive(true);
        UpdateWave(0, 31);
        for (int i = 0; i < nb_to_spawn; i++)
        {
            spawn();
        }
    }

    void spawn()
    {
        GameObject ennemi = Instantiate(ennemiPrefab, this.transform);
        ennemi.transform.position = RandomPos();
        ennemi.GetComponent<Unit>().SetTarget(player);
        ennemi.GetComponent<Unit>().SetEnnemyDetection(30);
        ennemi.GetComponent<Unit>().SetAngleVision(360);

        RaycastHit hit;
        if (Physics.Raycast(ennemi.transform.position, Vector3.down, out hit, Mathf.Infinity, 15))
        {
            Debug.DrawRay(ennemi.transform.position, Vector3.down * hit.distance, Color.red);
            ennemi.transform.position = new Vector3(ennemi.transform.position.x, ennemi.transform.position.y - hit.distance, ennemi.transform.position.z);
        }
        nb_spawned++;
        
    }

    void Update()
    {

        for (int i = 0; i < nb_spawned; i++)
        {
            if (transform.GetChild(i).GetComponent<Unit>().isDead)
            {
                transform.GetChild(i).SetParent(deadZomb.transform);
                nb_spawned--;
                nbrSpawnedKilled++;
                if (deadZomb.transform.childCount > 2)
                {
                    Destroy(deadZomb.transform.GetChild(0).gameObject);
                }
                if (id == 0)
                    UpdateWave(nbrSpawnedKilled, 31);
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
        } while (Physics.BoxCast(pos, new Vector3(0.5f, 1, 0.5f), Vector3.up));
        return pos;
    }
    private void UpdateWave(int p_nbrKilled, int nbrWave)
    {
        waveInfo.SetText(p_nbrKilled + "/" + nbrWave);
        if(p_nbrKilled == nbrWave)
        {
            escapeCar.SetActive(true);
        }
    }
}
