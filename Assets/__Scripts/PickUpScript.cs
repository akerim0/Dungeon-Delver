using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpScript : MonoBehaviour,ISwappable
{
    public enum eType { none,key,health,grappler}
    [Header("Inscribed")]
    public eType itemType;

    private Collider2D colld;
    private const float colliderEnableDelay = 0.5f;

    public GameObject guaranteedDrop { get; set; }
    public int tileNum { get; private set; }

    void Awake()
    {
        colld = GetComponent<Collider2D>();
        colld.enabled = false;
        Invoke(nameof(EnableCollider), colliderEnableDelay);
    }
    void EnableCollider()
    {
        colld.enabled = true;
    }

    //Iswappable Imp
    public virtual void Init(int fromTileNum, int tileX, int tileY)
    {
        tileNum = fromTileNum;
        transform.position = new Vector3(tileX, tileY, 0) + MapInfo.Offset;
    }
    //
}
