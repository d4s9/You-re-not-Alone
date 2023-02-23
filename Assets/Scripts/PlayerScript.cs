using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class PlayerScript : MonoBehaviour
{
    private float _smoothCoef = 0.2f;
    private Quaternion _lookAtRotation;

    void Start()
    {
        
    }

    void Update()
    {
        PlayerRotation();
        
        

    }
    
    private void FixedUpdate()
    {
        Quaternion rotation = Quaternion.Lerp(transform.rotation, _lookAtRotation, _smoothCoef);
        transform.rotation = rotation;
    }
    

    private void PlayerRotation()
    {
        Plane groundPlane = new Plane(Vector3.up, -transform.position.y);
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        float hitDistance;

        if (groundPlane.Raycast(mouseRay, out hitDistance))
        {
            Vector3 lookAtPostion = mouseRay.GetPoint(hitDistance);
            _lookAtRotation = Quaternion.LookRotation(lookAtPostion - transform.position, Vector3.up);
        }

    }
}
