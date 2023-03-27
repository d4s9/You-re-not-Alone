using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.UIElements;

public class Unit : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float ennemyDetectionDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float angleVision;
    [SerializeField] private float speed = 3;
    
    private bool _following = false;
    Vector3[] path;
    int targetIndex;

    private void Update()
    {
        PlayerDetection();
    }

    public void PlayerDetection()
    {
        Vector3 center = this.transform.position + this.transform.forward * (ennemyDetectionDistance / 2);
        Vector3 heading = new Vector3(target.position.x - this.transform.position.x, 0, target.position.z - this.transform.position.z);
        float distance = heading.magnitude;
        Vector3 direction = heading / distance;
        CharacterController playerCharCont = target.GetComponent<CharacterController>();
        CharacterController ennemyCharCont = this.gameObject.GetComponent<CharacterController>();
        RaycastHit hit;

        Vector3 p1 = transform.position + ennemyCharCont.center + Vector3.up * -ennemyCharCont.height * 0.5f;
        Vector3 p2 = p1 + Vector3.up * ennemyCharCont.height;
        
        if (_following)
        {     
            if(Physics.CheckBox(this.transform.position, Vector3.one * ennemyDetectionDistance/2, this.transform.rotation, playerLayer))
            {
                if (Physics.CheckCapsule(p1, p2, ennemyCharCont.radius + 0.1f, playerLayer))
                {
                    _following = false;
                    //Attack
                    Debug.Log("Hit");
                }
                else
                {
                    PathRequaestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                }
            }
        }
        
        else if(Physics.CheckBox(center, Vector3.one * (ennemyDetectionDistance/2), this.transform.rotation, playerLayer))
        {
            if (Physics.Raycast(this.transform.position + new Vector3(0, ennemyCharCont.height/2, 0), heading, out hit, distance))
            {
                if (hit.collider == playerCharCont)
                {
                    if (Vector3.Angle(direction, transform.forward) <= angleVision)
                    {                                            
                        if(Physics.CheckCapsule(p1, p2, ennemyCharCont.radius + 0.1f, playerLayer)){
                            _following = false;
                            //Attack
                            Debug.Log("Hit");
                        } else
                        {
                            _following = true;
                            PathRequaestManager.RequestPath(new PathRequest(transform.position, target.position, OnPathFound));
                        }
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
        CharacterController ennemyCharCont = this.gameObject.GetComponent<CharacterController>();
        float _smoothCoef = 0.02f;
        Quaternion rotation = this.transform.rotation;
        Vector3 currentWaypoint = path[0];
        while (true)
        {
            if(Mathf.Abs(transform.position.x - currentWaypoint.x) < 0.2f && Mathf.Abs(transform.position.z - currentWaypoint.z) < 0.2f)
            {
                targetIndex++;
                if(targetIndex >= path.Length)
                {
                    _following = false;
                    yield break;
                }
                currentWaypoint = path[targetIndex];               
            }          
            //Fix rotation whenever it stops moving (leans backwards)
            rotation = Quaternion.Slerp(this.transform.rotation, Quaternion.LookRotation(currentWaypoint - this.transform.position, Vector3.up), _smoothCoef);
            transform.rotation = rotation;
            Vector3 move = new Vector3(currentWaypoint.x - this.transform.position.x, 0, currentWaypoint.z - this.transform.position.z).normalized;
            ennemyCharCont.Move(move * Time.deltaTime * speed);
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
