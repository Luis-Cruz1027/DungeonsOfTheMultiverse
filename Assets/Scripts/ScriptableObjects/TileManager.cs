using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap dungeon;
    [SerializeField] private List<TileData> tileDatas;

    [SerializeField] private Dictionary<TileBase, TileData> dataFromTiles;
    private int tileType;
    private Vector2 tilePos;

    private void Awake(){
        dataFromTiles = new Dictionary<TileBase, TileData>();


        foreach(var tileData in tileDatas){
            foreach(var tile in tileData.tiles){
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void Update(){
        
    }

    public int GetTileUnderType(Vector3 WorldPosition){
        Vector3Int gridPositionDungeon = dungeon.WorldToCell(WorldPosition);

        TileBase onTile = dungeon.GetTile(gridPositionDungeon);
        if(onTile == null){
            tileType = 0;
        }
        else{
            tileType = dataFromTiles[onTile].type;
        }
        
        return tileType;
    }
}
