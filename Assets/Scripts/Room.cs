using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    public int xPos;
    public int yPos;

    public int type;

    public int numNeighbours = 0;
    public bool doorTop = false, doorBot = false, doorLeft = false, doorRight = false;

    public Room(int xPos, int yPos){
        this.xPos = xPos;
        this.yPos = yPos;
    }
}
