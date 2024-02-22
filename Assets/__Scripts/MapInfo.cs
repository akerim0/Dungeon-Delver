using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Globalization;

public class MapInfo : MonoBehaviour
{
    static public int W { get; private set; }
    static public int H { get; private set;  }
    static public int [,] Map { get; private set; }
    static public Vector3 Offset = new Vector3(0.5f, 0.5f, 0);

    static public string COLLISIONS { get; private set; }

    [Header("Inscribed")]
    public TextAsset delverCollisions;
    public TextAsset delverLevel;
    // Start is called before the first frame update
    void Start()
    {
        LoadMap();
        COLLISIONS = Utils.RemoveLineEndings(delverCollisions.text);
    }
    void LoadMap()
    {
        string[] lines = delverLevel.text.Split('\n');
        H = lines.Length;
        string[] tileNums = lines[0].Trim().Split(' ');
        W = tileNums.Length;
        Map = new int[W, H];
        
        for(int j = 0; j < H; j++)
        {
            tileNums = lines[j].Trim().Split(' ');
            for(int i=0; i < W; i++)
            {
                if(tileNums[i] == "..")
                {
                    Map[i, j] = 0;
                }
                else
                {
                    Map[i, j] = int.Parse(tileNums[i], NumberStyles.HexNumber);
                }
            }
        }
        TileSwapManager.SwapTiles(Map);
        Debug.Log("Map Size: " + W + " wide by " + H + " high");
    }
    public static BoundsInt Get_Map_Bounds()
    {
        BoundsInt bounds = new BoundsInt(0, 0, 0, W, H, 1);
        return bounds;
    }
}
