using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap dungeon; // this is a tilemap containing all the tiles for the dungeon
    [SerializeField] private Tilemap Fog; // this tilemap is acting as a mask over our dungeon

    private List<Vector3> positionsInRadius;
    private float radius;
    [SerializeField] private List<TileData> tileDatas;
    private List<Vector3> doorNames;
    public TileBase open;
    public TileBase close;

    [SerializeField] private Dictionary<TileBase, TileData> dataFromTiles;
    private int tileType;
    private Vector2 tilePos;
    private Vector3 mousePOS;

    //Chatgpt
    private string newRoomText;
    private OpenAIController controller;


    private void Awake(){
        radius = 10f;
        positionsInRadius = new List<Vector3>();
        for(float i = -radius; i <= radius; i++){
            for(float j = -radius; j <= radius; j++){
                positionsInRadius.Add(new Vector3(i, j, 0f));
            }
        }
        

        doorNames = new List<Vector3>();
        newRoomText = "The party enters a new room.";
        controller = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<OpenAIController>();

        dataFromTiles = new Dictionary<TileBase, TileData>();


        foreach(var tileData in tileDatas){
            foreach(var tile in tileData.tiles){
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector3Int POS = dungeon.WorldToCell(mousePOS);
            TileBase onTile = dungeon.GetTile(POS);
            if (dataFromTiles[onTile].isDoor == true && !doorNames.Contains(POS))
            {
                Debug.Log("door triggered.");
                controller.GetResponse(newRoomText);
                doorNames.Add(POS);
                dungeon.SetTile(POS, open);
            }
            if (doorNames.Contains(POS))
            {
                if (onTile == open)
                {
                    dungeon.SetTile(POS, close);
                }
                else
                {
                    dungeon.SetTile(POS, open);
                }
            }
        }
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

    public void UpdateFog(Vector3 position){
        Vector3Int currentPlayerPosition = Fog.WorldToCell(position);
        TileBase onTile;
        foreach(var element in positionsInRadius){
            onTile = dungeon.GetTile(dungeon.WorldToCell(position + element));
            if(onTile != null){
                if(dataFromTiles[onTile].type != 0){
                    Fog.SetTile(currentPlayerPosition + Fog.WorldToCell(element), null);
                }
                
            }
        }
    }
}
