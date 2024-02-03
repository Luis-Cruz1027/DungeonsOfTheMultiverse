using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class isDraggable : MonoBehaviour
{
    public LayerMask movableLayers;
    public bool isMoveRestricted = true;
    private Vector3 mousePositionOffset;
    private Transform dragging = null;
    private Vector3 extents;

    public void Update(){
        if(Input.GetMouseButtonDown(0)){
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, movableLayers);
            if(hit){
                dragging = hit.transform;
                mousePositionOffset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
                extents = dragging.GetComponent<SpriteRenderer>().sprite.bounds.extents;
            }
        }
        else if(Input.GetMouseButtonUp(0)){
            dragging = null;
        }

        if(dragging != null){
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mousePositionOffset;
            
            dragging.position = pos;
        }
    }
}
