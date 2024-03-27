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
    [SerializeField] private TileBase[] hallWalls;
    private List<BoundsInt> generatedRooms;

    private Dictionary<BoundsInt, Vector3Int> roomCenters;
    private GameObject player;

    // Start is called before the first frame update
    void Awake()
    {
        generatedRooms = new List<BoundsInt>();
        roomCenters = new Dictionary<BoundsInt, Vector3Int>();
        manager = FindFirstObjectByType<TileManager>();
        dungeon.CompressBounds();
        generateDungeon(dungeon.cellBounds, 12, 12);
        spawnRooms();
        spawnHallways();
        dungeon.RefreshAllTiles();
    }
    void Start(){
        player = GameObject.FindGameObjectWithTag("Player");
        setPlayer();
    }

    // Update is called once per frame
    private void spawnRooms(){
        string[] lines = System.IO.File.ReadAllLines("Assets/Scripts/Tiles/DefaultRoom.txt"); // get the default room txt file
        
        foreach(var room in generatedRooms){
            int grid_x = room.min.x, grid_y = room.max.y;
            foreach(string line in lines){
                grid_x = room.min.x;
                foreach(string sval in line.Split()){
                    Vector3Int currLocation = new Vector3Int(grid_x, grid_y);
                    switch(sval){
                        case "c":
                            dungeon.SetTile(currLocation, floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                            roomCenters.Add(room, currLocation);
                            break;
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

    private void spawnHallways(){
        List<BoundsInt> roomsList = new List<BoundsInt>();
        foreach(var room in generatedRooms){
            roomsList.Add(room);
        }

        var currentRoomCenter = roomsList[0];

        roomsList.Remove(currentRoomCenter);

        while(roomsList.Count > 0){
            BoundsInt closest = FindClosestPoint(currentRoomCenter, roomsList);
            roomsList.Remove(closest);
            createHallway(currentRoomCenter, closest);
            currentRoomCenter = closest;
        }
    }

    private BoundsInt FindClosestPoint(BoundsInt currentRoom, List<BoundsInt> roomList){
        BoundsInt closest = new BoundsInt(Vector3Int.zero, Vector3Int.zero);
        float length = float.MaxValue;
        foreach(var position in roomList){
            float currentLength = Vector3Int.Distance(roomCenters[position], roomCenters[currentRoom]);
            if(currentLength < length){
                length = currentLength;
                closest = position;
            }
        }
        return closest;
    }

    private void createHallway(BoundsInt currentRoom, BoundsInt closest){

        // initializing some helpful variables for room centers
        Vector3Int closCenter = roomCenters[closest]; // center of other room
        int tempCenterX = roomCenters[currentRoom].x; // current room x
        int tempCenterY = roomCenters[currentRoom].y; // current room y
        

        // same row, different column (left to right)
        if(tempCenterY == closCenter.y && tempCenterX < closCenter.x){
            tempCenterX += 4;
            setDoor(tempCenterX, tempCenterY, 270);
            tempCenterX++;
            generateLeftToRight(ref tempCenterX, ref tempCenterY,  closCenter);
            setDoor(tempCenterX, tempCenterY, 90);
        }

        //same row, different column (right to left)
        else if(tempCenterY == closCenter.y && tempCenterX > closCenter.x){
            tempCenterX -= 4;
            setDoor(tempCenterX, tempCenterY, 90);
            tempCenterX++;
            generateRightToLeft(ref tempCenterX, ref tempCenterY,  closCenter);
            setDoor(tempCenterX, tempCenterY, 270);
        }

        // same column, different row (bottom to top)
        else if(tempCenterX == closCenter.x && tempCenterY < closCenter.y){
            tempCenterY += 4;
            setDoor(tempCenterX, tempCenterY);
            tempCenterY++;
            generateUp(ref tempCenterX, ref tempCenterY,  closCenter);
            setDoor(tempCenterX, tempCenterY, 180);
        }

        // same column, different row (top to bottom)
        else if(tempCenterX == closCenter.x && tempCenterY > closCenter.y){
            tempCenterY -= 4;
            setDoor(tempCenterX, tempCenterY, 180);
            tempCenterY--;
            generateDown(ref tempCenterX, ref tempCenterY,  closCenter);
            setDoor(tempCenterX, tempCenterY);
        }

        // different row different column. This is the case where the room we're in is bottom left
        else if(tempCenterY < closCenter.y && tempCenterX < closCenter.x){
            if(closCenter.y - tempCenterY > 11){
                tempCenterY += 4;
                setDoor(tempCenterX, tempCenterY);
                tempCenterY++;
                generateLeftToRight(ref tempCenterX, ref tempCenterY,  closCenter, true);
                generateUp(ref tempCenterX, ref tempCenterY,  closCenter);
                setDoor(tempCenterX, tempCenterY, 180);
            }
            else{
                tempCenterX += 4;
                setDoor(tempCenterX, tempCenterY, 270);
                tempCenterX++;
                generateUp(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateLeftToRight(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY, 90);
            }
        }

        // different row different column. now the room we're in is top right
        else if(tempCenterY > closCenter.y && tempCenterX > closCenter.x){
            if(tempCenterY - closCenter.y > 11){
                tempCenterY -= 4;
                setDoor(tempCenterX, tempCenterY, 180);
                tempCenterY--;
                generateRightToLeft(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateDown(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY);
            }
            else{
                tempCenterX -= 4;
                setDoor(tempCenterX, tempCenterY, 90);
                tempCenterX--;
                generateDown(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateRightToLeft(ref tempCenterX, ref tempCenterY, closCenter);
                
                setDoor(tempCenterX, tempCenterY, 270);
            }
        }

        // different row different column. top left
        else if(tempCenterY > closCenter.y && tempCenterX < closCenter.x){
            if(tempCenterY - closCenter.y > 11){
                tempCenterY -= 4;
                setDoor(tempCenterX, tempCenterY, 180);
                tempCenterY--;
                generateLeftToRight(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateDown(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY);
            }
            else{
                tempCenterX += 4;
                setDoor(tempCenterX, tempCenterY, 270);
                tempCenterX++;
                generateDown(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateLeftToRight(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY, 90);
            }
        }

        // different row different column. bottom right
        else if(tempCenterY < closCenter.y && tempCenterX > closCenter.x){
            if(closCenter.y - tempCenterY > 11){
                tempCenterY += 4;
                setDoor(tempCenterX, tempCenterY);
                tempCenterY++;
                generateRightToLeft(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateUp(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY, 180);
            }
            else{
                tempCenterX -= 4;
                setDoor(tempCenterX, tempCenterY, 90);
                tempCenterX--;
                generateUp(ref tempCenterX, ref tempCenterY, closCenter, true);
                generateRightToLeft(ref tempCenterX, ref tempCenterY, closCenter);
                setDoor(tempCenterX, tempCenterY, 270);
            }
        }

    }

    private void generateLeftToRight(ref int x,ref  int y,   Vector3Int center, bool offset = false){
        if(offset){
            while(x < center.x){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                x++;
            }
        }
        else{
            while(x <= center.x - 5){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                x++;
            }
        }
    }

    private void generateRightToLeft(ref int x,ref  int y,  Vector3Int center, bool offset = false){
        if(offset){
            while(x > center.x){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                x--;
            }
        }
        else{
            while(x >= center.x + 5){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                x--;
            }
        }
    }

    private void generateUp(ref int x,ref int y,  Vector3Int center, bool offset = false){
        if(offset){
            while(y < center.y){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                y++;
            }
        }
        else{
            while(y <= center.y - 5){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                y++;
            }
        }
        
    }
    
    private void generateDown(ref int x,ref int y,  Vector3Int center, bool offset = false){
        if(offset){
            while(y > center.y){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                y--;
            }
        }
        else{
            while(y >= center.y + 5){
                dungeon.SetTile(new Vector3Int(x, y, 0), floorTiles[Random.Range(0, floorTiles.Length - 1)]);
                y--;
            }
        }
    }

    private void setDoor(int x, int y, int rotation = 0){
        dungeon.SetTile(new Vector3Int(x, y, 0), doorTiles[0]);
        dungeon.SetTransformMatrix(new Vector3Int(x, y, 0), Matrix4x4.Rotate(Quaternion.Euler(0,0,rotation)));
    }

    private void generateDungeon(BoundsInt spaceToSplit, int minWidth, int minHeight){
        Queue<BoundsInt> roomsQueue = new Queue<BoundsInt>();
        roomsQueue.Enqueue(spaceToSplit);
        while(roomsQueue.Count > 0){
            var room = roomsQueue.Dequeue();
            if(room.size.y >= minHeight && room.size.x >= minWidth){
                if(Random.value < .5f){
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

    private string nextTileUp(int x, int y){
        return dungeon.GetTile(new Vector3Int(x, y + 1, 0)).ToString();
    }

    private string nextTileDown(int x, int y){
        return dungeon.GetTile(new Vector3Int(x, y - 1, 0)).ToString();
    }

    private string nextTileRight(int x, int y){
        return dungeon.GetTile(new Vector3Int(x + 1, y, 0)).ToString();
    }

    private string nextTileLeft(int x, int y){
        return dungeon.GetTile(new Vector3Int(x - 1, y, 0)).ToString();
    }

    private void setPlayer(){
        player.transform.position = roomCenters[generatedRooms[0]];
    }

    public Dictionary<BoundsInt, Vector3Int> GetDictionary(){
        return roomCenters;
    }

    public List<BoundsInt> GetList(){
        return generatedRooms;
    }


}
