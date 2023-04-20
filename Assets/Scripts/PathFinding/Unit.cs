using Cinemachine.Utility;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class Unit : MonoBehaviour
{
    [SerializeField] private GameObject target;
    [SerializeField] private float ennemyDetectionDistance;
    [SerializeField] private LayerMask playerLayer;
    [SerializeField] private float angleVision;
    [SerializeField] private float speed = 3;
    [SerializeField] private AnimationClip zombAtt;
    [SerializeField] private float _maxZombHealth = default;
    [SerializeField] private GameObject _healthBar;
    private float _zombHealth;
    public bool isDead = false;   
    Animator animator;
    private bool _following = false, groundEnnemy = false;
    Vector3[] path;
    int targetIndex;

    private void Start()
    {
        _zombHealth = _maxZombHealth;
        _healthBar.GetComponent<Slider>().value = _zombHealth / _maxZombHealth;
        animator = GetComponent<Animator>();
    }
    
    private void Update()
    {
        this.GetComponent<CharacterController>().SimpleMove(Vector3.forward * 0);

        if (isDead == false)
        {
            if (!animator.GetCurrentAnimatorClipInfo(0).Equals(zombAtt))
            {
                PlayerDetection();
            }
        }
        
    }

    public void PlayerDetection()
    {
        Vector3 center = this.transform.position + this.transform.forward * (ennemyDetectionDistance / 2);
        Vector3 heading = new Vector3(target.transform.position.x - this.transform.position.x, 0, target.transform.position.z - this.transform.position.z);
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
                if (Physics.CheckCapsule(p1, p2, ennemyCharCont.radius * this.transform.localScale.x + 0.2f, playerLayer))
                {
                    _following = false;
                    animator.SetBool("isWalking", false);
                    Attack(target);
                }
                else
                {
                    PathRequaestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));
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
                        if(Physics.CheckCapsule(p1, p2, ennemyCharCont.radius * this.transform.localScale.x + 0.2f, playerLayer)){
                            animator.SetBool("isWalking", false);
                            Attack(target);
                        } else
                        {
                            _following = true;
                            PathRequaestManager.RequestPath(new PathRequest(transform.position, target.transform.position, OnPathFound));
                        }
                    }
                }                           
            }
        }
    }
    
    public void TakeDamage(float damage)
    {
         _zombHealth -= damage;
        _healthBar.GetComponent<Slider>().value = _zombHealth/_maxZombHealth;
        if(_zombHealth < 1)
        {
            Die();
        }
    }

    private void Die()
    {
        isDead = true;
    }

    //fix player mouvement

    public void Attack(GameObject p_target)
    {

        animator.SetBool("isAttack", true);
        StartCoroutine("waitAttack");
        
        
        //Remove health from player

    }

    IEnumerator waitAttack()
    {
        yield return new WaitForSeconds((animator.GetCurrentAnimatorStateInfo(0).length/1.5f) - 0.5f);
        animator.SetBool("isAttack", false);
        //TakeDamage(50);
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
        
        animator.SetBool("isWalking", true);
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
                    animator.SetBool("isWalking", false);
                    yield break;
                }
                currentWaypoint = path[targetIndex];               
            }

            Quaternion _lookatRotation = Quaternion.LookRotation(new Vector3(currentWaypoint.x - this.transform.position.x, 0, currentWaypoint.z - this.transform.position.z), Vector3.up);
            rotation = Quaternion.Slerp(this.transform.rotation, _lookatRotation, _smoothCoef);
            transform.rotation = rotation;
            Vector3 move = new Vector3(currentWaypoint.x - this.transform.position.x, 0, currentWaypoint.z - this.transform.position.z).normalized;
            ennemyCharCont.SimpleMove(move * Time.deltaTime * speed);
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
