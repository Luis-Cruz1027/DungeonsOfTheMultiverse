using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class tileSpawner : MonoBehaviour
{
    private TileManager manager;
    [SerializeField] private Tilemap dungeon;
    [SerializeField] private TileBase[] roomWalls;
    [SerializeField] private TileBase[] floorTiles;
    [SerializeField] private TileBase[] doorTiles;

    // Start is called before the first frame update
    void Start()
    {
        manager = FindFirstObjectByType<TileManager>();
        spawnRoom();
        dungeon.RefreshAllTiles();
    }

    // Update is called once per frame
    void spawnRoom(){
        string[] lines = System.IO.File.ReadAllLines("Assets/Scripts/DefaultRoom.txt"); // get the default room txt file
        Debug.Log(lines);
        int grid_x = 0, grid_y = 8;

        foreach(string line in lines){
            grid_x = 0;
            foreach(string sval in line.Split()){

                switch(sval){
                    case "0":
                        break;
                    case "1":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                        break;
                    case "2":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                        break;
                    case "3":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[0]);
                        break;
                    case "4":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[0]);
                        break;
                    case "5":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                        break;
                    case "6":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                        break;
                    case "7":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[0]);
                        break;
                    case "8":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                        break;
                    case "9":
                        dungeon.SetTile(new Vector3Int(grid_x, grid_y), roomWalls[0]);
                        break;
                    default:
                        Debug.Log("lol");
                        break;
                }
                grid_x++;
            }
            grid_y--;
        }
    }
}
