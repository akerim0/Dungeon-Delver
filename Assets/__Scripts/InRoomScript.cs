using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomScript : MonoBehaviour
{
    static public float Room_W = 16;
    static public float Room_H = 11;
    static public float Wall = 2;
    static Vector2 GRID_OFFSET = new Vector2(0.5f, 0.5f);

    //Doors Info
    static public int MAX_RM_X = 9; //Max boundaries of the map
    static public int MAX_RM_Y = 9;
    static public Vector2[] Doors = new Vector2[]{ new Vector2(14.5f,5.5f),
    new Vector2 (8.0f,9.5f),
    new Vector2(1.5f,5.5f),
    new Vector2(8.0f,1.5f)}; // Doors location in rooms

    [Header("Inscribed")]
    public bool keepInRoom = true;
    public float gridMult = 1;
    public float radius = 0.5f;

    private void LateUpdate()
    {
        if (!keepInRoom) return;
        if (isInRoom) return;

        Vector2 posIR = posInRoom;
        posIR.x = Mathf.Clamp(posIR.x, Wall + radius, Room_W - Wall - radius);
        posIR.y = Mathf.Clamp(posIR.y, Wall + radius, Room_H - Wall - radius);
        posInRoom = posIR;
    }
    public bool isInRoom
    {
        get
        {
            Vector2 posIR = posInRoom;
            if(posIR.x < Wall + radius
               || posIR.x > Room_W - Wall - radius
               || posIR.y < Wall + radius
               || posIR.y > Room_H - Wall - radius)
            {
                return false;
            }
            return true;
        }
    }
    public Vector2 posInRoom
    {
        get
        {
            Vector2 tPos = transform.position;
            Vector2 posIR = new Vector2();
            posIR.x = tPos.x % Room_W;
            posIR.y = tPos.y % Room_H;
            return posIR;
        }
        set
        {
             Vector2 rNum = roomNum;
             Vector2 tPos = new Vector2();
             tPos.x = roomNum.x * Room_W;
             tPos.y = roomNum.y * Room_H;
             transform.position = tPos + value;
        }
    }

    public Vector2 roomNum
    {
        get
        {
            Vector2 tPos = (Vector2)transform.position;
            Vector2 rNum = new Vector2();
            rNum.x = Mathf.Floor(tPos.x / Room_W);
            rNum.y = Mathf.Floor(tPos.y / Room_H);
            return rNum;
        }
        set
        {
            Vector2 rNum = value;
            Vector2 tPos = new Vector2();
            tPos.x = rNum.x * Room_W;
            tPos.y = rNum.y * Room_H;
            transform.position = tPos + posInRoom;
        }
    }

    public Vector2 GetGridPosInRoom(float mult = -1)
    {
        if (mult == -1)
        {
            mult = gridMult;
        }
        Vector2 posIR = posInRoom - GRID_OFFSET;
        posIR /= mult;
        posIR.x = Mathf.Round(posIR.x);
        posIR.y = Mathf.Round(posIR.y);
        posIR *= mult;
        posIR += GRID_OFFSET;
        return posIR;
    }
}
