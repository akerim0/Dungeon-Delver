using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridMoveScript : MonoBehaviour
{
    private IFacingMover IFmover;

    private void Awake()
    {
        IFmover = GetComponent<IFacingMover>();
    }
    private void FixedUpdate()
    {
        if (!IFmover.moving) return;
        int facing = IFmover.GetFacing();
        Vector2 posIR = IFmover.posInRoom;
        Vector2 posIRGrid = IFmover.GetGridPosInRoom();
        float delta = 0;

        if (facing == 0 || facing == 2)
        {
            delta = posIRGrid.y - posIR.y;
        }
        else
        {
            delta = posIRGrid.x - posIR.x;
        }
        if (delta == 0) return;

        float gridAlignSpeed = IFmover.GetSpeed() * Time.fixedDeltaTime;
        gridAlignSpeed = Mathf.Min(gridAlignSpeed, Mathf.Abs(delta));

        if (delta < 0) gridAlignSpeed = -gridAlignSpeed;
        if(facing == 0 || facing == 2)
        {
            posIR.y += gridAlignSpeed;
        }
        else
        {
            posIR.x += gridAlignSpeed;
        }

        IFmover.posInRoom = posIR;
    }
}
