using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InRoomScript : MonoBehaviour
{
    static public float Room_W = 16;
    static public float Room_H = 11;
    static public float Wall = 2;

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
}
