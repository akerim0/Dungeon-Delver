using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InRoomScript))]
public class DrayScript : MonoBehaviour,IFacingMover
{
    static public IFacingMover IFM;
    public enum eMode { idle, move, attack, roomTrans}
    [Header("Inscribed")]
    public float speed = 5;
    public float attackDuration = .25f;
    public float attackDelay = .5f;
    public float roomTransDelay = .5f;

    [Header("Dynamic")]
    public int dirHeld = -1;
    public int facing = 1;
    public eMode mode = eMode.idle;
    private float timeAtkDone = 0;
    private float timeAtkNext = 0;
    private float roomTransDone = 0;

    private Rigidbody2D rb;
    private Animator animator;
    private InRoomScript inRoomScript;
    private Vector2 roomTransPos;

    //IFacingMover Implements 
    public bool moving { get { return (mode == eMode.move); } }

    public float gridMult { get { return inRoomScript.gridMult; } }

    public bool isInRoom { get { return inRoomScript.isInRoom; } }

    public Vector2 roomNum { get { return inRoomScript.roomNum; }
        set { inRoomScript.roomNum = value; }
    }
    public Vector2 posInRoom { get => inRoomScript.posInRoom; 
        set =>inRoomScript.posInRoom = value; }
    //

    // Start is called before the first frame update
    void Awake()
    {
        IFM = this;
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        inRoomScript = GetComponent<InRoomScript>();
    }

    // Update is called once per frame
    void Update()
    {
        if(mode == eMode.roomTrans)
        {
            rb.velocity = Vector3.zero;
            animator.speed = 0;
            posInRoom = roomTransPos;
            if (Time.time < roomTransDone) return;
            mode = eMode.idle;
        }
        if (mode == eMode.attack && Time.time >= timeAtkDone)
        {
            mode = eMode.idle;
        }
        if (mode == eMode.idle || mode == eMode.move)
        {
            dirHeld = -1;
            if (Input.GetKey(KeyCode.D)) dirHeld = 0;
            if (Input.GetKey(KeyCode.W)) dirHeld = 1;
            if (Input.GetKey(KeyCode.A)) dirHeld = 2;
            if (Input.GetKey(KeyCode.S)) dirHeld = 3;

            if (dirHeld == -1)
            {
                mode = eMode.idle;
            }
            else
            {
                facing = dirHeld;
                mode = eMode.move;
            }
            if ((Input.GetKey(KeyCode.Return) || Input.GetKey(KeyCode.Mouse0)) && Time.time >= timeAtkNext)
            {
                mode = eMode.attack;
                timeAtkDone = Time.time + attackDuration;
                timeAtkNext = Time.time + attackDelay;

            }
        }
        Vector2 vel = Vector2.zero;
        switch (mode)
        {
            case eMode.attack:
                animator.Play("Dray_Attack_" + facing);
                animator.speed = 0;
                break;
            case eMode.idle:
                animator.Play("Dray_Walk_" + facing);
                animator.speed = 0;
                break;
            case eMode.move:
                animator.Play("Dray_Walk_" + facing);
                switch (facing)
                {
                    case 0:
                        vel = Vector2.right;
                        break;
                    case 1:
                        vel = Vector2.up;
                        break;
                    case 2:
                        vel = Vector2.left;
                        break;
                    case 3:
                        vel = Vector2.down;
                        break;

                }
                animator.speed = 1;
                break;
        }
     
        rb.velocity = vel * speed;

    }

    private void LateUpdate()
    {
        Vector2 gridPosIR = GetGridPosInRoom(0.25f);
        int doorNum;
        for(doorNum = 0; doorNum < 4; doorNum++)
        {
            if(gridPosIR == InRoomScript.Doors[doorNum])
            {
                break;
            }
        }
        if (doorNum > 3 || doorNum != facing) return;
        Vector2 rm = roomNum;
        switch (doorNum)
        {
            case 0:
                rm.x += 1;
                break;
            case 1:
                rm.y += 1;
                break;
            case 2:
                rm.x -= 1;
                break;
            case 3:
                rm.y -= 1;
                break;
        }

        if(0 <= rm.x && rm.x <= InRoomScript.MAX_RM_X)
        {
            if(0<=rm.y && rm.y <= InRoomScript.MAX_RM_Y)
            {
                roomNum = rm;
                roomTransPos = InRoomScript.Doors[(doorNum + 2) % 4];
                posInRoom = roomTransPos;
                mode = eMode.roomTrans;
                roomTransDone = Time.time + roomTransDelay;
            }
        }
    }

    // IFacingMover Implements
    public int GetFacing()
    {
        return facing;
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
