using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IkeyMaster
{
    public int keyCount { get; set; }
    Vector2 pos { get; }
    int GetFacing();

}
