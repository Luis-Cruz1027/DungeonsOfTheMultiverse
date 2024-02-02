using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class isDraggable : MonoBehaviour
{
    Vector3 mousePositionOffset;
    
    private Vector3 GetMouseWorldPosition(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }

    private void OnMouseDown(){
        // capture the mouse offset
        mousePositionOffset = gameObject.transform.position - GetMouseWorldPosition();
    }

    private void OnMouseDrag(){
        transform.position = GetMouseWorldPosition() + mousePositionOffset;
    }
}
