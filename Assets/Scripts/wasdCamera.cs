using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasdCamera : MonoBehaviour
{
    private Vector3 CameraPosition;
    

    private float Speed = .5f;
    // Start is called before the first frame update
    void Start()
    {
        CameraPosition = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W)){
            CameraPosition.y += Speed / 50;
        }
        if(Input.GetKey(KeyCode.A)){
            CameraPosition.x -= Speed / 50;
        }
        if(Input.GetKey(KeyCode.S)){
            CameraPosition.y -= Speed / 50;
        }
        if(Input.GetKey(KeyCode.D)){
            CameraPosition.x += Speed / 50;
        }

        transform.position = CameraPosition;
    }
}
