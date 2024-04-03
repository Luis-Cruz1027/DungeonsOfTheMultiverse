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
    private bool moved;
    

    private void Awake(){
        extents = GetComponent<SpriteRenderer>().bounds.extents;
        manager = FindFirstObjectByType<TileManager>();
        moved = false;
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
                break;
            case 1:
                transform.position = GetMouseWorldPosition() + mousePositionOffset;
                moved = true;
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

    public bool justMoved(){
        return moved;
    }

    public void changeBack(){
        moved = !moved;
    }
}
