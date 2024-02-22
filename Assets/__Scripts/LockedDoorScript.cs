using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockedDoorScript : MonoBehaviour,ISwappable
{
    static private LockedDoorScript[,] _LockedDoors;
    public Vector2Int mapLoc;
    static private Dictionary<int, DoorInfo> _DOOR_INFO_DICT;

    const int locked_R = 73;
    const int locked_UR = 57;
    const int locked_UL = 56;
    const int locked_L = 72;
    const int locked_DL = 88;
    const int locked_DR = 89;

    //ISwippable Imp
    public GameObject guaranteedDrop { get; set; }
    public int tileNum { get; private set; }
    //

    public class DoorInfo
    {
        public int tileNum;
        public Vector2Int otherHalf;

        public DoorInfo(int tN,Vector2Int oH)
        {
            tileNum = tN;
            otherHalf = oH;
            if (_DOOR_INFO_DICT != null)
            {
                _DOOR_INFO_DICT.Add(tileNum, this);
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        if(_LockedDoors == null)
        {
            BoundsInt mapBounds = MapInfo.Get_Map_Bounds();
            _LockedDoors = new LockedDoorScript[mapBounds.size.x, mapBounds.size.y];
            InitDoorInfoDict();
        }
        mapLoc = Vector2Int.FloorToInt(transform.position);
        _LockedDoors[mapLoc.x, mapLoc.y] = this;
    }
    private void OnCollisionStay2D(Collision2D collision)
    {
        if (GetLockedDoor(mapLoc) == null) return;

        IkeyMaster iKeyM = collision.gameObject.GetComponent<IkeyMaster>();
        if(iKeyM == null) return;
        if (!_DOOR_INFO_DICT.ContainsKey(tileNum))
        {
            Debug.LogError("Door Dict has no Key" + tileNum);
            return;
        }
        DoorInfo myDoor = _DOOR_INFO_DICT[tileNum];
        int reqFacing = GetRequiredFacingToOpenDoor(iKeyM);

        if(iKeyM.keyCount > 0 && iKeyM.GetFacing() == reqFacing)
        {
            iKeyM.keyCount--;
            Destroy(gameObject);
            _LockedDoors[mapLoc.x, mapLoc.y] = null;

            if (myDoor.otherHalf == Vector2Int.zero) return;
            Vector2Int otherHalfLoc = mapLoc + myDoor.otherHalf;
            LockedDoorScript otherLD = GetLockedDoor(otherHalfLoc);
            if(otherLD != null)
            {
                Destroy(otherLD.gameObject);
                _LockedDoors[otherHalfLoc.x, otherHalfLoc.y] = null;
            }
        }
    }

    static LockedDoorScript GetLockedDoor(Vector2Int mLoc)
    {
        if (_LockedDoors == null) return null;
        if (mLoc.x < 0 || mLoc.x >= _LockedDoors.GetLength(0)) return null;
        if (mLoc.y < 0 || mLoc.y >= _LockedDoors.GetLength(1)) return null;

        return _LockedDoors[mLoc.x, mLoc.y];
    }

    int GetRequiredFacingToOpenDoor(IkeyMaster iKeyMa)
    {
        Vector2 relPos = (Vector2)transform.position - iKeyMa.pos;

        if (Mathf.Abs(relPos.x) > Mathf.Abs(relPos.y))
        {
            return (relPos.x > 0) ? 0 : 2;

        }
        else
        {
            return (relPos.y > 0) ? 1 : 3;
        }
    }
    void InitDoorInfoDict()
    {
        _DOOR_INFO_DICT = new Dictionary<int, DoorInfo>();
        new DoorInfo(locked_R, Vector2Int.zero);
        new DoorInfo(locked_UR, Vector2Int.left);
        new DoorInfo(locked_UL, Vector2Int.right);
        new DoorInfo(locked_L, Vector2Int.zero);
        new DoorInfo(locked_DL, Vector2Int.right);
        new DoorInfo(locked_DR, Vector2Int.left);

    }

    //ISwippable Imp
    public void Init(int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;
        SpriteRenderer sRend = GetComponent<SpriteRenderer>();
        sRend.sprite = TileMapManager.Delver_Tiles[fromTileNum].sprite;
        transform.position = new Vector3(tileX, tileY, 0) + MapInfo.Offset;

    }
}
