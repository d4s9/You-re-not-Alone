using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{

    [SerializeField] private GameObject player = default;

    private Vector3 camPos = new Vector3(0f, 8f, -2f);
    private Vector3 camRotation = new Vector3(65f, 0f, 0f);
    void Start()
    {
        this.gameObject.transform.position = player.transform.position + camPos;
        this.gameObject.transform.rotation = Quaternion.Euler(player.transform.rotation.x + 65f, 0, 0);
    }

    void Update()
    {
        this.gameObject.transform.rotation = Quaternion.Euler(65f , player.transform.eulerAngles.y, 0);
        this.gameObject.transform.position = player.transform.position + new Vector3(-2 * Mathf.Abs(Mathf.Sin(this.gameObject.transform.eulerAngles.y)), 8f, -2 *Mathf.Abs(Mathf.Cos(this.gameObject.transform.eulerAngles.y)));
        Debug.Log(Mathf.Sin(this.gameObject.transform.eulerAngles.y));
    }
}
