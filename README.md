# Dungeons of the Multiverse
![unity logo](https://upload.wikimedia.org/wikipedia/commons/8/8a/Official_unity_logo.png)

# Project Description

<img width="671" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/36de4802-73c3-44d5-927a-a8a0adcb3449">

This project is a collaboration between Luis Cruz and Gilberto Gonzalez.

The aim of this project is to create an application to be used as a tool for players of the tabletop role-playing game, Dungeons and Dragons. Whether the player is new to DND or not, whether they're in a party, it doesn't matter.
The goal is to create a dynamic experience using OpenAI's API to allow the users to interact with chatGPT.

This project is created in the Unity environment and makes use of it's premade tools.

## Dungeon Procedural Generation

To make the game more dynamic, everytime the application is run a map is generated using a version of an algorithm called Binary Space Partitioning. Using Unity's [Tilemaps](https://docs.unity3d.com/ScriptReference/Tilemaps.Tilemap.html)

The algorithm creates instances of BoundsInt (which are members of the TileMap class) from the existing BoundsInt of the Tilemap. It continues cutting the tilemap until it reaches a size appropriate for placing a room.


<img width="290" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/19df57b7-d461-4d7d-833d-5530fc66b33b"> <img width="260" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/f24303cb-7529-4252-ab5d-83974656686b">

<img width="457" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/ddf00de1-aba5-49a0-afc5-a80b896d86e4"> <img width="455" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/91b09e0f-37be-400d-b9e1-6962aa6bc335">

## Dalle-2 Image Generation
During loading, images are generated by DALLE-2 to be used later when an action occurs. In this application an action is

<img width="719" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/1880fc36-cc01-4389-aca3-b0db6eefa220">

<img width="248" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/541aeff0-6b79-4712-9673-63a3858bb3cc"> <img width="137" alt="image" src="https://github.com/Luis-Cruz1027/DungeonsOfTheMultiverse/assets/147957506/9acaf5b1-364e-4a19-b46f-d4aa698b2710">
 



## Current Issues

The application currently has issues and is still being worked on. Some known issues:

- ### Procedural Generation
  - Sometimes the procedural generation will generate a hallway that clips through other rooms. To alleviate this another algorithm for hallways would need to be created. This would most likely have to be some version of A* since we are not concerned with finding the most optimal path as opposed to Dijkstra's algorithm. We just want something that can run faster and create a "correct path"
- ### API Key
  - Since this repository is public, the API key does not currently work. Even if the repository were private, we'd ideally want the API key not to be in the files. For this, we would have to run our own server that houses the API key and make requests and pass the key through encryption. This is of course to prevent others from taking our key and using it maliciously and/or irresponsibly.
- ### Traversal
  - As of right now traversal is physics based (using our own physics using data from the tile). This works fine and walls will not allow you to move around except if your mouse happens to hover over a hallway on the other side, in which case your character teleports to where your mouse cursor is. This can be fixed by implementing a working fog of war and make traversal dependant on the physics AND line of sight. 
