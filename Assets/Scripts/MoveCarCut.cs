using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCarCut : MonoBehaviour
{
    [SerializeField] private GameObject[] Waypoints;
    int count = 0;
    void Start()
    {
        
    }

    void Update()
    {
        if ((this.transform.position - Waypoints[count].transform.position).x < 2 && (this.transform.position - Waypoints[count].transform.position).z < 2 && count < Waypoints.Length-1)
        {
            count++;
        }
        else if(count < Waypoints.Length)
        {
            moveTowards(Waypoints[count]);
        }       
    }

    private void moveTowards(GameObject waypoint)
    {
        this.transform.LookAt(waypoint.transform.position);
        
        if (count > 0)
        {
            this.transform.position += this.transform.forward * 0.2f;
        } else
        {
            this.transform.position += this.transform.forward * 0.1f;
        }

    }
}
