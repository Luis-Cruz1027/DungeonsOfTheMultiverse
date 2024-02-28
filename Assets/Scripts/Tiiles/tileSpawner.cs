using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

public class tileSpawner : MonoBehaviour
{
    private TileManager manager;
    [SerializeField] private Tilemap dungeon;
    [SerializeField] private TileBase[] roomWalls;
    [SerializeField] private TileBase[] floorTiles;
    [SerializeField] private TileBase[] doorTiles;
    [SerializeField] private TileBase nullTile;
    private List<BoundsInt> generatedRooms;

    // Start is called before the first frame update
    void Awake()
    {
        generatedRooms = new List<BoundsInt>();
        manager = FindFirstObjectByType<TileManager>();
        dungeon.CompressBounds();
        generateDungeon(dungeon.cellBounds, 11, 11);
        spawnRooms();
        dungeon.RefreshAllTiles();
    }
    void Start(){
        foreach(var room in generatedRooms){
            Debug.Log(room);
        }
    }

    // Update is called once per frame
    private void spawnRooms(){
        string[] lines = System.IO.File.ReadAllLines("Assets/Scripts/DefaultRoom.txt"); // get the default room txt file
        
        foreach(var room in generatedRooms){
            int grid_x = room.min.x, grid_y = room.max.y;
            foreach(string line in lines){
                grid_x = room.min.x;
                foreach(string sval in line.Split()){
                    Vector3Int currLocation = new Vector3Int(grid_x, grid_y);
                    switch(sval){
                        case "0":
                            break;
                        case "1":
                            dungeon.SetTile(currLocation, floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                            break;
                        case "2":
                            dungeon.SetTile(currLocation, roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                            break;
                        case "3":
                            dungeon.SetTile(currLocation, roomWalls[0]);
                            break;
                        case "4":
                            dungeon.SetTile(currLocation, roomWalls[0]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(0,180,0)));
                            break;
                        case "5":
                            dungeon.SetTile(currLocation, roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(0,0,90)));
                            break;
                        case "6":
                            dungeon.SetTile(currLocation, roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(0, 0, 270)));
                            break;
                        case "7":
                            dungeon.SetTile(currLocation, roomWalls[0]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(180,0,0)));
                            break;
                        case "8":
                            dungeon.SetTile(currLocation, roomWalls[Random.Range(1, roomWalls.Length - 1)]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(180,0,0)));
                            break;
                        case "9":
                            dungeon.SetTile(currLocation, roomWalls[0]);
                            dungeon.SetTransformMatrix(currLocation, Matrix4x4.Rotate(Quaternion.Euler(180,180,0)));
                            break;
                        default:
                            break;
                    }
                    grid_x++;
                }
                grid_y--;
            }
        }
    }

    private void generateDungeon(BoundsInt spaceToSplit, int minWidth, int minHeight){
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while(roomsQueue.Count > 0){
            var room = roomsQueue.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWidth){
                if(Random.value < .8f){
                    if(room.size.y >= minHeight * 2){
                        SplitHorizonally(minHeight, roomsQueue, room);
                    }
                    else if(room.size.x >= minWidth * 2){
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else{
                        generatedRooms.Add(room);
                    }
                }
                else{
                    if(room.size.x >= minWidth * 2){
                        SplitVertically(minWidth, roomsQueue, room);
                    }
                    else if(room.size.y >= minHeight * 2){
                        SplitHorizonally(minHeight, roomsQueue, room);
                    }
                    else{
                        generatedRooms.Add(room);
                    }
                }
            }
        }
    }

    private static void SplitVertically(int minWidth, Queue<BoundsInt> roomsQueue, BoundsInt room){
        var xSplit = Random.Range(1, room.size.x);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(xSplit, room.size.y, 0));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x + xSplit, room.min.y, 0), new Vector3Int(room.size.x - xSplit, room.size.y, 0));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }

    private static void SplitHorizonally(int minHeight, Queue<BoundsInt> roomsQueue, BoundsInt room){
        var ySplit = Random.Range(1, room.size.y);
        BoundsInt room1 = new BoundsInt(room.min, new Vector3Int(room.size.x, ySplit, 0));
        BoundsInt room2 = new BoundsInt(new Vector3Int(room.min.x, room.min.y + ySplit, 0), new Vector3Int(room.size.x, room.size.y - ySplit, 0));
        roomsQueue.Enqueue(room1);
        roomsQueue.Enqueue(room2);
    }
}
