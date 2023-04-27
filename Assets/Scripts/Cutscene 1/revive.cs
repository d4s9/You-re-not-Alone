using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class revive : MonoBehaviour
{
    float i;
    float k;
    [SerializeField] float rotation_speed = 500f;
    [SerializeField] float up_speed = 1f;
    [SerializeField] float rot_acc = 3f;

    void Start()
    {
        i = 0f;
        k = gameObject.transform.localPosition.y;
    }

    void rotate()
    {
        i += Time.deltaTime * rotation_speed;
        transform.localRotation = Quaternion.Euler(0f, i, 0f);
        rotation_speed += rot_acc;
    }

    void upp()
    {
        k += Time.deltaTime * up_speed;
        transform.localPosition = new Vector3 (gameObject.transform.position.x, k, gameObject.transform.position.z);
    }

    void Update()
    {
        if (gameObject.transform.position.y < 15)
        {
            rotate();
            upp();
        }
    }
}
