using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProceduralGenerator : MonoBehaviour
{
    public int height, width, scale, minRooms, maxRooms = 0;

    public int seed, roomCount;
    public GameObject roomPrefab;

    public float randomMulti, randomDistanceMulti, randomMinusMulti, randomNeighbourMulti;

    private Stack<Room> checkRooms = new Stack<Room>();
    private List<Vector2> checkedRooms = new List<Vector2>();

    public Vector2 startRoomLocation;
    System.Random psuedoRandom;

    private void Start()
    {  
        seed =  Random.Range(1,255); 
        psuedoRandom = new System.Random(seed.GetHashCode());

        CreateRooms();     
    }

    private void Update(){
        if (Input.GetMouseButtonDown(0)){
            foreach (Transform child in transform) {
                GameObject.Destroy(child.gameObject);
            }

            roomCount = 0;
            checkedRooms.Clear();

            CreateRooms();
        }
    }

    private void CreateRooms(){ 
        GameObject o = Instantiate(roomPrefab) as GameObject;
        o.transform.parent = transform;
        o.transform.localPosition = new Vector2(width/2, height/2);
        o.transform.localScale = Vector3.one * scale;
        o.GetComponent<MeshRenderer>().material.color = Color.white;

        Room startRoom = o.GetComponent<Room>();
        startRoom.xPos = width/2;
        startRoom.yPos = height/2;
        startRoomLocation = new Vector2(startRoom.xPos, startRoom.yPos);

        roomCount++;
        checkedRooms.Add(new Vector2(startRoom.xPos, startRoom.yPos));
        
        checkRooms.Push(startRoom);
        
        Room currentRoom = checkRooms.Pop();

        while(checkRooms.Count > 0 || roomCount < minRooms){
           
            if(checkRooms.Count > 0){
                currentRoom = checkRooms.Pop();
            }

            CheckRoom(currentRoom, psuedoRandom.Next(0, 100), currentRoom.xPos, currentRoom.yPos + 2, 1);
            CheckRoom(currentRoom, psuedoRandom.Next(0, 100), currentRoom.xPos, currentRoom.yPos - 2, 2);
            CheckRoom(currentRoom, psuedoRandom.Next(0, 100), currentRoom.xPos + 2, currentRoom.yPos, 3);
            CheckRoom(currentRoom, psuedoRandom.Next(0, 100), currentRoom.xPos - 2, currentRoom.yPos, 4);
        }
    }

    private void CheckRoom(Room room, float random, int xPos, int yPos, int direction){
        Room newRoom;

        if(randomMulti * Mathf.Pow(randomNeighbourMulti, room.numNeighbours) < random 
            /** Mathf.Pow(randomDistanceMulti ,Vector2.Distance(new Vector2(xPos, yPos), startRoomLocation))*/){
            return;
        }

        if(roomCount >= maxRooms){
            return;
        }

        if(!checkedRooms.Contains(new Vector2(xPos, yPos))){
            GameObject o = Instantiate(roomPrefab) as GameObject;
            o.transform.parent = transform;
            o.transform.localPosition = new Vector2(xPos, yPos);
            o.transform.localScale = Vector3.one * scale;
            o.GetComponent<MeshRenderer>().material.color = Color.white;

            newRoom = o.GetComponent<Room>();
        
            newRoom.xPos = xPos;
            newRoom.yPos = yPos;

            room.numNeighbours += 1;
            newRoom.numNeighbours += 1;

            if(direction == 1){
                room.doorTop = true;
                newRoom.doorBot = true;
            }
            else if(direction == 2){
                room.doorBot = true;
                newRoom.doorTop = true;
            }
            else if(direction == 3){
                room.doorRight = true;
                newRoom.doorLeft = true;
            }
            else if(direction == 4){
                room.doorLeft = true;
                newRoom.doorRight = true;
            }

            checkedRooms.Add(new Vector2(xPos, yPos));

            checkRooms.Push(newRoom);
            roomCount++;
        }
    }
}
