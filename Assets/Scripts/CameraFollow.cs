using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    //Variable Declaration
    private Transform player;
    [SerializeField] private Vector3 offset;
    [SerializeField] private float turnSpeed = 10f;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("Player").transform;
    }

    void LateUpdate()
    {
        //Camera Translation
        transform.position = player.position + offset;

        //Camera Rotation
        transform.LookAt(player.position);
        if (Input.GetMouseButton(1)) //Right Clicking Mouse 
        {
            offset = Quaternion.AngleAxis(Input.GetAxis("Mouse X") * turnSpeed, Vector3.up) * offset;
        }
        
    }
}
