using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
public class isDraggable : MonoBehaviour
{


    private Vector3 mousePositionOffset;
    Vector3 startingPos;

    private Vector2 extents;
    private TileManager manager;
    

    private void Awake(){
        manager = FindFirstObjectByType<TileManager>();
        extents = GetComponent<SpriteRenderer>().bounds.extents;
    }

    private Vector3 GetMouseWorldPosition(){
        return Camera.main.ScreenToWorldPoint(Input.mousePosition);
    }
    private void OnMouseDown(){
        startingPos = transform.position;
        mousePositionOffset = transform.position - GetMouseWorldPosition();
    }
  
    private void OnMouseDrag(){
        int mytile = manager.GetTileUnderType(GetMouseWorldPosition());

        switch(mytile){
            case 0:
                transform.position = GetMouseWorldPosition() + mousePositionOffset;
                break;
            case 1:
                transform.position = GetMouseWorldPosition() + mousePositionOffset;
                break;
            case 2:
                break;
            default:
                Debug.Log("Tile type isn't 0, 1, or 2...");
                break;
        }
            

        
    }

    private void OnMouseUp(){
        int mytile = manager.GetTileUnderType(GetMouseWorldPosition());

        if(mytile == 0){
            transform.position = startingPos;
        }
    }
    // public void FixedUpdate(){
        
    //     if(Input.GetMouseButtonDown(0)){
    //         RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero, float.PositiveInfinity, movableLayers);
    //         if(hit){
    //             dragging = hit.transform;
    //             mousePositionOffset = dragging.position - Camera.main.ScreenToWorldPoint(Input.mousePosition);
    //             extents = dragging.GetComponent<SpriteRenderer>().sprite.bounds.extents;
    //         }
    //     }
    //     else if(Input.GetMouseButtonUp(0)){
    //         dragging = null;
    //     }
        

    //     if(dragging != null){
    //         Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition) + mousePositionOffset;
            
    //         dragging.position = pos;
    //     }
    // }
}
