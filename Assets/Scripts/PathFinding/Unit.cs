using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float ennemyDetectionDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float angleVision;
    float speed = 3;
    Vector3[] path;
    int targetIndex;

    void Start()
    {
     
    }
    private void Update()
    {
        PlayerDetection();
    }

    public void PlayerDetection()
    {      
        Vector3 heading = target.position - this.transform.position;
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;

        Vector3 center = this.transform.position + this.transform.forward * (ennemyDetectionDistance/2);
        if(Physics.CheckBox(center, Vector3.one * (ennemyDetectionDistance/2), this.transform.rotation, playerLayer))
        {
            Collider playerCollider = target.GetComponent<Collider>();
            RaycastHit hit;
            if (Physics.Raycast(this.transform.position, this.transform.TransformDirection(direction), out hit, distance))
            {
                if (hit.collider == playerCollider)
                {
                    if (Vector3.Angle(direction, transform.forward) < angleVision)
                    {
                        PathRequaestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                    }
                }                           
            }
        }
    }

    public void OnPathFound(Vector3[] newPath, bool pathSuccessful)
    {
        if (pathSuccessful)
        {
            path = newPath;
            StopCoroutine("FollowPath");
            StartCoroutine("FollowPath");
        }
    }

    IEnumerator FollowPath()
    {
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if(transform.position == currentWaypoint)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    yield break;
                }
                currentWaypoint = path[targetIndex];
            }
            transform.position = Vector3.MoveTowards(transform.position, currentWaypoint, speed*Time.deltaTime);
            yield return null;
        }
    }

    public void OnDrawGizmos()
    {
        if(path != null)
        {
            for(int i = targetIndex; i < path.Length; i++)
            {
                Gizmos.color = Color.black;
                Gizmos.DrawCube(path[i], Vector3.one);

                if(i == targetIndex)
                {
                    Gizmos.DrawLine(transform.position, path[i]);
                }
                else
                {
                    Gizmos.DrawLine(path[i - 1], path[i]);
                }
            }
        }
    }
}
