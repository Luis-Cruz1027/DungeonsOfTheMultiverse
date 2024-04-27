using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TileManager : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private Tilemap dungeon; // this is a tilemap containing all the tiles for the dungeon
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
    Vector3Int POS;
    private List<Vector3Int> allTiles;
    private tileSpawner tilespawner;


    private void Awake(){
        doorNames = new List<Vector3>();
        
        allTiles = new List<Vector3Int>();
        
        controller = GameObject.FindGameObjectWithTag("StartMenu").GetComponentInChildren<OpenAIController>();

        dataFromTiles = new Dictionary<TileBase, TileData>();


        foreach(var tileData in tileDatas){
            foreach(var tile in tileData.tiles){
                dataFromTiles.Add(tile, tileData);
            }
        }
    }

    private void Start(){
        tilespawner = GameObject.FindGameObjectWithTag("TileManager").GetComponent<tileSpawner>();
        allTiles = tilespawner.AllTheTiles();

        foreach(var tile in allTiles){
            dungeon.SetTileFlags(tile, TileFlags.None);
            dungeon.SetColor(tile, Color.black);
        }

    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            mousePOS = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            POS = dungeon.WorldToCell(mousePOS);
            TileBase onTile = dungeon.GetTile(POS);
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
            else if (dataFromTiles[onTile].isDoor == true && !doorNames.Contains(POS))
            {
                Debug.Log("door triggered.");
                //append list of monsters to end of new room text
                newRoomText = "The party enters a new room.";
                controller.GetResponse(newRoomText);
                dungeon.SetTile(POS, open);
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

    public Vector3Int getDoorPos(){
        return POS;
    }

    public void disableDoors(Vector3Int center){
        doorNames.Add(new Vector3Int(center.x - 4, center.y, 0));
        doorNames.Add(new Vector3Int(center.x + 4, center.y, 0));
        doorNames.Add(new Vector3Int(center.x, center.y - 4, 0));
        doorNames.Add(new Vector3Int(center.x, center.y + 4, 0));
        Debug.Log("Doors Disabled!");
    }
}
