using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class playerVisibility : MonoBehaviour
{
    private Tilemap dungeon;
    private int radius = 10;
    private List<Vector3Int> currentRadius;
    private isDraggable dragScript;
    private TileManager manager;

    private void Start(){
        dungeon = GameObject.FindGameObjectWithTag("Dungeon").GetComponent<Tilemap>();
        currentRadius = new List<Vector3Int>();
        dragScript = gameObject.GetComponent<isDraggable>();
        manager = FindFirstObjectByType<TileManager>();
    }
    private void OnMouseUp(){
        if(dragScript.justMoved()){
            updateFog();
            dragScript.changeBack();
        }
    }

    private void updateFog(){
        for(int i = dungeon.WorldToCell(transform.position).x - radius; i <= dungeon.WorldToCell(transform.position).x + radius; i++){
            for(int j = dungeon.WorldToCell(transform.position).y - radius; j <= dungeon.WorldToCell(transform.position).y + radius; j++){
                if(dungeon.GetTile(new Vector3Int(i, j, 0)) != null){
                    currentRadius.Add(new Vector3Int(i, j, 0));
                }
            }
        }

        foreach(var tile in currentRadius){
            dungeon.SetTileFlags(tile, TileFlags.None);
            dungeon.SetColor(tile, Color.white);
            dungeon.SetTileFlags(tile, TileFlags.LockColor);
        }
    }
}
