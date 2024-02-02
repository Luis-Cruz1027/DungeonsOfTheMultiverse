using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wasdCamera : MonoBehaviour
{
    private Vector3 CameraPosition;
    [Header("Camera Speed")]

    public float Speed;
    // Start is called before the first frame update
    void Start()
    {
        CameraPosition = this.transform.position;
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

        this.transform.position = CameraPosition;
    }
}
