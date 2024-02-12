using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InRoomScript))]
public class SkeletosScript : EnemiesScript,IFacingMover
{
    [Header("Inscribed : Skeletos")]
    public int speed = 2;
    public float timeThinkMin = 1f;
    public float timeThinkMax = 4f;

    [Header("Dynamic : Skeletos")]
    [Range(0, 4)]
    public int facing = 0;
    public float timeNextDecision = 0;

    private InRoomScript inRoomScript;

    //IFacingMover Implements 
    public bool moving { get => (facing < 4); }
    public float gridMult { get => inRoomScript.gridMult; }
    public bool isInRoom { get => inRoomScript.isInRoom; }
    public Vector2 roomNum
    {
        get { return inRoomScript.roomNum; }
        set { inRoomScript.roomNum = value; }
    }
    public Vector2 posInRoom
    {
        get => inRoomScript.posInRoom;
        set => inRoomScript.posInRoom = value;
    }
    //

    protected override void Awake()
    {
        base.Awake();
        inRoomScript = GetComponent<InRoomScript>();
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.time > timeNextDecision)
        {
            DecideDirection();
        }
        rb.velocity = directions[facing] * speed;
    }
    void DecideDirection()
    {
        facing = Random.Range(0, 5);
        timeNextDecision = Time.time + Random.Range(timeThinkMin, timeThinkMax);
    }

    // IFacingMover Implements
    public int GetFacing()
    {
        return (facing % 4);
    }

    public float GetSpeed()
    {
        return speed;
    }

    public Vector2 GetGridPosInRoom(float mult = -1)
    {
        return inRoomScript.GetGridPosInRoom(mult);
    }
    //
}
